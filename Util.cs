using System.Security.Cryptography;
using System.Text;

namespace SpaceInvaders
{
    internal class Util
    {
        public static string HashPassword(string pwd, string username)
        {
            pwd = $"5p4c3 1nv4d3r5::{pwd}::{username}";

            byte[] dt = Encoding.UTF8.GetBytes(pwd);
            for (int i = 0; i < dt.Length; i++)
                dt[i] = (byte)(~dt[i] | dt[i] >> 2 & dt[i] << 2);

            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] bytes = sha512Hash.ComputeHash(dt);

                StringBuilder sb = new StringBuilder();
                foreach (byte t in bytes)
                    sb.Append(t.ToString("x"));

                return sb.ToString();
            }
        }
    }
}