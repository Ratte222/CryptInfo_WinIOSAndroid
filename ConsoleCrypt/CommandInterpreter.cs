using System;
using System.Collections.Generic;
using System.Text;
using CryptLibrary;
using CommonForCryptPasswordLibrary;
//using System.Void;
namespace ConsoleCrypt
{
    public class CommandInterpreter
    {
        private string password;
        I_InputOutput InputOutputFile;
        ImyIO_Console console_IO;
        ISettings settings;
        public CommandInterpreter(I_InputOutput _InputOutputFile, ImyIO_Console _console_IO, ISettings settings)
        {
            InputOutputFile = _InputOutputFile;
            console_IO = _console_IO;
            this.settings = settings;
        }
        public void Start()
        {
            console_IO.WriteLine("Enter command");
            bool work = false;
            do
            {
                string command = console_IO.ReadLine();
                string[] splitCommand = command.Split(' ');
                splitCommand[0] = splitCommand[0].ToLower();
                if (!String.IsNullOrWhiteSpace(command) && !String.IsNullOrEmpty(command))
                {
                    work = InterpretCommand(splitCommand);
                }
                else
                {
                    console_IO.WriteLine("Command isNull Or WriteSpace Or Empty");
                }
            }
            while (work);
        }

        public bool InterpretCommand(string[] splitCommand)
        {
            if (String.Equals(splitCommand[0].ToLower(), "exit"))
            {
                HandleCallIntergaceMethods(settings.SaveSetting());
                return false;
            }
            else if (String.Equals(splitCommand[0], "help"))
            {
                console_IO.WriteLine("");
                if (splitCommand.Length > 1)
                {
                    splitCommand[1] = splitCommand[1].ToLower();
                    if (String.Equals(splitCommand[1], "set"))
                    {
                        console_IO.WriteLine("-c - set in setting path crypt file. Example: set -c E:\\Crypr.txt");
                        console_IO.WriteLine("-d - set in setting path decrypt file. Example: set -d E:\\Decrypr.txt");
                    }
                    else if (String.Equals(splitCommand[1], "decrypt"))
                    {
                        console_IO.WriteLine("-f - decrypt file. Example: decrypt -f");
                    }
                    else if (String.Equals(splitCommand[1], "crypt"))
                    {
                        console_IO.WriteLine("-f - crypt file. Example: crypt -f ");
                    }
                    else if (String.Equals(splitCommand[1], "search"))
                    {
                        console_IO.WriteLine("search -cs -sh -st -sufm -vsi searchWord");
                        console_IO.WriteLine("-cs  (optional, toggle default param) case sensetive");
                        console_IO.WriteLine("-sh  (optional, toggle default param) search in header");
                        console_IO.WriteLine("-st  (optional, , toggle default param) search in tegs");
                        console_IO.WriteLine("-sufm  (optional, toggle default param) search until first math");
                        console_IO.WriteLine("-vsi  (optional, toggle default param) view setting information");
                    }
                    else if (String.Equals(splitCommand[1], "reenter"))
                    {
                        console_IO.WriteLine("-p - re-enter password. Example: reenter -p");
                    }
                    else if (String.Equals(splitCommand[1], "show"))
                    {
                        console_IO.WriteLine("-b - show block in crypt file. Example: show -b 5 -vsi ");
                        console_IO.WriteLine("-ln - show line in block in crypt file. Example: show -b 5 -ln 4 ");
                        console_IO.WriteLine("-all - show all data in crypt file. Example: show -all -vsi");
                        console_IO.WriteLine("-vsi  (optional, toggle default param) view setting information");
                    }
                    else if (String.Equals(splitCommand[1], "update"))
                    {
                        console_IO.WriteLine("-b - update (rewrite) block in crypt file. Example: update -b 5");
                        console_IO.WriteLine("-ln - update (rewrite) line in block in crypt file. Example: update -b 5 -ln 4 ");
                    }
                    else if (String.Equals(splitCommand[1], "generatepassword"))
                    {
                        console_IO.WriteLine("Example: generatePassword 10");
                    }
                    else if (String.Equals(splitCommand[1], "add"))
                    {
                        console_IO.WriteLine("-toend - add string to end crypt file as block Example: add -toend");
                        console_IO.WriteLine("-inblock - coming soon");
                    }
                    else
                    {
                        console_IO.WriteLine("set - set some params");
                        console_IO.WriteLine("decrypt - decrypt something");
                        console_IO.WriteLine("crypt - crypt something");
                        console_IO.WriteLine("search - search in crypt file ");
                        console_IO.WriteLine("viewSetting - view path program setting ");
                        console_IO.WriteLine("generatePassword - generate random string desired length");
                        console_IO.WriteLine("reEnter - allows you to re-enter the parameter");
                        console_IO.WriteLine("show - show something");
                        console_IO.WriteLine("update - update (rewrite) data in crypt file");
                        console_IO.WriteLine("add - add string in crypt file");
                    }
                }
                else
                {
                    console_IO.WriteLine("set - set some params");
                    console_IO.WriteLine("decrypt - decrypt something");
                    console_IO.WriteLine("crypt - crypt something");
                    console_IO.WriteLine("search - search in crypt file ");
                    console_IO.WriteLine("viewSetting - view path program setting ");
                    console_IO.WriteLine("generatePassword - generate random string desired length");
                    console_IO.WriteLine("reEnter - allows you to re-enter the parameter");
                    console_IO.WriteLine("show - show something");
                    console_IO.WriteLine("update - update (rewrite) data in crypt file");
                    console_IO.WriteLine("add - add string in crypt file");
                }
                console_IO.WriteLine("");
            }
            else if (String.Equals(splitCommand[0], "reenter"))
            {
                if (splitCommand.Length < 2)
                {
                    console_IO.WriteLineTooFewParameters();
                    //console_IO.WriteLine("expected \"generatePassword lengthPassword\"");
                }
                else
                {
                    if (String.Equals(splitCommand[1], "-p"))
                    {
                        console_IO.WriteLine("Enter password");
                        password = console_IO.GetHiddenInput();
                        console_IO.WriteLine("Ok");
                    }
                    else
                    {
                        console_IO.WriteLineUnknownCommand("reenter");
                    }
                }
            }
            else if (String.Equals(splitCommand[0], "generatepassword"))
            {
                if (splitCommand.Length < 2)
                {
                    console_IO.WriteLineTooFewParameters();
                    console_IO.WriteLine("expected \"generatePassword lengthPassword\"");
                }
                int passwordLength;
                if(!Int32.TryParse(splitCommand[1], out passwordLength))
                {
                    console_IO.WriteLine("parameter expected number");
                }
                console_IO.WriteLine($"password: {CryptoWithoutTry.GeneratePassword(passwordLength)}");
            }
            else if (String.Equals(splitCommand[0], "set"))
            {
                //string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitCommand.Length < 3)
                {
                    console_IO.WriteLineTooFewParameters();
                    //console_IO.WriteLine("expected \"search keyCrypt keyWord\"");
                }
                if (String.Equals(splitCommand[1], "-c"))
                {
                    HandleCallIntergaceMethods(settings.SetDirCryptFile(splitCommand[splitCommand.Length - 1]));
                    HandleCallIntergaceMethods(settings.ResetSetting());
                }
                else if (String.Equals(splitCommand[1], "-d"))
                {
                    HandleCallIntergaceMethods(settings.SetDirDecryptFile(splitCommand[splitCommand.Length - 1]));
                    HandleCallIntergaceMethods(settings.ResetSetting());
                }
                else
                {
                    console_IO.WriteLineUnknownCommand("set");
                }
            }
            else if (String.Equals(splitCommand[0], "decrypt"))
            {
                //string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitCommand.Length < 2)
                {
                    console_IO.WriteLineTooFewParameters();
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
                        console_IO.WriteLineUnknownCommand("dectypt");
                    }
                }

            }
            else if (String.Equals(splitCommand[0], "crypt"))
            {
                //string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitCommand.Length < 2)
                {
                    console_IO.WriteLineTooFewParameters();
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
                        console_IO.WriteLineUnknownCommand("ctypt");
                    }
                }
            }
            else if (String.Equals(splitCommand[0], "search"))
            {
                //string[] splitCommand = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);                        
                InputOutputFile.LoadDefaultParams();
                if (splitCommand.Length < 2)
                {
                    console_IO.WriteLineTooFewParameters();
                    console_IO.WriteLine("expected \"search keyWord\"");
                }
                if (_IndexOfInArray(splitCommand, "-cs") > -1)//case sensetive
                {
                    InputOutputFile.Toggle_caseSensitive();
                }
                if (_IndexOfInArray(splitCommand, "-sh") > -1)// search In Header 
                {
                    InputOutputFile.Toggle_searchInHeader();
                }
                if (_IndexOfInArray(splitCommand, "-st") > -1)//search in tegs
                {
                    InputOutputFile.Toggle_searchInTegs();
                }
                if (_IndexOfInArray(splitCommand, "-sufm") > -1)//search until first match
                {
                    InputOutputFile.Toggle_searchUntilFirstMatch();
                }
                if (_IndexOfInArray(splitCommand, "-vsi") > -1)//view service information
                {
                    InputOutputFile.Toggle_viewServiceInformation();
                }
                CheckPassword();
                HandleCallIntergaceMethods(InputOutputFile.SearchBlockFromCryptRepositoriesUseKeyWord(password,
                    splitCommand[splitCommand.Length - 1]));
                console_IO.WriteLine("");
            }
            else if (String.Equals(splitCommand[0], "show"))
            {
                InputOutputFile.LoadDefaultParams();
                if (splitCommand.Length < 2)
                {
                    console_IO.WriteLineTooFewParameters();
                    //console_IO.WriteLine("expected \"search keyWord\"");
                }
                else
                {
                    if (_IndexOfInArray(splitCommand, "-vsi") > -1)//view service information
                    {
                        InputOutputFile.Toggle_viewServiceInformation();
                    }
                    if (_IndexOfInArray(splitCommand, "-all") > -1)
                    {
                        CheckPassword();
                        HandleCallIntergaceMethods(InputOutputFile.ShowAllFromCryptFile(password));
                    }
                    else if (_IndexOfInArray(splitCommand, "-b") > -1)
                    {
                        if (splitCommand.Length < 3)
                        {
                            console_IO.WriteLineTooFewParameters();
                        }
                        else if ((_IndexOfInArray(splitCommand, "-ln") > -1) && (splitCommand.Length < 3))
                        {
                            console_IO.WriteLineTooFewParameters();
                        }
                        else
                        {
                            //int[] vs;
                            CheckPassword();
                            if (_IndexOfInArray(splitCommand, "-ln") > -1)
                                console_IO.WriteLine(InputOutputFile.GetBlockData(out _, password,
                                    Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-b") + 1]),
                                    Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-ln") + 1])));
                            else
                                console_IO.WriteLine(InputOutputFile.GetBlockData(out _, password,
                                    Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-b") + 1])));
                        }
                    }
                }
                console_IO.WriteLine("");
            }
            else if (String.Equals(splitCommand[0], "update"))
            {
                if (splitCommand.Length < 3)
                {
                    console_IO.WriteLineTooFewParameters();
                }
                else
                {
                    InputOutputFile.LoadDefaultParams();
                    if (settings.Get_viewServiceInformation())
                        InputOutputFile.Toggle_viewServiceInformation();
                    if ((_IndexOfInArray(splitCommand, "-ln") > -1) && (splitCommand.Length < 3))
                    {
                        console_IO.WriteLineTooFewParameters();
                    }
                    else
                    {
                        int[] vs;
                        CheckPassword();
                        if (_IndexOfInArray(splitCommand, "-ln") > -1)
                        {
                            console_IO.WriteLine("Please, edit line and press \"Enter\"");
                            console_IO.WriteLine(InputOutputFile.GetBlockData(out vs, password,
                                Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-b") + 1]),
                                Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-ln") + 1])).TrimEnd(
                                new char[] { '\r', '\n' }));
                            console_IO.WriteLine("");
                            string content = Console.ReadLine();
                            if (QuestionAgreeOrDissagry("Save this line? "))
                                HandleCallIntergaceMethods(InputOutputFile.Update(password, content, vs));
                        }
                        else
                        {
                            console_IO.WriteLine("");
                            console_IO.WriteLine(InputOutputFile.GetBlockData(out vs, password,
                                Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-b") + 1])));
                            console_IO.WriteLine("");
                            string content = console_IO.ConsoleReadMultiline();
                            if (QuestionAgreeOrDissagry("Save this line? "))
                                HandleCallIntergaceMethods(InputOutputFile.Update(password, content, vs));
                        }

                    }
                }
            }
            else if (String.Equals(splitCommand[0], "viewsetting"))
            {
                console_IO.WriteLine($"Path {System.Reflection.Assembly.GetEntryAssembly().Location}");
                console_IO.WriteLine($"DirCryptFile {settings.GetDirCryptFile()}");
                console_IO.WriteLine($"DirDecryptFile {settings.GetDirDecryptFile()}");
                console_IO.WriteLine($"caseSensitive {settings.Get_caseSensitive()}");
                console_IO.WriteLine($"searchInTegs {settings.Get_searchInTegs()}");
                console_IO.WriteLine($"searchInHeader {settings.Get_searchInHeader()}");
                console_IO.WriteLine($"searchUntilFirstMatch {settings.Get_searchUntilFirstMatch()}");
                console_IO.WriteLine($"viewServiceInformation {settings.Get_viewServiceInformation()}");
                console_IO.WriteLine("");
            }
            else if (String.Equals(splitCommand[0], "add"))
            {
                if (splitCommand.Length < 2)
                {
                    console_IO.WriteLineTooFewParameters();
                    //console_IO.WriteLine("expected \"search keyWord\"");
                }
                else
                {
                    if (_IndexOfInArray(splitCommand, "-toend") > -1)
                    {
                        CheckPassword();
                        HandleCallIntergaceMethods(InputOutputFile.WriteToEndCryptFile(password,
                            console_IO.ConsoleReadMultiline()));
                    }
                    else if (_IndexOfInArray(splitCommand, "-inblock") > -1)
                    {
                        console_IO.WriteLine($"->Coming soon");
                    }
                }
            }
            else
            {
                console_IO.WriteLine("->Unknown command");
            }
            return true;
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
                console_IO.WriteLine("Enter password");
                password = console_IO.GetHiddenInput();
                console_IO.WriteLine("Ok");
            }
        }

        protected int _IndexOfInArray(string[] args, string needStr)
        {
            int result = -1;
            for(int i = 0; i < args.Length; i++)
            {
                if (String.Equals(args[i], needStr))
                { 
                    result = i;
                    break;
                }
            }
            return result;
        }
        

        protected bool HandleCallIntergaceMethods(E_INPUTOUTPUTMESSAGE i_)
        {
            if(i_ != E_INPUTOUTPUTMESSAGE.Ok)
            {
                console_IO.WriteLine($"errore {i_.ToString()}");
                return false;
            }
            console_IO.WriteLine($"Ok");
            return true;
        }
    }
}
