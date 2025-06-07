using System.CommandLine;
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
        
        Option<HashAlgorithm> algsOption = new(["--algorithm", "--alg", "-a"], "The hash algorithms.")
        {
            IsRequired = true
        };

        Option<FileInfo?> hashFileOption = new(["--hash", "-h"], () => null, "The file with hash. If not specified, stdin will be used.");

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

            string? input;
            if (hashFile == null)
            {
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.Error.WriteLine("No hash file specified.");
                    return;
                }
            }
            else
            {
                if (!hashFile.Exists)
                {
                    Console.Error.WriteLine($"The hash file {hashFile.FullName} does not exist.");
                    return;
                }
                input = File.ReadAllText(hashFile.FullName).Trim();
            }
            HashComparer comparer = new(file);
            int result = comparer.Compare(input, alg, out string hash);

            Console.WriteLine(result == 0 ? "Hashes are equal!" : "Hashes are not equal!");
            Console.WriteLine($"File: {hash}{Environment.NewLine}Hash: {input}");

        }, fileOption, algsOption, hashFileOption);
        

        rootCommand.Invoke(args);
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