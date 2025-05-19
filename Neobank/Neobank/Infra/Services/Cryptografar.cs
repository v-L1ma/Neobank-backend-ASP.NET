using System.Security.Cryptography;
using System.Text;

namespace Neobank.Services;

public class Cryptografia
{
    private static readonly byte[] chave = Encoding.UTF8.GetBytes("1234567890123456");
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("6543210987654321");
    
    public static string Cryptografar(string conteudo)
    {
        using var aes = Aes.Create();
        aes.Key = chave;
        aes.IV = iv;

        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);

        sw.Write(conteudo);
        sw.Flush();
        cs.FlushFinalBlock();

        byte[] encryptedBytes = ms.ToArray();

        return string.Join("", encryptedBytes.Select(b => b.ToString(("D3"))));
    }

    
    public static string Descryptografar(string conteudo)
    {
        
        byte[] encryptedBytes = Enumerable.Range(0, conteudo.Length / 3)
            .Select(i => byte.Parse(conteudo.Substring(i * 3, 3)))
            .ToArray();
        
        using var aes = Aes.Create();
        aes.Key = chave;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(encryptedBytes);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }

}