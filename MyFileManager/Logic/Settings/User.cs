using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MyFileManager
{
    [Serializable]    
    class User
    {
        private byte[] hashPassword;
        private byte[] hashLogin;
        public static User LoadFromFile()
        {
            var fileName = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "MyLoginPassword.dat");
            BinaryFormatter formatter = new BinaryFormatter();
            Entry entry = Factory.GetEntry(fileName);
            if (entry.Exists)
            {
                using (FileStream fs = entry.FileOpen(FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return (User)formatter.Deserialize(fs);
                }
            }
            else
            {
                return new User();
            }
            //return new User();
        }
        public void WriteToFile()
        {
            var fileName = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "MyLoginPassword.dat");
            BinaryFormatter formatter = new BinaryFormatter();
            Entry entry = Factory.GetEntry(fileName);
            using (FileStream fs = entry.FileOpen(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(fs, this);
            }
        }
        private User()
        {
            MD5 md5 = MD5.Create();
            hashPassword = md5.ComputeHash(GetBytes("123"));
            hashLogin = md5.ComputeHash(GetBytes("123"));
        }
        public bool isCorrect(string iLogin, string iPassword)
        {
            MD5 md5 = MD5.Create();
            var hashILogin = md5.ComputeHash(GetBytes(iLogin));
            var hashIPassword = md5.ComputeHash(GetBytes(iPassword));
            return hashPassword.SequenceEqual(hashIPassword) && hashPassword.SequenceEqual(hashIPassword);
        }
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        [OnSerializing]
        private void SetValuesOnSerializing(StreamingContext context)
        {
            Decrypt(hashPassword);
            Decrypt(hashLogin);
        }
        [OnDeserialized]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            Encrypt(hashPassword);
            Encrypt(hashLogin);
        }
        private byte[] Encrypt(byte[] bytes)
        {
            byte[] result = (byte[])bytes.Clone();
            int len =bytes.Length;
            if (len > 1)
            {
                byte x = bytes[0];
                bytes[0] = bytes[len-1];
                bytes[len-1] = x;
            }
            return result;
        }
        private byte[] Decrypt(byte[] bytes)
        {
            byte[] result = (byte[])bytes.Clone();
            int len = bytes.Length;
            if (len > 1)
            {
                byte x = bytes[0];
                bytes[0] = bytes[len-1];
                bytes[len-1] = x;
            }
            return result;  
        }
    }
}
