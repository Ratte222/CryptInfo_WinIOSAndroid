using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using WorkWithFileLibrary;
using CryptLibrary;

namespace CommonForCryptPasswordLibrary
{
    
    public class InputOutputFile:I_InputOutput
    {
        IMyIO console_IO;
        Encoding _encoding = Encoding.UTF8;
        ISettings settings;
        public E_INPUTOUTPUTMESSAGE lastProblem { get; protected set; } = E_INPUTOUTPUTMESSAGE.Ok;
        
        
        private bool caseSensitive = false,
            searchInTegs = false,
            searchInHeader = false,
            searchUntilFirstMatch = false,
            viewServiceInformation = false,
            showAllFromCryptFile = false;

        public InputOutputFile(IMyIO _console_IO, ISettings settings)
        {
            console_IO = _console_IO;
            this.settings = settings;
            LoadDefaultParams();
        }

        public E_INPUTOUTPUTMESSAGE LoadDefaultParams()
        {
            caseSensitive = settings.Get_caseSensitive();
            searchInTegs = settings.Get_searchInTegs();
            searchInHeader = settings.Get_searchInHeader();
            searchUntilFirstMatch = settings.Get_searchUntilFirstMatch();
            viewServiceInformation = settings.Get_viewServiceInformation();
            return E_INPUTOUTPUTMESSAGE.Ok;
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
                if (!File.Exists(settings.GetDirDecryptFile()))
                    return E_INPUTOUTPUTMESSAGE.DecryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return E_INPUTOUTPUTMESSAGE.KeyIsNull;
                string content;
                srDecrypt = new StreamReader(settings.GetDirDecryptFile(), _encoding);
                swCrypt = new StreamWriter(settings.GetDirCryptFile(), false, _encoding);
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
                console_IO.WriteLine(message);
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
                if (!File.Exists(settings.GetDirCryptFile()))
                    return E_INPUTOUTPUTMESSAGE.CryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return E_INPUTOUTPUTMESSAGE.KeyIsNull;
                string content;

                srCrypt = new StreamReader(settings.GetDirCryptFile(), _encoding);
                swDecrypt = new StreamWriter(settings.GetDirDecryptFile(), false, _encoding);
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
                swDecrypt.Flush();
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
                console_IO.WriteLine(message);
#endif
            }
            finally
            {
                srCrypt?.Dispose();                
                swDecrypt?.Dispose();                
            }
            return E_INPUTOUTPUTMESSAGE.ExceprionDecryptFile;
        }

        public E_INPUTOUTPUTMESSAGE SearchBlockFromCryptRepositoriesUseKeyWord(string key, string keyWord)
        {
            StreamReader srCrypt = null;
            
            try
            {                
                if (!File.Exists(settings.GetDirCryptFile()))
                    return E_INPUTOUTPUTMESSAGE.CryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return E_INPUTOUTPUTMESSAGE.KeyIsNull;
                string content, block="";
                srCrypt = new StreamReader(settings.GetDirCryptFile(), _encoding);
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
                        if ((lineBlock == 0) && content.IndexOf(settings.Get_charStartAttributes()) > -1)
                        { 
                            tegs = true;
                            header = false;
                        }
                        else if ((lineBlock == 0) && content.IndexOf(settings.Get_charStartAttributes()) == -1)
                        {
                            tegs = false;
                            header = true;
                        }
                        else if ((lineBlock == 1) && content.IndexOf(settings.Get_charStartAttributes()) == -1)
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
                        if(String.Equals(content, settings.Get_separateBlock()))
                        {
                            if(flagTargetBlock || showAllFromCryptFile)
                            {
                                block = block.Replace(settings.Get_separateBlock(), "");
                                block = block.TrimEnd(new char[] { '\r', '\n' });
                                console_IO.WriteLine(block);
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
                            console_IO.WriteLine("found nothing");
                        break;
                    }
                }                   
                
                return E_INPUTOUTPUTMESSAGE.Ok;
            }
            catch (Exception ex)
            {
                console_IO.HandleMessage("", ex);
            }
            finally
            {
                srCrypt?.Dispose();
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
                if (!File.Exists(settings.GetDirCryptFile()))
                    return E_INPUTOUTPUTMESSAGE.CryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return E_INPUTOUTPUTMESSAGE.KeyIsNull;
                sw = new StreamWriter(settings.GetDirCryptFile(), true, _encoding);
                data = $"{data}{settings.Get_separateBlock()}";
                string[] splitData = data.Split("\r\n");
                foreach(string s in splitData)
                {
                    sw.WriteLine(CryptoWithoutTry.Encrypt(s, key));
                }                
                sw.Flush();
            }
            catch (Exception ex)
            {
                console_IO.HandleMessage("", ex);
                res = E_INPUTOUTPUTMESSAGE.WriteToEndCryptFile;
            }
            finally
            {               
                sw?.Dispose();
            }
            return res;
        }

