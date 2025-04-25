using CeramikaAPI.Context;
using CeramikaAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace CeramikaAPI.Services
{
    public class UserService
    {
        private CeramikaContext context;
        public UserService() { context = new CeramikaContext(); }

        private static readonly byte[] StaticSalt = Encoding.UTF8.GetBytes("StalaSol12345678");
        private static readonly byte[] StaticIV = Encoding.UTF8.GetBytes("InicjalVector123");

        public static byte[] DeriveKeyFromPassword(string password, int keySize = 32)
        {
            var newKey = new Rfc2898DeriveBytes(password, StaticSalt, 100_000, HashAlgorithmName.SHA256);
            return newKey.GetBytes(keySize); 
        }

        public static byte[] EncryptToken(string plainText, byte[] key)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = StaticIV;
            aes.Mode = CipherMode.CBC;

            using var encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        public static string GenerateToken(string username)
        {
           
            byte[] key = DeriveKeyFromPassword("passwordofsite");
            

            string payload = $"{username}";
            byte[] encrypted = EncryptToken(payload, key);

            
            byte[] tokenBytes = StaticSalt.Concat(StaticIV).Concat(encrypted).ToArray();
            return Convert.ToBase64String(tokenBytes);
        }

        public static string DecryptToken(string tokenBase64)
        {
            byte[] tokenBytes = Convert.FromBase64String(tokenBase64);

            
            byte[] salt = tokenBytes.Take(16).ToArray(); 
            byte[] iv = tokenBytes.Skip(16).Take(16).ToArray();
            byte[] ciphertext = tokenBytes.Skip(32).ToArray();

            
            byte[] key = DeriveKeyFromPassword("passwordofsite"); 
            byte[] expectedIV = StaticIV;

            
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public bool? VerifyUser(string token)
        {
            var user = UserByName(DecryptToken(token));
            if (user == null) { return null; }
            var search = context.UserRoles.Where(c => (c.Role.Name == "Admin" && c.User.Name == user.Name)).ToList();
            if (search.Any()) { return true; }    
            else { return false; }
                
        }

        public List<UserModel> GetUsers()
        {
            return context.Users.ToList();
        }

        private UserModel? UserByName(string name)
        { 
            try
            {
                return context.Users.First(x => x.Name == name);
            }
            catch { return null; }
        }

        public UserModel? UserById(int id)
        {
            try
            {
                return context.Users.First(x => x.Id == id);
            }
            catch { return null; }
        }

        private UserModel? UserByEmail(string email)
        {
            try
            {
                return context.Users.First(x => x.Email == email);
            }
            catch { return null; }
        }

        public UserModel? AddUser(UserModel user)
        {
            context.Users.Add(user);
            try { context.SaveChanges(); } catch { return null; }
            return user;
        }

        public UserModel? LogUser(string name, string password)
        {
            var user = UserByName(name);
            if (user == null) { return null; }
            if (user.Password != password) { return null; }
            user.Name = GenerateToken(user.Name);
            user.Password = "";
            user.Email = "";


            return user;
        }

        public UserModel? UpdateUser(string name, string password, string? newName = "", string? newPassword = "", string? newEmail = "")
        {
            UserModel? user = LogUser(name, password);
            if (user == null) { return null; }
            if (!string.IsNullOrEmpty(newName)) { user.Name = newName; }
            if (!string.IsNullOrEmpty(newPassword)) { user.Password = newPassword; }
            if (!string.IsNullOrEmpty(newEmail)) { user.Email = newEmail; }
            context.Users.Update(user);
            try
            { context.SaveChanges(); }
            catch { return null; }
            return user;
        }

        public UserModel? DeleteUser(string name, string password)
        {
            var user = LogUser(name, password);
            if (user == null) { return null; }
            context.Users.Remove(user);
            try { context.SaveChanges(); } catch { return null; }
            return user;
        }
    }
}
