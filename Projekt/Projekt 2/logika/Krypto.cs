using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Math.Gmp.Native;

namespace Projekt_2.logika
{
    class Krypto
    {

        X509Certificate2 cert;

        private RSA rsaHisPublic;
        private RSA rsaMyPrivate;//= RSA.Create();

        private AesManaged aes;

        public Krypto(X509Certificate2 cert)
        {
            this.cert = cert;
           // RSA rsaprikey = cert.GetRSAPrivateKey();
            rsaMyPrivate = cert.GetRSAPrivateKey();
            aes = new AesManaged();
            aes.GenerateKey();
            aes.GenerateIV();
        }


        public byte[] AESIVGet()
        {
            return aes.IV;
        }
        public void AESIVSet(byte[] iv)
        {
            aes.IV = iv;
        }

        public byte[] AESKeyGet()
        {
            return aes.Key;
        }
        public void AESKeySet(string  key)
        {
            String[] arr = key.Split('-');
            byte[] array = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++) array[i] = Convert.ToByte(arr[i], 16);

            aes.Key = array;
        }

        public byte[] GenAndGetIV()
        {
            aes.GenerateIV();
            return aes.IV;
        }

        public string GetPublicKeyXML()
        {
            RSA t = cert.GetRSAPublicKey();

            return  t.ToXmlString(false);
        }

        public byte[] ZakodujRSA(string s)
        {

            if (rsaHisPublic == null) return Encoding.UTF8.GetBytes(s);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            //byte[] encrypted = rsaHisPublic.EncryptValue(bytes);

            byte[] encrypted = rsaHisPublic.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
            return encrypted;
        }


        public string OdkodujRSA(byte[] s)
        {
            // byte[] decrypted = rsaMyPrivate.DecryptValue(s);
            byte[] decrypted = rsaMyPrivate.Decrypt(s,RSAEncryptionPadding.Pkcs1);

            string wyn = Encoding.UTF8.GetString(decrypted);

            return wyn;
        }
        public byte[] ZakodujAES(string s)
        {
            byte[] encrypted;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            
            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(s);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }
            byte[] finite = new byte[aes.IV.Length + encrypted.Length];
            Buffer.BlockCopy(aes.IV, 0, finite, 0, aes.IV.Length);
            //Console.WriteLine(@"zadodowane: {0}", Encoding.ASCII.GetString(encrypted));
           // Console.Write("zadodowane: "); Print(encrypted);
            Buffer.BlockCopy(encrypted, 0, finite, aes.IV.Length, encrypted.Length);
            //Console.WriteLine(@"zadodowane: {0}", Encoding.ASCII.GetString(finite));
            //Console.Write("zadodowane: "); Print(finite);
            //Console.Write("zadodowane: "); Print(aes.IV);
            return finite;
        }


        public string OdkodujAES(byte[] s) // tu jest błąd
        {
           // Console.WriteLine("odkodowane: {0}", aes.IV.Length);
            string plaintext = null;
            //Console.WriteLine(@"Odkodowane: {0}", Encoding.ASCII.GetString(s));
            //Console.Write("Odkodowane: "); Print(s);
            byte[] iv = new byte[aes.IV.Length];
            Buffer.BlockCopy(s, 0, iv, 0, aes.IV.Length);
            aes.IV = iv;
            byte[] ss = new byte[s.Length - aes.IV.Length];
            Buffer.BlockCopy(s, aes.IV.Length, ss, 0, ss.Length);
            //Console.WriteLine(@"odkodowane: {0}", Encoding.ASCII.GetString(ss));
            //Console.Write("Odkodowane: "); Print(ss);
           // Console.Write("Odkodowane: "); Print(aes.IV);
            //for (int i = aes.IV.Length; i < s.Length; i++) ss[i - aes.IV.Length] = s[i];

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream msDecrypt = new MemoryStream(ss))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

        
        public void Set_HisPubklicKey(X509Certificate2 cert)
        {
            rsaHisPublic = cert.GetRSAPublicKey();
        }
        public void Set_HisPubklicKey(string cert)
        {
            rsaHisPublic = RSA.Create();
            rsaHisPublic.FromXmlString(cert);

        }

        private void Print(byte[] b)
        {
            foreach(byte bb in b)
            {
                Console.Write(@"|{0}",bb);
            }
            Console.WriteLine();
        }
    }
}
