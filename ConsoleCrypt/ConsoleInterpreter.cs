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
                if(!String.IsNullOrWhiteSpace(command)&&!String.IsNullOrEmpty(command))
                {
                    if(String.Equals(command.ToLower(), "exit"))
                    {
                        HandleCallIntergaceMethods(InputOutputFile.SaveSetting());
                        break;
                    }
                    else if (command.IndexOf("set") > -1)
                    {
                        string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
                    else if(command.IndexOf("decrypt")> -1)
                    {
                        string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
                    else if (command.IndexOf("crypt") > -1)
                    {
                        string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
                    else if (command.IndexOf("search") > -1)
                    {
                        string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
