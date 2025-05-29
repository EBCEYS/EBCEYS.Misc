using System.CommandLine;
using System.Text;

namespace passgen;

// ReSharper disable once ClassNeverInstantiated.Global
class Program
{
    private const byte DefaultLength = 8;
    
    private static readonly char[] Alphabet =
        "abcdefghijklnmopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    private static readonly char[] Specs = "[]{}=+-_!@#$%^&*()".ToCharArray();
    
    private static string l => Environment.NewLine;

    static void Main(string[] args)
    {
        Option<byte> lengthOption = new("--length", () => DefaultLength, $"The password length 0 < length <= {byte.MaxValue}.");
        lengthOption.AddAlias("-l");
        Option<bool> useSpecsOption = new("--specs", () => true, "Spec symbols usage.");
        useSpecsOption.AddAlias("-s");

        RootCommand root = new($"The password generation application.{l}Generates new password (default length is {DefaultLength}) and writes it to stdout without new line.");
        root.AddOption(lengthOption);
        root.AddOption(useSpecsOption);

        root.SetHandler((length, useSpecs) =>
        {
            if (length == 0)
            {
                Console.Error.WriteLine("The password length must be greater than 0.");
                return;
            }
            
            char[] symbols = useSpecs ? Alphabet.Concat(Specs).ToArray() : Alphabet;
            Random random = new();
            StringBuilder sb = new();
            for (int i = 0; i < length; i++)
            {
                sb.Append(random.GetItems(symbols, 1));
            }
            Console.Write(sb.ToString());
        }, lengthOption, useSpecsOption);

        root.Invoke(args);
    }
}