using System.CommandLine;
using System.Text;
using HashLib;

namespace hashcomparer;

class Program
{
    static void Main(string[] args)
    {
        Option<FileInfo> fileOption = new(["--input", "-i"], "The input file to hash calculate.")
        {
            IsRequired = true
        };

        Option<HashAlgorithm> algsOption = new(["--algorithm", "--alg", "-a"], "The hash algorithms.");

        Option<FileInfo?> hashFileOption = new(["--hash", "-h"], () => null,
            "The file with hash. If not specified, stdin will be used.");

        RootCommand rootCommand = new("App compares file hash with input hash by selected algorithms.");

        rootCommand.AddOption(fileOption);
        rootCommand.AddOption(algsOption);
        rootCommand.AddOption(hashFileOption);

        rootCommand.SetHandler((file, alg, hashFile) =>
        {
            if (!file.Exists)
            {
                Console.Error.WriteLine($"The file {file.FullName} does not exist.");
                return;
            }

            if (!TryGetInputHash(hashFile, out string? input) || input == null) return;
            HashComparer comparer = new(file);
            CompareHashes(comparer, input, alg);
        }, fileOption, algsOption, hashFileOption);

        rootCommand.SetHandler((file, hashFile) =>
        {
            if (!file.Exists)
            {
                Console.Error.WriteLine($"The file {file.FullName} does not exist.");
                return;
            }

            if (!TryGetInputHash(hashFile, out string? input) || input == null) return;

            foreach (string line in input.Split(Environment.NewLine).Select(s => s.Trim()).Where(s => s.Length > 0))
            {
                try
                {
                    FileHashInfo hashInfo = FileHashInfo.CreateFromString(line);
                    HashComparer comparer = new(file);
                    CompareHashes(comparer, hashInfo.Hash, hashInfo.Algorithm);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error on reading line: {ex.Message}");
                    return;
                }
            }
        }, fileOption, hashFileOption);


        rootCommand.Invoke(args);
    }

    private static void CompareHashes(HashComparer comparer, string input, HashAlgorithm alg)
    {
        int result = comparer.Compare(input, alg, out string hash);

        Console.WriteLine(result == 0 ? $"{alg} Hashes are equal!" : $"{alg} Hashes are not equal!");
        Console.WriteLine($"File: {hash}{Environment.NewLine}Hash: {input}");
    }

    private static bool TryGetInputHash(FileInfo? hashFile, out string? input)
    {
        if (hashFile == null)
        {
            input = ReadFromStdIn();
            if (!string.IsNullOrEmpty(input)) return true;
            Console.Error.WriteLine("No hash file specified.");
            return false;
        }

        if (!hashFile.Exists)
        {
            Console.Error.WriteLine($"The hash file {hashFile.FullName} does not exist.");
            input = null;
            return false;
        }

        input = File.ReadAllText(hashFile.FullName).Trim();

        return true;
    }

    private static string ReadFromStdIn()
    {
        StringBuilder sb = new();
        string? line;
        while (!((line = Console.ReadLine())?.EndsWith('\0') ?? true) || !string.IsNullOrWhiteSpace(line))
        {
            sb.AppendLine(line);
        }

        return sb.ToString();
    }
}

internal struct FileHashInfo
{
    public HashAlgorithm Algorithm { get; init; }
    public string Hash { get; init; }

    private FileHashInfo(HashAlgorithm algorithm, string hash)
    {
        Algorithm = algorithm;
        Hash = hash;
    }

    /// <summary>
    /// Example format: <br/>
    /// MD5: some_hash
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static FileHashInfo CreateFromString(string hash)
    {
        string[] elems = hash.Split(":").Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();
        if (elems.Length != 2)
        {
            throw new ArgumentException($"Invalid hash format at line {hash}");
        }

        HashAlgorithm alg = Enum.Parse<HashAlgorithm>(elems.First(), true);
        string sHash = elems.Last();
        return new FileHashInfo(alg, sHash);
    }

    public static IEnumerable<FileHashInfo> CreateFromFile(FileInfo file)
    {
        using StreamReader sr = new(file.FullName);
        while (sr.ReadLine() is { } line)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            yield return CreateFromString(line);
        }
    }
}

internal class HashComparer(FileInfo fileInput)
{
    public int Compare(string hash, HashAlgorithm alg, out string fileHash)
    {
        Hasher hasher = new(alg, fileInput.OpenRead());
        fileHash = hasher.Process();
        return string.Compare(fileHash, hash, StringComparison.InvariantCultureIgnoreCase);
    }
}