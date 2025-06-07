using System.Security.Cryptography;

namespace HashLib;

public class Hasher(HashAlgorithm alg, Stream stream)
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


public enum HashAlgorithm
{
    MD5,
    SHA1,
    SHA256,
    SHA512
}

public static class HashByteArrayExtensions
{
    public static string ToHex(this byte[] bytes)
    {
        return Convert.ToHexString(bytes);
    }
}