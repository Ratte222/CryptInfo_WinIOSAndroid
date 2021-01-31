using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ConsoleCrypt
{
    class C_InputOutputFile:I_InputOutput
    {
        public AppSettings appSettings { get; set; }
        XmlSerializer formatter = new XmlSerializer(typeof(AppSettings));
        string _pathAppSetting = Environment.CurrentDirectory + "\\AppSettings.xml";
        public I_INPUTOUTPUTMESSAGE LoadSetting()
        {
            try
            {
                #region SerializableSetting


                using (FileStream fs = new FileStream(_pathAppSetting, FileMode.OpenOrCreate))
                {
                    appSettings = (AppSettings)formatter.Deserialize(fs);
                }
                #endregion
                return I_INPUTOUTPUTMESSAGE.Ok;
            }
            catch (Exception ex)
            {
                string message = $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                            $"Message = {ex?.Message?.ToString()} \r\n" +
                            $"Source = {ex?.Source?.ToString()} \r\n" +
                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                            $"TargetSite = {ex?.TargetSite?.ToString()}";
#if DEBUG
                ShowAPersone(message);
#endif
            }
            return I_INPUTOUTPUTMESSAGE.ExceptionLoadSetting;
        }
        public I_INPUTOUTPUTMESSAGE SaveSetting()
        {
            try
            {
                if (appSettings == null)
                {
                    return I_INPUTOUTPUTMESSAGE.NoLinkToSettings;
                }
                else
                {
                    using (FileStream fs = new FileStream(_pathAppSetting, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, appSettings);
                    }
                    return I_INPUTOUTPUTMESSAGE.Ok;
                }
            }
            catch (Exception ex)
            {
                string message = $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                            $"Message = {ex?.Message?.ToString()} \r\n" +
                            $"Source = {ex?.Source?.ToString()} \r\n" +
                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                            $"TargetSite = {ex?.TargetSite?.ToString()}";
#if DEBUG
                ShowAPersone(message);
#endif
            }
            return I_INPUTOUTPUTMESSAGE.ExceptionSaveSetting;
        }
        public I_INPUTOUTPUTMESSAGE ResetSetting()
        {
            I_INPUTOUTPUTMESSAGE i_ = SaveSetting();
            if(i_ != I_INPUTOUTPUTMESSAGE.Ok)
            {
                return i_;
            }
            return LoadSetting();
        }
        public I_INPUTOUTPUTMESSAGE SetDirCryptFile(string val)
        {
            if(appSettings == null)
            {
                I_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != I_INPUTOUTPUTMESSAGE.Ok)
                    return i_;
            }
            appSettings.DirCryptFile = val;
            return I_INPUTOUTPUTMESSAGE.Ok;
        }
        public I_INPUTOUTPUTMESSAGE SetDirDecryptFile(string val)
        {
            if (appSettings == null)
            {
                I_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != I_INPUTOUTPUTMESSAGE.Ok)
                    return i_;
            }
            appSettings.DirDecryptFile = val;
            return I_INPUTOUTPUTMESSAGE.Ok;
        }

        public string GetDirCryptFile()
        {
            if (appSettings == null)
            {
                I_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != I_INPUTOUTPUTMESSAGE.Ok)
                    return null;
            }
            return appSettings.DirCryptFile;
        }
        public string GetDirDecryptFile()
        {
            if (appSettings == null)
            {
                I_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != I_INPUTOUTPUTMESSAGE.Ok)
                    return null;
            }
            return appSettings.DirDecryptFile;
        }
        public I_INPUTOUTPUTMESSAGE CryptFile(string key)
        {
            StreamReader srDecrypt = null;
            StreamWriter swCrypt = null;
            try
            {
                if (appSettings == null)
                {
                    I_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                    if (i_ != I_INPUTOUTPUTMESSAGE.Ok)
                        return i_;
                }
                if (!File.Exists(appSettings.DirDecryptFile))
                    return I_INPUTOUTPUTMESSAGE.DecryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return I_INPUTOUTPUTMESSAGE.KeyIsNull;
                string content;
                srDecrypt = new StreamReader(appSettings.DirDecryptFile, Encoding.UTF8);
                swCrypt = new StreamWriter(appSettings.DirCryptFile, false, Encoding.UTF8);
                while (true)
                {
                    content = srDecrypt.ReadLine();
                    if (content != null)
                    {
                        swCrypt.WriteLine(CryptoWithoutTry.Encrypt(content, key));
                    }
                    else
                    {                                
                        break;
                    }
                }
                return I_INPUTOUTPUTMESSAGE.Ok;
            }
            catch(Exception ex)
            {
                string message = $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                            $"Message = {ex?.Message?.ToString()} \r\n" +
                            $"Source = {ex?.Source?.ToString()} \r\n" +
                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                            $"TargetSite = {ex?.TargetSite?.ToString()}";
#if DEBUG
                ShowAPersone(message);
#endif
            }
            finally
            {
                srDecrypt?.Close();
                swCrypt?.Flush();
                swCrypt?.Close();
            }
            return I_INPUTOUTPUTMESSAGE.ExceprionCryptFile;
        }

        public I_INPUTOUTPUTMESSAGE DecryptFile(string key)
        {
            StreamReader srCrypt = null;
            StreamWriter swDecrypt = null;
            try
            {
                if (appSettings == null)
                {
                    I_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                    if (i_ != I_INPUTOUTPUTMESSAGE.Ok)
                        return i_;
                }
                if (!File.Exists(appSettings.DirCryptFile))
                    return I_INPUTOUTPUTMESSAGE.CryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return I_INPUTOUTPUTMESSAGE.KeyIsNull;
                string content;

                srCrypt = new StreamReader(appSettings.DirCryptFile, Encoding.UTF8);
                swDecrypt = new StreamWriter(appSettings.DirDecryptFile, false, Encoding.UTF8);
                while (true)
                {
                    content = srCrypt.ReadLine();
                    if (content != null)
                    {
                        swDecrypt.WriteLine(CryptoWithoutTry.Decrypt(content, key));
                    }
                    else
                    {                        
                        break;
                    }
                }                 
                return I_INPUTOUTPUTMESSAGE.Ok;
            }
            catch (Exception ex)
            {
                string message = $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                            $"Message = {ex?.Message?.ToString()} \r\n" +
                            $"Source = {ex?.Source?.ToString()} \r\n" +
                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                            $"TargetSite = {ex?.TargetSite?.ToString()}";
#if DEBUG
                ShowAPersone(message);
#endif
            }
            finally
            {
                srCrypt?.Close();
                swDecrypt?.Flush();
                swDecrypt?.Close();
            }
            return I_INPUTOUTPUTMESSAGE.ExceprionDecryptFile;
        }

        public I_INPUTOUTPUTMESSAGE SearchBlockFromCryptRepositoriesUseKeyWord(string key, string keyWord, bool caseSensitive)
        {
            StreamReader srCrypt = null;
            try
            {
                if (appSettings == null)
                {
                    I_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                    if (i_ != I_INPUTOUTPUTMESSAGE.Ok)
                        return i_;
                }
                if (!File.Exists(appSettings.DirCryptFile))
                    return I_INPUTOUTPUTMESSAGE.CryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return I_INPUTOUTPUTMESSAGE.KeyIsNull;
                string content, block="";
                srCrypt = new StreamReader(appSettings.DirCryptFile, Encoding.UTF8);
                bool flagTargetBlock = false;
                if (!caseSensitive)
                    keyWord = keyWord.ToLower();
                while (true)
                {
                    content = srCrypt.ReadLine();
                    if (content != null)
                    {
                        content = (CryptoWithoutTry.Decrypt(content, key));
                        block += $"{content}\r\n";
                        if (!caseSensitive)
                            content = content.ToLower();
                        if(String.Equals(content, appSettings.SeparateBlock))
                        {
                            if(flagTargetBlock)
                            {
                                block = block.Replace(appSettings.SeparateBlock, "");
                                ShowAPersone(block);
                                
                                break;
                            }
                            block = "";
                        }
                        else if(content.IndexOf(keyWord) > -1)
                        {
                            flagTargetBlock = true;
                        }
                    }
                    else
                    {
                        ShowAPersone("found nothing");
                        break;
                    }
                }                   
                
                return I_INPUTOUTPUTMESSAGE.Ok;
            }
            catch (Exception ex)
            {
                string message = $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                            $"Message = {ex?.Message?.ToString()} \r\n" +
                            $"Source = {ex?.Source?.ToString()} \r\n" +
                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                            $"TargetSite = {ex?.TargetSite?.ToString()}";
#if DEBUG
                ShowAPersone(message);
#endif
            }
            finally
            {
                srCrypt?.Close();
            }
            return I_INPUTOUTPUTMESSAGE.SearchBlockFromCryptRepositoriesUseKeyWord;
        }        

        public void ShowAPersone(string message)
        {
            Console.WriteLine(message);
        }

        public string ReadFromPersone()
        {
            return Console.ReadLine();
        }
    }
}
