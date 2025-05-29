using System.CommandLine;
using System.Security.Cryptography;

namespace filehasher;

class Program
{
    enum HashAlgorithm
    {
        MD5,
        SHA1,
        SHA256,
        SHA512
    }
    static void Main(string[] args)
    {
        Option<FileInfo> fileOption = new(["-f", "--file"], "The file path.")
        {
            IsRequired = true
        };
        Option<HashAlgorithm[]> algsOption = new(["-a", "--algorithms"], () => [HashAlgorithm.MD5, HashAlgorithm.SHA1], "The hash algorithms.");

        RootCommand root = new("The file hash calculator. Uses base64 strings of sha256 and md5 algorithms.");
        root.AddOption(fileOption);
        root.AddOption(algsOption);

        root.SetHandler((file, algs) =>
        {
            if (!file.Exists)
            {
                Console.Error.WriteLine($"The file {file.FullName} does not exist.");
                return;
            }

            if (algs.Length == 0)
            {
                Console.Error.WriteLine("No hash algorithms specified.");
                return;
            }

            using Stream fs = file.OpenRead();
            foreach (HashAlgorithm alg in algs)
            {
                fs.Seek(0, SeekOrigin.Begin);
                Hasher hasher = new(alg, fs);
                Console.WriteLine($"{alg}: {hasher.Process()}");
            }
        }, fileOption, algsOption);
        
        root.Invoke(args);
    }

    private class Hasher(HashAlgorithm alg, Stream stream)
    {
        public string Process()
        {
            switch (alg)
            {
                case HashAlgorithm.MD5:
                    MD5 md5 = MD5.Create();
                    return md5.ComputeHash(stream).ToHex();
                case HashAlgorithm.SHA1:
                    SHA1 sha1 = SHA1.Create();
                    return sha1.ComputeHash(stream).ToHex();
                case HashAlgorithm.SHA256:
                    SHA256 sha256 = SHA256.Create();
                    return sha256.ComputeHash(stream).ToHex();
                case HashAlgorithm.SHA512:
                    SHA512 sha512 = SHA512.Create();
                    return sha512.ComputeHash(stream).ToHex();
                default:
                    throw new ArgumentOutOfRangeException(nameof(alg), alg, null);
            }
        }
    }
}

public static class Extensions
{
    public static string ToHex(this byte[] bytes)
    {
        return Convert.ToHexString(bytes);
    }
}