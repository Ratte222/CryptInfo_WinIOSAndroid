using System;
using System.Collections.Generic;
using System.Text;
using CryptWinIOSAndroid;

namespace ConsoleCrypt
{
    class ConsoleInterpreter
    {
        private string password;
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
                        if(splitCommand.Length > 1)
                        {
                            splitCommand[1] = splitCommand[1].ToLower();
                            if (String.Equals(splitCommand[1], "set"))
                            {
                                InputOutputFile.ShowAPersone("-c - set in setting path crypt file. Example: set -c E:\\Crypr.txt");
                                InputOutputFile.ShowAPersone("-d - set in setting path decrypt file. Example: set -d E:\\Decrypr.txt");
                            }
                            else if (String.Equals(splitCommand[1], "decrypt"))
                            {
                                InputOutputFile.ShowAPersone("-f - decrypt file. Example: decrypt -f");
                            }
                            else if (String.Equals(splitCommand[1], "crypt"))
                            {
                                InputOutputFile.ShowAPersone("-f - crypt file. Example: crypt -f ");
                            }
                            else if (String.Equals(splitCommand[1], "search"))
                            {
                                InputOutputFile.ShowAPersone("search -cs -sh -st -sufm -vsi searchWord");
                                InputOutputFile.ShowAPersone("-cs  (optional, toggle default param) case sensetive");
                                InputOutputFile.ShowAPersone("-sh  (optional, toggle default param) search in header");
                                InputOutputFile.ShowAPersone("-st  (optional, , toggle default param) search in tegs");
                                InputOutputFile.ShowAPersone("-sufm  (optional, toggle default param) search until first math");
                                InputOutputFile.ShowAPersone("-vsi  (optional, toggle default param) view setting information");
                            }
                            else if (String.Equals(splitCommand[1], "reenter"))
                            {
                                InputOutputFile.ShowAPersone("-p - re-enter password. Example: reenter -p");
                            }
                            else if (String.Equals(splitCommand[1], "show"))
                            {
                                InputOutputFile.ShowAPersone("-b - show block in crypt file. Example: show -b 5 -vsi ");
                                InputOutputFile.ShowAPersone("-ln - show line in block in crypt file. Example: show -b 5 -ln 4 ");
                                InputOutputFile.ShowAPersone("-all - show all data in crypt file. Example: show -all -vsi");
                                InputOutputFile.ShowAPersone("-vsi  (optional, toggle default param) view setting information");
                            }
                            else if (String.Equals(splitCommand[1], "update"))
                            {
                                InputOutputFile.ShowAPersone("-b - update (rewrite) block in crypt file. Example: update -b 5");
                                InputOutputFile.ShowAPersone("-ln - update (rewrite) line in block in crypt file. Example: update -b 5 -ln 4 ");                                
                            }
                            else if (String.Equals(splitCommand[1], "generatepassword"))
                            {
                                InputOutputFile.ShowAPersone("Example: generatePassword 10");
                            }
                            else if (String.Equals(splitCommand[1], "add"))
                            {
                                InputOutputFile.ShowAPersone("-toend - add string to end crypt file as block Example: add -toend");
                                InputOutputFile.ShowAPersone("-inblock - coming soon");
                            }
                            else
                            {
                                InputOutputFile.ShowAPersone("set - set something params");
                                InputOutputFile.ShowAPersone("decrypt - decrypt something");
                                InputOutputFile.ShowAPersone("crypt - crypt something");
                                InputOutputFile.ShowAPersone("search - search in crypt file ");
                                InputOutputFile.ShowAPersone("viewSetting - view path program setting ");
                                InputOutputFile.ShowAPersone("generatePassword - generate random string desired length");
                                InputOutputFile.ShowAPersone("reEnter - allows you to re-enter the parameter");
                                InputOutputFile.ShowAPersone("show - show something");
                                InputOutputFile.ShowAPersone("update - update (rewrite) data in crypt file");
                                InputOutputFile.ShowAPersone("add - add string in crypt file");
                            }
                        }
                        else
                        {
                            InputOutputFile.ShowAPersone("set - set something params");
                            InputOutputFile.ShowAPersone("decrypt - decrypt something");
                            InputOutputFile.ShowAPersone("crypt - crypt something");
                            InputOutputFile.ShowAPersone("search - search in crypt file ");
                            InputOutputFile.ShowAPersone("viewSetting - view path program setting ");
                            InputOutputFile.ShowAPersone("generatePassword - generate random string desired length");
                            InputOutputFile.ShowAPersone("reEnter - allows you to re-enter the parameter");
                            InputOutputFile.ShowAPersone("show - show something");
                            InputOutputFile.ShowAPersone("update - update (rewrite) data in crypt file");
                            InputOutputFile.ShowAPersone("add - add string in crypt file");
                        }
                    }
                    else if (String.Equals(splitCommand[0], "reenter"))
                    {
                        if (splitCommand.Length < 2)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");
                            //InputOutputFile.ShowAPersone("expected \"generatePassword lengthPassword\"");
                        }
                        else
                        {
                            if (String.Equals(splitCommand[1], "-p"))
                            {
                                InputOutputFile.ShowAPersone("Enter password");
                                password = GetHiddenConsoleInput();
                                InputOutputFile.ShowAPersone("Ok");
                            }
                            else
                            {
                                InputOutputFile.ShowAPersone("uncknow command");
                            }
                        }
                    }
                    else if (String.Equals(splitCommand[0], "generatepassword"))
                    {
                        if (splitCommand.Length < 2)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");
                            InputOutputFile.ShowAPersone("expected \"generatePassword lengthPassword\"");
                        }
                        InputOutputFile.ShowAPersone($"password: {CryptoWithoutTry.GeneratePassword(Convert.ToInt32(splitCommand[1]))}");
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
                        if (splitCommand.Length < 2)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");
                        }
                        else
                        {

                            if (String.Equals(splitCommand[1], "-f"))
                            {
                                CheckPassword();
                                HandleCallIntergaceMethods(InputOutputFile.DecryptFile(password));
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
                        if(splitCommand.Length < 2)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");
                        }
                        else
                        {
                            if (String.Equals(splitCommand[1], "-f"))
                            {
                                CheckPassword();
                                HandleCallIntergaceMethods(InputOutputFile.CryptFile(password));
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
                        InputOutputFile.LoadDefaultParams();
                        if (splitCommand.Length < 2)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");
                            //InputOutputFile.ShowAPersone("expected \"search keyWord\"");
                        }                        
                        if (command.IndexOf("-cs")> -1)//case sensetive
                        {
                            InputOutputFile.Toggle_caseSensitive();                            
                        }
                        if (command.IndexOf("-sh") > -1)// search In Header 
                        {
                            InputOutputFile.Toggle_searchInHeader();
                        }
                        if (command.IndexOf("-st") > -1)//search in tegs
                        {
                            InputOutputFile.Toggle_searchInTegs();
                        }
                        if (command.IndexOf("-sufm") > -1)//search until first match
                        {
                            InputOutputFile.Toggle_searchUntilFirstMatch();
                        }
                        if (command.IndexOf("-vsi") > -1)//view service information
                        {
                            InputOutputFile.Toggle_viewServiceInformation();
                        }
                        CheckPassword();
                        HandleCallIntergaceMethods(InputOutputFile.SearchBlockFromCryptRepositoriesUseKeyWord(password,
                            splitCommand[splitCommand.Length - 1]));
                        
                    }
                    else if (String.Equals(splitCommand[0], "show"))
                    {
                        InputOutputFile.LoadDefaultParams();
                        if (splitCommand.Length < 2)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");
                            //InputOutputFile.ShowAPersone("expected \"search keyWord\"");
                        }
                        else
                        {
                            if (command.IndexOf("-vsi") > -1)//view service information
                            {
                                InputOutputFile.Toggle_viewServiceInformation();
                            }
                            if (command.IndexOf("-all") > -1)
                            {
                                CheckPassword();
                                HandleCallIntergaceMethods(InputOutputFile.ShowAllFromCryptFile(password));
                            }
                            else if (command.IndexOf("-b") > -1)
                            {
                                if (splitCommand.Length < 3)
                                {
                                    InputOutputFile.ShowAPersone("too few parameters");                                    
                                }
                                else if ((command.IndexOf("-ln") > -1) && (splitCommand.Length < 3))
                                {
                                    InputOutputFile.ShowAPersone("too few parameters");
                                }
                                else
                                {
                                    int[] vs;
                                    CheckPassword();
                                    if (command.IndexOf("-ln") > -1)
                                        InputOutputFile.ShowAPersone(InputOutputFile.GetBlockData(out vs, password,
                                            Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-b")+1]),
                                            Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-ln") + 1])));
                                    else
                                        InputOutputFile.ShowAPersone(InputOutputFile.GetBlockData(out vs, password,
                                            Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-b") + 1])));
                                }
                            }
                        }
                        
                    }
                    else if (String.Equals(splitCommand[0], "update"))
                    {
                        if (splitCommand.Length < 3)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");                            
                        }
                        else
                        {
                            InputOutputFile.LoadDefaultParams();
                            if (InputOutputFile.Get_viewServiceInformation())
                                InputOutputFile.Toggle_viewServiceInformation();
                            if ((command.IndexOf("-ln") > -1) && (splitCommand.Length < 3))
                            {
                                InputOutputFile.ShowAPersone("too few parameters");
                            }
                            else
                            {
                                int[] vs;
                                CheckPassword();
                                if (command.IndexOf("-ln") > -1)
                                {
                                    InputOutputFile.ShowAPersone("Please, edit line and press \"Enter\"");
                                    InputOutputFile.ShowAPersone(InputOutputFile.GetBlockData(out vs, password,
                                        Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-b") + 1]),
                                        Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-ln") + 1])).TrimEnd(
                                        new char[] { '\r', '\n' }));
                                    string content = Console.ReadLine();
                                    if (QuestionAgreeOrDissagry("Save this line? "))
                                        HandleCallIntergaceMethods(InputOutputFile.Update(password, content, vs));
                                }                                    
                                else
                                {
                                    InputOutputFile.ShowAPersone(InputOutputFile.GetBlockData(out vs, password,
                                        Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-b") + 1])));
                                    string content = ConsoleReadMultiline();
                                    if (QuestionAgreeOrDissagry("Save this line? "))
                                        HandleCallIntergaceMethods(InputOutputFile.Update(password, content, vs));
                                }
                                    
                            }
                        }
                    }
                    else if(String.Equals(splitCommand[0], "viewsetting"))
                    {
                        InputOutputFile.ShowAPersone($"Path {System.Reflection.Assembly.GetEntryAssembly().Location}"); 
                        InputOutputFile.ShowAPersone($"DirCryptFile {InputOutputFile.GetDirCryptFile()}");
                        InputOutputFile.ShowAPersone($"DirDecryptFile {InputOutputFile.GetDirDecryptFile()}");
                        InputOutputFile.ShowAPersone($"caseSensitive {InputOutputFile.Get_caseSensitive()}");
                        InputOutputFile.ShowAPersone($"searchInTegs {InputOutputFile.Get_searchInTegs()}");                        
                        InputOutputFile.ShowAPersone($"searchInHeader {InputOutputFile.Get_searchInHeader()}");
                        InputOutputFile.ShowAPersone($"searchUntilFirstMatch {InputOutputFile.Get_searchUntilFirstMatch()}");
                        InputOutputFile.ShowAPersone($"viewServiceInformation {InputOutputFile.Get_viewServiceInformation()}");
                    }
                    else if (String.Equals(splitCommand[0], "add"))
                    {
                        if (splitCommand.Length < 2)
                        {
                            InputOutputFile.ShowAPersone("too few parameters");
                            //InputOutputFile.ShowAPersone("expected \"search keyWord\"");
                        }
                        else
                        {
                            if (command.IndexOf("-toend") > -1)
                            {
                                CheckPassword();
                                HandleCallIntergaceMethods(InputOutputFile.WriteToEndCryptFile(password, ConsoleReadMultiline()));
                            }
                            else if (command.IndexOf("-inblock") > -1)
                            {
                                InputOutputFile.ShowAPersone($"Coming soon");
                            }
                        }                        
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

        public bool QuestionAgreeOrDissagry(string question)
        {
            while(true)
            {
                Console.WriteLine($"{question} [y/n]");
                string answer = Console.ReadLine().ToLower();
                if (String.Equals(answer, "y"))
                {
                    return true;
                }
                else if (String.Equals(answer, "n"))
                {
                    return false;
                }
            }            
        }

        protected int GetIndexInArray(ref string[] vs, string keyWord)
        {
            int i = 0;
            foreach(string s in vs)
            {
                if(String.Equals(s, keyWord))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        private void CheckPassword()
        {
            if(String.IsNullOrEmpty(password))
            {
                InputOutputFile.ShowAPersone("Enter password");
                password = GetHiddenConsoleInput();
                InputOutputFile.ShowAPersone("Ok");
            }
        }

        private string ConsoleReadMultiline()
        {
            InputOutputFile.ShowAPersone("When you end - write \"end\". If you want to undo the input - write \"cancel\"");
            string line="", result="";
            do
            {
                //input code
                result += $"{line}\r\n";
                //Check for exit conditions
                line = $"{Console.ReadLine()}";
            } while (/*!String.IsNullOrWhiteSpace(line) && */!String.Equals(line.ToLower(), "end")
            && !String.Equals(line.ToLower(), "cancel"));
            if (String.Equals(line.ToLower(), "cancel"))
                return "";
            return result.TrimStart(new char[] { '\r', '\n' });
        }

        private static string GetHiddenConsoleInput()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
                else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
            }
            return input.ToString();
        }

        protected bool HandleCallIntergaceMethods(E_INPUTOUTPUTMESSAGE i_)
        {
            if(i_ != E_INPUTOUTPUTMESSAGE.Ok)
            {
                InputOutputFile.ShowAPersone($"errore {i_.ToString()}");
                return false;
            }
            InputOutputFile.ShowAPersone($"Ok");
            return true;
        }
    }
}
