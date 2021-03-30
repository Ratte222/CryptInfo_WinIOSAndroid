using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using CryptWinIOSAndroid;

namespace ConsoleCrypt
{
    class C_InputOutputFile:I_InputOutput
    {
        public AppSettings appSettings { get; set; }
        XmlSerializer formatter = new XmlSerializer(typeof(AppSettings));
        string _pathAppSetting = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "AppSettings.xml");
        private bool caseSensitive = false,
            searchInTegs = false,
            searchInHeader = false,
            searchUntilFirstMatch = false,
            viewServiceInformation = false,
            showAllFromCryptFile = false;
        public E_INPUTOUTPUTMESSAGE LoadDefaultParams()
        {
            caseSensitive = Get_caseSensitive();
            searchInTegs = Get_searchInTegs();
            searchInHeader = Get_searchInHeader();
            searchUntilFirstMatch = Get_searchUntilFirstMatch();
            viewServiceInformation = Get_viewServiceInformation();
            return E_INPUTOUTPUTMESSAGE.Ok;
        }

        public E_INPUTOUTPUTMESSAGE LoadSetting()
        {
            try
            {
                #region SerializableSetting


                using (FileStream fs = new FileStream(_pathAppSetting, FileMode.OpenOrCreate))
                {
                    appSettings = (AppSettings)formatter.Deserialize(fs);
                }
                LoadDefaultParams();
                #endregion
                return E_INPUTOUTPUTMESSAGE.Ok;
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
            return E_INPUTOUTPUTMESSAGE.ExceptionLoadSetting;
        }
        public E_INPUTOUTPUTMESSAGE SaveSetting()
        {
            try
            {
                if (appSettings == null)
                {
                    return E_INPUTOUTPUTMESSAGE.NoLinkToSettings;
                }
                else
                {
                    using (FileStream fs = new FileStream(_pathAppSetting, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, appSettings);
                    }
                    return E_INPUTOUTPUTMESSAGE.Ok;
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
            return E_INPUTOUTPUTMESSAGE.ExceptionSaveSetting;
        }
        public E_INPUTOUTPUTMESSAGE ResetSetting()
        {
            E_INPUTOUTPUTMESSAGE i_ = SaveSetting();
            if(i_ != E_INPUTOUTPUTMESSAGE.Ok)
            {
                return i_;
            }
            return LoadSetting();
        }
        public E_INPUTOUTPUTMESSAGE SetDirCryptFile(string val)
        {
            if(appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return i_;
            }
            appSettings.DirCryptFile = val;
            return E_INPUTOUTPUTMESSAGE.Ok;
        }
        public E_INPUTOUTPUTMESSAGE SetDirDecryptFile(string val)
        {
            if (appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return i_;
            }
            appSettings.DirDecryptFile = val;
            return E_INPUTOUTPUTMESSAGE.Ok;
        }

        public string GetDirCryptFile()
        {
            if (appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return null;
            }
            return appSettings.DirCryptFile;
        }
        public string GetDirDecryptFile()
        {
            if (appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return null;
            }
            return appSettings.DirDecryptFile;
        }
        public bool Get_caseSensitive()
        {
            if (appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return false;
            }
            return appSettings.SearchSettingDefault.caseSensitive;
        }
        public bool Get_searchInTegs()
        {
            if (appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return false;
            }
            return appSettings.SearchSettingDefault.searchInTegs;
        }
        public bool Get_searchInHeader()
        {
            if (appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return false;
            }
            return appSettings.SearchSettingDefault.searchInHeader;
        }
        public bool Get_searchUntilFirstMatch()
        {
            if (appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return false;
            }
            return appSettings.SearchSettingDefault.searchUntilFirstMatch;
        }
        public bool Get_viewServiceInformation()
        {
            if (appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return false;
            }
            return appSettings.SearchSettingDefault.viewServiceInformation;
        }
        public void Toggle_caseSensitive()
        {
            caseSensitive = !caseSensitive;
        }
        public void Toggle_searchInTegs()
        {
            searchInTegs = !searchInTegs;
        }
        public void Toggle_searchInHeader()
        {
            searchInHeader = !searchInHeader;
        }
        public void Toggle_searchUntilFirstMatch()
        {
            searchUntilFirstMatch = !searchUntilFirstMatch;
        }
        public void Toggle_viewServiceInformation()
        {
            viewServiceInformation = !viewServiceInformation;
        }
        public E_INPUTOUTPUTMESSAGE CryptFile(string key)
        {
            StreamReader srDecrypt = null;
            StreamWriter swCrypt = null;
            try
            {
                if (appSettings == null)
                {
                    E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                    if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                        return i_;
                }
                if (!File.Exists(appSettings.DirDecryptFile))
                    return E_INPUTOUTPUTMESSAGE.DecryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return E_INPUTOUTPUTMESSAGE.KeyIsNull;
                string content;
                srDecrypt = new StreamReader(appSettings.DirDecryptFile, Encoding.UTF8);
                swCrypt = new StreamWriter(appSettings.DirCryptFile, false, Encoding.UTF8);
                while (true)
                {
                    content = srDecrypt.ReadLine();
                    if (content != null)
                    {
                        swCrypt.WriteLine(CryptoWithoutTry.Encrypt(content, key, Encoding.UTF8));
                    }
                    else
                    {                                
                        break;
                    }
                }
                return E_INPUTOUTPUTMESSAGE.Ok;
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
            return E_INPUTOUTPUTMESSAGE.ExceprionCryptFile;
        }

        public E_INPUTOUTPUTMESSAGE DecryptFile(string key)
        {
            StreamReader srCrypt = null;
            StreamWriter swDecrypt = null;
            try
            {
                if (appSettings == null)
                {
                    E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                    if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                        return i_;
                }
                if (!File.Exists(appSettings.DirCryptFile))
                    return E_INPUTOUTPUTMESSAGE.CryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return E_INPUTOUTPUTMESSAGE.KeyIsNull;
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
                return E_INPUTOUTPUTMESSAGE.Ok;
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
            return E_INPUTOUTPUTMESSAGE.ExceprionDecryptFile;
        }

        public E_INPUTOUTPUTMESSAGE SearchBlockFromCryptRepositoriesUseKeyWord(string key, string keyWord)
        {
            StreamReader srCrypt = null;
            
            try
            {
                if (appSettings == null)
                {
                    E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                    if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                        return i_;
                }
                if (!File.Exists(appSettings.DirCryptFile))
                    return E_INPUTOUTPUTMESSAGE.CryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return E_INPUTOUTPUTMESSAGE.KeyIsNull;
                string content, block="";
                srCrypt = new StreamReader(appSettings.DirCryptFile, Encoding.UTF8);
                bool flagTargetBlock = false, stringIsHeader = false, header=false, tegs=false;
                int lineBlock = 0, intBlock = -1;
                if (!caseSensitive)
                    keyWord = keyWord.ToLower();
                while (true)
                {
                    content = srCrypt.ReadLine();
                    if (content != null)
                    {
                        content = (CryptoWithoutTry.Decrypt(content, key));
                        if(viewServiceInformation)
                            block += $"lb{lineBlock}# {content}\r\n";
                        else
                            block += $"{content}\r\n";
                        if ((lineBlock == 0) && content.IndexOf(appSettings.CharStartAttributes) > -1)
                        { 
                            tegs = true;
                            header = false;
                        }
                        else if ((lineBlock == 0) && content.IndexOf(appSettings.CharStartAttributes) == -1)
                        {
                            tegs = false;
                            header = true;
                        }
                        else if ((lineBlock == 1) && content.IndexOf(appSettings.CharStartAttributes) == -1)
                        {
                            tegs = false;
                            header = true;
                        }
                        else
                        {
                            tegs = false;
                            header = false;
                        }
                        lineBlock++;
                        if (!caseSensitive)
                            content = content.ToLower();
                        if(String.Equals(content, appSettings.SeparateBlock))
                        {
                            if(flagTargetBlock || showAllFromCryptFile)
                            {
                                block = block.Replace(appSettings.SeparateBlock, "");
                                block = block.TrimEnd(new char[] { '\r', '\n' });
                                ShowAPersone(block);
                                if (searchUntilFirstMatch && !showAllFromCryptFile)
                                    break;
                                else if (!searchUntilFirstMatch && !showAllFromCryptFile)
                                    flagTargetBlock = false;
                            }                            
                            intBlock++;
                            lineBlock = 0;
                            if (viewServiceInformation)
                                block = $"Block №{intBlock}\r\n";
                            else
                                block = "";
                        }
                        else 
                        {
                            if (!searchInHeader && !searchInTegs)
                                if (content.IndexOf(keyWord) > -1)
                                    flagTargetBlock = true;
                            else if(searchInHeader&&header)
                                    if (content.IndexOf(keyWord) > -1)
                                        flagTargetBlock = true;
                            else if (searchInTegs && tegs)
                                if (content.IndexOf(keyWord) > -1)
                                    flagTargetBlock = true;
                        }

                    }
                    else
                    {
                        if(searchUntilFirstMatch && !showAllFromCryptFile)
                            ShowAPersone("found nothing");
                        break;
                    }
                }                   
                
                return E_INPUTOUTPUTMESSAGE.Ok;
            }
            catch (Exception ex)
            {
                Program.HandleMessage("", ex);
            }
            finally
            {
                srCrypt?.Close();
            }
            return E_INPUTOUTPUTMESSAGE.SearchBlockFromCryptRepositoriesUseKeyWord;
        }

        public E_INPUTOUTPUTMESSAGE WriteToEndCryptFile(string key, string data)
        {
            StreamWriter sw = null;
            E_INPUTOUTPUTMESSAGE res = E_INPUTOUTPUTMESSAGE.Ok;
            try
            {
                if(String.IsNullOrWhiteSpace(data)||String.IsNullOrEmpty(data))
                {
                    return E_INPUTOUTPUTMESSAGE.False;
                }
                if (appSettings == null)
                {
                    E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                    if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                        return i_;
                }
                if (!File.Exists(appSettings.DirCryptFile))
                    return E_INPUTOUTPUTMESSAGE.CryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return E_INPUTOUTPUTMESSAGE.KeyIsNull;
                sw = new StreamWriter(appSettings.DirCryptFile, true, Encoding.UTF8);
                data = $"{data}{appSettings.SeparateBlock}";
                string[] splitData = data.Split("\r\n");
                foreach(string s in splitData)
                {
                    sw.WriteLine(CryptoWithoutTry.Encrypt(s, key, Encoding.UTF8));
                }                
                sw.Flush();
            }
            catch (Exception ex)
            {
                Program.HandleMessage("", ex);
                res = E_INPUTOUTPUTMESSAGE.WriteToEndCryptFile;
            }
            finally
            {
                sw?.Close();
                sw?.Dispose();
            }
            return res;
        }

        public E_INPUTOUTPUTMESSAGE ShowAllFromCryptFile(string key)
        {
            if (appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return i_;
            }
            showAllFromCryptFile = true;
            E_INPUTOUTPUTMESSAGE vs = SearchBlockFromCryptRepositoriesUseKeyWord(key, "");
            showAllFromCryptFile = false;
            return vs;
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
