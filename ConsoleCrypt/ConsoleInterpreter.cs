using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt
{
    class ConsoleInterpreter
    {
        C_InputOutputFile InputOutputFile;
        public ConsoleInterpreter(C_InputOutputFile _InputOutputFile)
        {
            InputOutputFile = _InputOutputFile;
        }
        public void Start()
        {
            InputOutputFile.ShowAPersone("Enter command");
            while (true)
            {
                string command = InputOutputFile.ReadFromPersone();
                string[] splitCommand = command.Split(' ');
                splitCommand[0] = splitCommand[0].ToLower();
                if(!String.IsNullOrWhiteSpace(command)&&!String.IsNullOrEmpty(command))
                {
                    if(String.Equals(command.ToLower(), "exit"))
                    {
                        HandleCallIntergaceMethods(InputOutputFile.SaveSetting());
                        break;
                    }
                    else if (String.Equals(splitCommand[0], "help"))
                    {
                        InputOutputFile.ShowAPersone("set -c pathCryptFile");
                        InputOutputFile.ShowAPersone("set -d pathDecryptFile");
                        InputOutputFile.ShowAPersone("decrypt -f password");
                        InputOutputFile.ShowAPersone("crypt -f password");
                        InputOutputFile.ShowAPersone("search -cs password searchWord");
                        InputOutputFile.ShowAPersone("-cs  (optional) case sensetive");
                        InputOutputFile.ShowAPersone("viewSetting");
                    }

                    else if (String.Equals(splitCommand[0], "set"))
                    {
                        //string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (splitCommand.Length < 3)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");
                            //InputOutputFile.ShowAPersone("expected \"search keyCrypt keyWord\"");
                        }
                        if (String.Equals(splitCommand[1], "-c"))
                        {
                            HandleCallIntergaceMethods(InputOutputFile.SetDirCryptFile(splitCommand[splitCommand.Length - 1]));
                            HandleCallIntergaceMethods(InputOutputFile.ResetSetting());
                        }
                        else if (String.Equals(splitCommand[1], "-d"))
                        {
                            HandleCallIntergaceMethods(InputOutputFile.SetDirDecryptFile(splitCommand[splitCommand.Length - 1]));
                            HandleCallIntergaceMethods(InputOutputFile.ResetSetting());
                        }
                        else
                        {
                            InputOutputFile.ShowAPersone("set. Unknown parameter. Use help");
                        }
                    }
                    else if(String.Equals(splitCommand[0], "decrypt"))
                    {
                        //string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (splitCommand.Length < 3)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");
                        }
                        else
                        {
                            if (String.Equals(splitCommand[1], "-f"))
                            {
                                HandleCallIntergaceMethods(InputOutputFile.DecryptFile(splitCommand[splitCommand.Length - 1]));
                            }
                            else
                            {
                                InputOutputFile.ShowAPersone("decrypt. Unknown command. Use help");
                            }
                        }
                        
                    }
                    else if (String.Equals(splitCommand[0], "crypt"))
                    {
                        //string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if(splitCommand.Length < 3)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");
                        }
                        else
                        {
                            if (String.Equals(splitCommand[1], "-f"))
                            {
                                HandleCallIntergaceMethods(InputOutputFile.CryptFile(splitCommand[splitCommand.Length - 1]));
                            }
                            else
                            {
                                InputOutputFile.ShowAPersone("crypt. Unknown command. Use help");
                            }
                        }
                    }
                    else if (String.Equals(splitCommand[0], "search"))
                    {
                        //string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if(splitCommand.Length < 3)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");
                            InputOutputFile.ShowAPersone("expected \"search keyCrypt keyWord\"");
                        }
                        bool caseSensitive = false;
                        if (command.IndexOf("-cs")> -1)
                        {
                            caseSensitive = true;
                            
                        }
                        HandleCallIntergaceMethods(InputOutputFile.SearchBlockFromCryptRepositoriesUseKeyWord(splitCommand[splitCommand.Length - 2],
                            splitCommand[splitCommand.Length - 1], caseSensitive));
                        
                    }
                    else if(String.Equals(splitCommand[0], "viewsetting"))
                    {
                        InputOutputFile.ShowAPersone($"DirCryptFile {InputOutputFile.GetDirCryptFile()}");
                        InputOutputFile.ShowAPersone($"DirDecryptFile {InputOutputFile.GetDirDecryptFile()}");
                    }
                    else
                    {
                        InputOutputFile.ShowAPersone("Unknown command. Use help");
                    }
                }
                else
                {
                    InputOutputFile.ShowAPersone("Command isNull Or WriteSpace Or Empty");
                }
            }
            
        }

        protected bool HandleCallIntergaceMethods(I_INPUTOUTPUTMESSAGE i_)
        {
            if(i_ != I_INPUTOUTPUTMESSAGE.Ok)
            {
                InputOutputFile.ShowAPersone($"errore {i_.ToString()}");
                return false;
            }
            InputOutputFile.ShowAPersone($"Ok");
            return true;
        }
    }
}