        public E_INPUTOUTPUTMESSAGE ShowAllFromCryptFile(string key)
        {            
            showAllFromCryptFile = true;
            E_INPUTOUTPUTMESSAGE vs = SearchBlockFromCryptRepositoriesUseKeyWord(key, "");
            showAllFromCryptFile = false;
            return vs;
        }
        enum EBLOCKDATA
        {
            stringBlockStartInFile = 0,
            blockLength
        }
        public string GetBlockData(out int[] blockData, string key, int targetBlock, int targetLine = -1)
        {
            blockData = new int[] { -1, -1 };
            StreamReader srCrypt = null;
            string result = "";
            if (viewServiceInformation && (targetLine >= 0))
                targetLine++;
            try
            {               
                if (!File.Exists(settings.GetDirCryptFile()))
                {
                    lastProblem = E_INPUTOUTPUTMESSAGE.CryptFileNotExist;
                    return result;
                }
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                {
                    lastProblem = E_INPUTOUTPUTMESSAGE.KeyIsNull;
                    return result;
                }
                string content, block_str = "";
                srCrypt = new StreamReader(settings.GetDirCryptFile(), _encoding);
                int lineBlock = 0, intBlock = -1, currentLineInFile = 0; ;
                while (true)
                {
                    content = srCrypt.ReadLine();
                    if (content != null)
                    {
                        content = (CryptoWithoutTry.Decrypt(content, key));
                        if (viewServiceInformation)
                            block_str += $"lb{lineBlock}# {content}\r\n";
                        else
                            block_str += $"{content}\r\n";
                        lineBlock++; currentLineInFile++;
                        if (String.Equals(content, settings.Get_separateBlock()))
                        {                                                         
                            if (targetBlock == intBlock)
                            {
                                if(targetLine < 0)
                                {
                                    block_str = block_str.Replace(settings.Get_separateBlock(), "");
                                    block_str = block_str.TrimEnd(new char[] { '\r', '\n' });
                                    result = block_str;
                                    blockData[(int)EBLOCKDATA.blockLength] = lineBlock - 1;
                                    blockData[(int)EBLOCKDATA.stringBlockStartInFile] = currentLineInFile - lineBlock - 2;
                                }
                                else
                                {
                                    if (lineBlock-1 >= targetLine)
                                    {
                                        result = block_str.Split('\n')[targetLine].TrimEnd(new char[] { '\r', '\n' });
                                        if(viewServiceInformation)
                                            blockData[(int)EBLOCKDATA.stringBlockStartInFile] = currentLineInFile+(targetLine - lineBlock);
                                        else
                                            blockData[(int)EBLOCKDATA.stringBlockStartInFile] = currentLineInFile+(targetLine - lineBlock)+1;
                                    }                                        
                                    else
                                        result = "target line more then line in block";
                                }                                
                                break;
                            }
                            intBlock++;
                            lineBlock = 0;
                            if (viewServiceInformation)
                                block_str = $"Block №{intBlock}\r\n";
                            else
                                block_str = "";
                        }                        
                    }
                    else
                    {
                        if (intBlock < targetBlock)
                            result = "target block more then block in file";
                        break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                console_IO.HandleMessage("", ex);
            }
            finally
            {
                srCrypt?.Dispose();
            }
            return result;
        }
        public E_INPUTOUTPUTMESSAGE Update(string key, string data, int[] blockData)
        {            
            try
            {                
                if (!File.Exists(settings.GetDirCryptFile()))
                    return E_INPUTOUTPUTMESSAGE.CryptFileNotExist;
                if (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key))
                    return E_INPUTOUTPUTMESSAGE.KeyIsNull;
                if(blockData[(int)EBLOCKDATA.blockLength] == -1)//update one line
                {
                    if (CWorkWithFileLibrary.rewriteLineText(blockData[(int)EBLOCKDATA.stringBlockStartInFile], 
                        settings.GetDirCryptFile(), CryptoWithoutTry.Encrypt(data, key), _encoding))
                    {
                        return E_INPUTOUTPUTMESSAGE.Ok;
                    }
                    else return E_INPUTOUTPUTMESSAGE.Update;
                }
                else//update block
                {
                    string[] vs = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    string cryptContent = "";
                    foreach(string s in vs)
                    {
                        cryptContent += CryptoWithoutTry.Encrypt(s.Trim(), key) + "\r\n";
                    }    
                    if (CWorkWithFileLibrary.rewriteMultiLineText(blockData[(int)EBLOCKDATA.stringBlockStartInFile] + 3,
                        blockData[(int)EBLOCKDATA.blockLength], settings.GetDirCryptFile(),
                        cryptContent, _encoding))
                    {
                        return E_INPUTOUTPUTMESSAGE.Ok;
                    }
                    else return E_INPUTOUTPUTMESSAGE.Update;
                }
            }
            catch (Exception ex)
            {
                console_IO.HandleMessage("", ex);
            }            
            return E_INPUTOUTPUTMESSAGE.Update;
        }

        
       
    }
}
