using System.CommandLine;
using HashLib;

namespace filehasher;

class Program
{
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
                Console.WriteLine(algs.Length == 1 ? hasher.Process() : $"{alg}: {hasher.Process()}");
            }
        }, fileOption, algsOption);
        
        root.Invoke(args);
    }
}