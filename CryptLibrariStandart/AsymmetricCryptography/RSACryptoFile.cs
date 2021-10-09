using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using CryptLibrariStandart.Exceptions;

namespace CryptLibrariStandart.AsymmetricCryptography
{
    //using RSA method
    //https://docs.microsoft.com/ru-ru/dotnet/standard/security/walkthrough-creating-a-cryptographic-application
    public class RSACryptoFile
    {
        // Declare CspParmeters and RsaCryptoServiceProvider
        // objects with global scope of your Form class.
        CspParameters cspp = new CspParameters();
        RSACryptoServiceProvider rsa;//normal work only Windows!!!

        string _keyName = "Key01";

        public RSACryptoFile(string keyName = "Key01")
        {
            (_keyName) = (keyName);
        }

        public string CreateAsmKeys(string key)
        {
            if (!String.IsNullOrEmpty(key))
            {
                _keyName = key;
                return CreateAsmKeys();
            }
            else
            {
                return "";
            }
        }

        public string CreateAsmKeys()
        {
            if (String.IsNullOrEmpty(_keyName))
            {
                throw new CryptoArgumentNullException($"AsymmetricKey is null or empty");
            }
            cspp.KeyContainerName = _keyName;
            rsa = new RSACryptoServiceProvider(cspp);
            rsa.PersistKeyInCsp = true;
            if (rsa.PublicOnly == true)
                return "Key: " + cspp.KeyContainerName + " - Public Only";
            else
                return "Key: " + cspp.KeyContainerName + " - Full Key Pair";
        }

        public void ExportPublicKey(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new CryptoArgumentNullException("Path is null or empty");
            // Save the public key created by the RSA
            // to a file. Caution, persisting the
            // key to a file is a security risk.
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            StreamWriter sw = new StreamWriter(path, false);
            sw.Write(rsa.ToXmlString(false));
            sw.Close();
        }

        public string ImportPublicKey(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new CryptoArgumentNullException("Path is null or empty");
            if (!File.Exists(path))
                throw new CryptoException($"File {path} does not exist");
            StreamReader sr = new StreamReader(path);
            cspp.KeyContainerName = _keyName;
            rsa = new RSACryptoServiceProvider(cspp);
            string keytxt = sr.ReadToEnd();
            sr.Close();
            rsa.FromXmlString(keytxt);
            rsa.PersistKeyInCsp = true;
            if (rsa.PublicOnly == true)
                return "Key: " + cspp.KeyContainerName + " - Public Only";
            else
                return "Key: " + cspp.KeyContainerName + " - Full Key Pair";

        }



