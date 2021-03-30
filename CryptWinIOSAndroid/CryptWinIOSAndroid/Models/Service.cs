using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CryptWinIOSAndroid.Models
{
    class Service
    {
        public _Problem problem = new _Problem();
        public string[] CryptoKey { get; private set; }
        public string[] CryptoKeyImp { get; private set; }
        public int[] BlockStartIndex { get; private set; }
        public string CSServiceInformation { get; private set; }
        public string AssemblyServiceInformation_ { get; private set; }
        static public List<string> fileContentCryptSplit;
        string MainImportantKey;
        string MainKey;
        public List<BlockOfInformation> blockOfInformations = new List<BlockOfInformation>();
        public Service(string[] assemblyServiceInformation)
        {
            
            DisassemblyServiceInformation(assemblyServiceInformation);
        }

        public Service(string key, string impkey) 
        {
            MainKey = key;
            MainImportantKey = impkey;
        }

        public Service() { }

        public void SetKeys(string key, string impkey)
        {
            MainKey = key;
            MainImportantKey = impkey;
            DecryptFile();
        }

        public string DecryptFile(string path = null, bool defaultPath = true)
        {
            if(defaultPath)
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "New.txt");
            }
            if (!File.Exists(path))
            {
                CreateEmptyFile();
            }
            return DecryptFileContent(File.ReadAllText(path));
            //using (StreamReader reader = new StreamReader(fileName))
            //{
            //    fileContentCrypt = reader.ReadToEnd();
            //}

        }

        public string DecryptFileContent(string fileContentCrypt)
        {
            //fileContentCryptSplit = fileContentCrypt.Split(new char[] { '\r', '\n' }).ToList();
            var temp = fileContentCrypt.Split(new char[] { '\r', '\n' });
            fileContentCryptSplit = temp.ToList();
            DisassemblyAndDecryptServiceInformation();
            string result = "";
            for (int i = 0; i < BlockStartIndex.Length; i++)
            {
                blockOfInformations.Add(new BlockOfInformation(0, BlockStartIndex[i],
                    CryptoKey[i], CryptoKeyImp[i]));
                result += blockOfInformations[i].Decrypt();
                //result += DecryptBlock(i);
            }
            return result;
        }

        public void Insert(string[] val)
        {
            string[] tempArrStr = new string[CryptoKey.Length + 1];
            Array.Copy(CryptoKey, 0, tempArrStr, 0, CryptoKey.Length);
            //CryptoKey = new string[tempArrStr.Length];
            tempArrStr[tempArrStr.Length - 1] = CreateKey();
            CryptoKey = tempArrStr;
            int[] tempArrInt = new int[BlockStartIndex.Length + 1];
            Array.Copy(BlockStartIndex, 0, tempArrInt, 0, BlockStartIndex.Length);

        }

        private void AssemblyServiceInformation()
        {
            //AssemblyServiceInformation_ = null;
            //string[] temp = new string[3];
            fileContentCryptSplit[0] = "";
            foreach (string s in CryptoKey)
            {
                fileContentCryptSplit[0] += $"{s}!";
            }
            fileContentCryptSplit[0] = fileContentCryptSplit[0].TrimEnd('!');
            fileContentCryptSplit[1] = "";
            foreach (string s in CryptoKeyImp)
            {
                fileContentCryptSplit[1] += $"{s}!";
            }
            fileContentCryptSplit[1] = fileContentCryptSplit[1].TrimEnd('!');
            fileContentCryptSplit[2] = "";
            foreach (int i in BlockStartIndex)
            {
                fileContentCryptSplit[2] += $"{i.ToString()}!";
            }
            fileContentCryptSplit[2] = fileContentCryptSplit[2].TrimEnd('!');
            fileContentCryptSplit[3] = CryptoWithoutTry.CalculateCSString(fileContentCryptSplit[0] +
                fileContentCryptSplit[1] + fileContentCryptSplit[2]);
            //problem = Crypto_.CalculateCSString(temp[0] + temp[1] + temp[2]);
            //if (problem.HasProblem) { return; }
            //AssemblyServiceInformation_ = $"{temp[0]}\r\n{temp[1]}\r\n{temp[2]}\r\n{problem.str}\r\n";
        }

        private void AssemblyAndEncryptServiceInformation()
        {
            //AssemblyServiceInformation_ = null;
            AssemblyServiceInformation();
            string key = MainKey.Insert(MainKey.Length / 2, MainImportantKey);
            for (int i = 0; i < 4; i++)
            {
                fileContentCryptSplit[i] = CryptoWithoutTry.Encrypt(fileContentCryptSplit[i], key, Encoding.UTF8);
            }
        }

        public void DisassemblyAndDecryptServiceInformation()
        {
            string key = MainKey.Insert(MainKey.Length / 2, MainImportantKey);
            for (int i = 0; i < 4; i++)
            {
                fileContentCryptSplit[i] = CryptoWithoutTry.Decrypt(fileContentCryptSplit[i], key);
            }
            AssemblyServiceInformation_ = $"{fileContentCryptSplit[0]}" +
                $"{fileContentCryptSplit[1]}{fileContentCryptSplit[2]}";
            problem = Crypto_.CalculateCSString(AssemblyServiceInformation_);
            if (problem.HasProblem) { return; }
            CSServiceInformation = problem.str;
            if (CSServiceInformation != fileContentCryptSplit[3])
            {
                problem = new _Problem(true, "CS does not match");
                return;
            }
            string[] tempSplit = fileContentCryptSplit[0].Split('!');
            CryptoKey = new string[tempSplit.Length];
            Array.Copy(tempSplit, 0, CryptoKey, 0, tempSplit.Length);
            tempSplit = fileContentCryptSplit[1].Split('!');
            CryptoKeyImp = new string[tempSplit.Length];
            Array.Copy(tempSplit, 0, CryptoKeyImp, 0, tempSplit.Length);
            tempSplit = fileContentCryptSplit[2].Split('!');
            BlockStartIndex = new int[tempSplit.Length];
            for (int i = 0; i < tempSplit.Length; i++)
            {
                BlockStartIndex[i] = Convert.ToInt32(tempSplit[i]);
            }
        }

        public void DisassemblyServiceInformation(string[] assemblyServiceInformation)
        {
            AssemblyServiceInformation_ = $"{assemblyServiceInformation[0]}" +
                $"{assemblyServiceInformation[1]}";
            problem = Crypto_.CalculateCSString(AssemblyServiceInformation_);
            if (problem.HasProblem) { return; }
            CSServiceInformation = problem.str;
            if (CSServiceInformation != assemblyServiceInformation[3])
            {
                problem = new _Problem(true, "CS does not match");
                return;
            }
            string[] tempSplit = assemblyServiceInformation[0].Split('!');
            CryptoKey = new string[tempSplit.Length];
            Array.Copy(tempSplit, 0, CryptoKey, 0, tempSplit.Length);
            tempSplit = assemblyServiceInformation[1].Split('!');
            BlockStartIndex = new int[tempSplit.Length];
            for (int i = 0; i < tempSplit.Length; i++)
            {
                BlockStartIndex[i] = Convert.ToInt32(tempSplit[i]);
            }
        }

        private string CreateKey(bool important = true)
        {
            Random random = new Random();
            string result = "";
            for (int i = 0; i < 6; i++)
            {
                result += Encoding.UTF8.GetString(new byte[] { Convert.ToByte(random.Next(48, 122)) });
            }
            return result;
        }

        public string GetEnctyptFile()
        {
            AssemblyAndEncryptServiceInformation();
            return null;
        }

        public void CreateEmptyFile()
        {
            string FileNamePassword = Environment.CurrentDirectory + "\\New.txt";
            string fileContent = null;
            CryptoKey = new string[] { CreateKey(), CreateKey() };
            CryptoKeyImp = new string[] { "", CreateKey() };
            BlockStartIndex = new int[] { 4, 6 };
            fileContentCryptSplit = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                fileContentCryptSplit.Add(i.ToString());
            }
            AssemblyAndEncryptServiceInformation();
            foreach (string s in fileContentCryptSplit)
            {
                fileContent += $"{s}\r\n";
            }
            //fileContent = AssemblyServiceInformation_;
            blockOfInformations.Add(new BlockOfInformation(new string[] { "", "CryptInf" }, 0,
                CryptoKey[0], CryptoKeyImp[0], MainKey, MainImportantKey));
            blockOfInformations.Add(new BlockOfInformation(new string[] { "", "Crypt important information" }, 1,
                CryptoKey[1], CryptoKeyImp[1], MainKey, MainImportantKey));
            //fileContent += EncryptBlock(new string[] { "", "CryptInf" }, 0);
            //fileContent += EncryptBlock(new string[] { "", "Crypt important information" }, 1);
            fileContent += blockOfInformations[0].EncryptBlock;
            fileContent += blockOfInformations[1].EncryptBlock;
            File.WriteAllText(FileNamePassword, fileContent);

            //using (FileStream fs = new FileStream(Form1.cPSetting.FileNamePassword, FileMode.CreateNew))
            //{                
            //    fs.Write()
            //}
        }
    }
}
