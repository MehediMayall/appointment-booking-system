using System.Security.Cryptography;

namespace SharedKernel;


public sealed class Security
{

    public static string CreateMD5Hash(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2")); // "x2" for lowercase hexadecimal
            }
            return sb.ToString();
        }
    }

    public static string GetBase64Hash(string input) => Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
    public static string GetPlanTextFromBase64Hash(string input) => Encoding.UTF8.GetString(Convert.FromBase64String(input));
}