        public string EncryptFile(string pathFrom, string pathTo)
        {
            if (rsa == null)
            {
                return "Key not set.";
            }
            else
            {
                if (String.IsNullOrEmpty(pathFrom))
                    throw new CryptoArgumentNullException($"{nameof(pathFrom)} is null or empty");
                if (String.IsNullOrEmpty(pathTo))
                    throw new CryptoArgumentNullException($"{nameof(pathTo)} is null or empty");
                // Display a dialog box to select a file to encrypt.
                string dir = Path.GetDirectoryName(pathTo);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string fName = pathFrom;
                if (fName != null)
                {
                    FileInfo fInfo = new FileInfo(fName);
                    if (!fInfo.Exists)
                        throw new CryptoException($"File {fInfo.FullName} does not exist");
                    // Pass the file name without the path.
                    string name = fInfo.FullName;
                    _EncryptFile(name, pathTo);
                    return "File encrypt successfully";
                }
                return "File was not encrypt";
            }
        }
        private void _EncryptFile(string inFile, string pathTo)
        {

            // Create instance of Aes for
            // symmetric encryption of the data.
            Aes aes = Aes.Create();
            ICryptoTransform transform = aes.CreateEncryptor();

            // Use RSACryptoServiceProvider to
            // encrypt the AES key.
            // rsa is previously instantiated:
            //    rsa = new RSACryptoServiceProvider(cspp);
            byte[] keyEncrypted = rsa.Encrypt(aes.Key, false);

            // Create byte arrays to contain
            // the length values of the key and IV.
            byte[] LenK = new byte[4];
            byte[] LenIV = new byte[4];

            int lKey = keyEncrypted.Length;
            LenK = BitConverter.GetBytes(lKey);
            int lIV = aes.IV.Length;
            LenIV = BitConverter.GetBytes(lIV);

            // Write the following to the FileStream
            // for the encrypted file (outFs):
            // - length of the key
            // - length of the IV
            // - ecrypted key
            // - the IV
            // - the encrypted cipher content

            int startFileName = inFile.LastIndexOf("\\") + 1;
            // Change the file's extension to ".enc"
            string outFile = pathTo + ".enc";

            using (FileStream outFs = new FileStream(outFile, FileMode.Create))
            {

                outFs.Write(LenK, 0, 4);
                outFs.Write(LenIV, 0, 4);
                outFs.Write(keyEncrypted, 0, lKey);
                outFs.Write(aes.IV, 0, lIV);

                // Now write the cipher text using
                // a CryptoStream for encrypting.
                using (CryptoStream outStreamEncrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                {

                    // By encrypting a chunk at
                    // a time, you can save memory
                    // and accommodate large files.
                    int count = 0;
                    int offset = 0;

                    // blockSizeBytes can be any arbitrary size.
                    int blockSizeBytes = aes.BlockSize / 8;
                    byte[] data = new byte[blockSizeBytes];
                    int bytesRead = 0;

                    using (FileStream inFs = new FileStream(inFile, FileMode.Open))
                    {
                        do
                        {
                            count = inFs.Read(data, 0, blockSizeBytes);
                            offset += count;
                            outStreamEncrypted.Write(data, 0, count);
                            bytesRead += blockSizeBytes;
                        }
                        while (count > 0);
                        inFs.Close();
                    }
                    outStreamEncrypted.FlushFinalBlock();
                    outStreamEncrypted.Close();
                }
                outFs.Close();
            }
        }

        public string DecryptFile(string pathFrom, string pathTo)
        {
            if (rsa == null)
            {
                return "Key not set.";
            }
            else
            {
                if (String.IsNullOrEmpty(pathFrom))
                    throw new CryptoArgumentNullException($"{nameof(pathFrom)} is null or empty");
                if (String.IsNullOrEmpty(pathTo))
                    throw new CryptoArgumentNullException($"{nameof(pathTo)} is null or empty");
                if (pathFrom != null)
                {
                    FileInfo fi = new FileInfo(pathFrom);
                    if (!fi.Exists)
                        throw new CryptoException($"File {fi.FullName} does not exist");
                    string name = fi.FullName;
                    _DecryptFile(name, pathTo);
                    return "File decrypt successfully";
                }
                return "File was not decrypt";
            }
        }

        private void _DecryptFile(string inFile, string pathTo)
        {

            // Create instance of Aes for
            // symetric decryption of the data.
            Aes aes = Aes.Create();

            // Create byte arrays to get the length of
            // the encrypted key and IV.
            // These values were stored as 4 bytes each
            // at the beginning of the encrypted package.
            byte[] LenK = new byte[4];
            byte[] LenIV = new byte[4];

            // Construct the file name for the decrypted file.            
            //string fileName = Path.GetFileName(inFile);
            //int length = fileName.LastIndexOf(".");
            //string outFile = Path.Combine(pathTo, fileName.Substring(0, length));
            string outFile = pathTo;

            // Use FileStream objects to read the encrypted
            // file (inFs) and save the decrypted file (outFs).
            using (FileStream inFs = new FileStream(inFile, FileMode.Open))
            {

                inFs.Seek(0, SeekOrigin.Begin);
                inFs.Seek(0, SeekOrigin.Begin);
                inFs.Read(LenK, 0, 3);
                inFs.Seek(4, SeekOrigin.Begin);
                inFs.Read(LenIV, 0, 3);

                // Convert the lengths to integer values.
                int lenK = BitConverter.ToInt32(LenK, 0);
                int lenIV = BitConverter.ToInt32(LenIV, 0);

                // Determine the start postition of
                // the ciphter text (startC)
                // and its length(lenC).
                int startC = lenK + lenIV + 8;
                int lenC = (int)inFs.Length - startC;

                // Create the byte arrays for
                // the encrypted Aes key,
                // the IV, and the cipher text.
                byte[] KeyEncrypted = new byte[lenK];
                byte[] IV = new byte[lenIV];

                // Extract the key and IV
                // starting from index 8
                // after the length values.
                inFs.Seek(8, SeekOrigin.Begin);
                inFs.Read(KeyEncrypted, 0, lenK);
                inFs.Seek(8 + lenK, SeekOrigin.Begin);
                inFs.Read(IV, 0, lenIV);
                Directory.CreateDirectory(Path.GetDirectoryName(pathTo));
                // Use RSACryptoServiceProvider
                // to decrypt the AES key.
                byte[] KeyDecrypted = rsa.Decrypt(KeyEncrypted, false);

                // Decrypt the key.
                ICryptoTransform transform = aes.CreateDecryptor(KeyDecrypted, IV);

                // Decrypt the cipher text from
                // from the FileSteam of the encrypted
                // file (inFs) into the FileStream
                // for the decrypted file (outFs).
                using (FileStream outFs = new FileStream(outFile, FileMode.Create))
                {

                    int count = 0;
                    int offset = 0;

                    // blockSizeBytes can be any arbitrary size.
                    int blockSizeBytes = aes.BlockSize / 8;
                    byte[] data = new byte[blockSizeBytes];

                    // By decrypting a chunk a time,
                    // you can save memory and
                    // accommodate large files.

                    // Start at the beginning
                    // of the cipher text.
                    inFs.Seek(startC, SeekOrigin.Begin);
                    using (CryptoStream outStreamDecrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                    {
                        do
                        {
                            count = inFs.Read(data, 0, blockSizeBytes);
                            offset += count;
                            outStreamDecrypted.Write(data, 0, count);
                        }
                        while (count > 0);

                        outStreamDecrypted.FlushFinalBlock();
                        outStreamDecrypted.Close();
                    }
                    outFs.Close();
                }
                inFs.Close();
            }
        }
    }
}
