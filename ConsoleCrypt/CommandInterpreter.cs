using System;
using System.Collections.Generic;
using System.Text;
using CryptLibrary;
using CommonForCryptPasswordLibrary;
using CommonForCryptPasswordLibrary.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using CommonForCryptPasswordLibrary.Filters;
using CommonForCryptPasswordLibrary.Model;
using ConsoleCrypt.DTO;
//using System.Void;
namespace ConsoleCrypt
{
    public class CommandInterpreter
    {
        private string password;
        private I_InputOutput _inputOutputFile;
        ImyIO_Console _console_IO;
        IAppSettings _appSettings;
        ISearchSettings _searchSettings;
        IMapper _mapper;
        ICryptBlock _cryptBlock;
        ICryptGroup _cryptGroup;
        public CommandInterpreter(I_InputOutput _InputOutputFile, ImyIO_Console _console_IO,
            IAppSettings appSettings, ISearchSettings searchSettings, IMapper mapper,
            ICryptBlock cryptBlock, ICryptGroup cryptGroup)
        {
            _inputOutputFile = _InputOutputFile;
            this._console_IO = _console_IO;
            _appSettings = appSettings;
            _searchSettings = searchSettings;
            _mapper = mapper;
            _cryptBlock = cryptBlock;
            _cryptGroup = cryptGroup;
        }
        public void Start()
        {
            _console_IO.WriteLine("Enter command");
            bool work = false;
            do
            {
                string command = _console_IO.ReadLine();
                string[] splitCommand = command.Split(' ');
                splitCommand[0] = splitCommand[0].ToLower();
                if (!String.IsNullOrWhiteSpace(command) && !String.IsNullOrEmpty(command))
                {
                    work = InterpretCommand(splitCommand);
                }
                else
                {
                    _console_IO.WriteLine("Command isNull Or WriteSpace Or Empty");
                }
            }
            while (work);
        }

        public bool InterpretCommand(string[] splitCommand)
        {
            if (String.Equals(splitCommand[0].ToLower(), "exit"))
            {
                //_appSettings.Save();
                //_searchSettings.Save();
                return false;
            }
            var handler = splitCommand[0].ToLower() switch
            {
                "help" => Help(splitCommand),
                "show" => Show(splitCommand),
                "add" => Add(splitCommand),
                "search" => Search(splitCommand),
                "set" => Set(splitCommand),
                "decrypt" => Decrypt(splitCommand),
                "crypt" => Crypt(splitCommand),
                "viewsetting" => ViewSettings(splitCommand),
                "generatepassword" => GeneratePassword(splitCommand),
                "reenter" => ReEnter(splitCommand),
                "update" => Update(splitCommand),
                "initfiles" => InitFiles(splitCommand),
                _ => UnknownCommand(splitCommand)
            };
            handler.GetAwaiter();           
            
            return true;
        }

        private async Task UnknownCommand(string[] splitCommand)
        {
            _console_IO.WriteLine("->Unknown command");
        }

        private async Task Help(string[] splitCommand)
        {
            _console_IO.WriteLine("");
            if (splitCommand.Length > 1)
            {
                splitCommand[1] = splitCommand[1].ToLower();
                if (String.Equals(splitCommand[1], "set"))
                {
                    
                }
                else if (String.Equals(splitCommand[1], "decrypt"))
                {
                    _console_IO.WriteLine("-f - decrypt file. Example: decrypt -f");
                }
                else if (String.Equals(splitCommand[1], "crypt"))
                {
                    _console_IO.WriteLine("-f - crypt file. Example: crypt -f ");
                }
                else if (String.Equals(splitCommand[1], "search"))
                {
                    _console_IO.WriteLine("search [-cs -se -sufm] searchWord");
                    _console_IO.WriteLine("-cs  (optional, toggle default param) case sensetive");
                    _console_IO.WriteLine("-se  (optional, toggle default param) search everywhere");
                    //_console_IO.WriteLine("-st  (optional, , toggle default param) search in tegs");
                    _console_IO.WriteLine("-sufm  (optional, toggle default param) search until first math");
                    //_console_IO.WriteLine("-vsi  (optional, toggle default param) view setting information");
                }
                else if (String.Equals(splitCommand[1], "initfiles"))
                {
                    _console_IO.WriteLine("-c init crypt files Example: intifiles -c");
                    //_console_IO.WriteLine("-d init decrypt files Example: intifiles -d");
                }
                else if (String.Equals(splitCommand[1], "reenter"))
                {
                    _console_IO.WriteLine("-p - re-enter password. Example: reenter -p");
                }
                else if (String.Equals(splitCommand[1], "show"))
                {
                    _console_IO.WriteLine("There must be at least one parameter ([-b] or [-g] or [-all]) ");
                    _console_IO.WriteLine("-b - show blocks. Example: show -b google ");
                    _console_IO.WriteLine("-g - show block in group. Example: show -b google -ln work ");
                    _console_IO.WriteLine("-allblocks - show all blocks and groups in crypt file. Example: show -allblocks");
                    _console_IO.WriteLine("-allgroups - show all groups in crypt file. Example: show -allgroups");
                    _console_IO.WriteLine("-cs  (optional, toggle default param) case sensetive." +
                        " Example: show -b Google -cs");
                }
                else if (String.Equals(splitCommand[1], "update"))
                {
                    //_console_IO.WriteLine("-b - update (rewrite) block in crypt file. Example: update -b 5");
                    //_console_IO.WriteLine("-ln - update (rewrite) line in block in crypt file. Example: update -b 5 -ln 4 ");
                }
                else if (String.Equals(splitCommand[1], "generatepassword"))
                {
                    _console_IO.WriteLine("Example: generatePassword 10");
                }
                else if (String.Equals(splitCommand[1], "add"))
                {
                    _console_IO.WriteLine("-block - add block data to selected group. Example: add -block [name group]");
                    _console_IO.WriteLine("-group - add group data. Example: add -group");
                    //_console_IO.WriteLine("-inblock - coming soon");
                }
                else
                {
                    _console_IO.WriteLine("set - set some params");
                    _console_IO.WriteLine("decrypt - decrypt something");
                    _console_IO.WriteLine("crypt - crypt something");
                    _console_IO.WriteLine("search - search in crypt file ");
                    _console_IO.WriteLine("viewSetting - view path program setting ");
                    _console_IO.WriteLine("generatePassword - generate random string desired length");
                    _console_IO.WriteLine("reEnter - allows you to re-enter the parameter");
                    _console_IO.WriteLine("show - show something");
                    _console_IO.WriteLine("update - update (rewrite) data in crypt file");
                    _console_IO.WriteLine("add - add string in crypt file");
                    _console_IO.WriteLine("initfiles - init files");
                }
            }
            else
            {
                _console_IO.WriteLine("set - set some params");
                _console_IO.WriteLine("decrypt - decrypt something");
                _console_IO.WriteLine("crypt - crypt something");
                _console_IO.WriteLine("search - search in crypt file ");
                _console_IO.WriteLine("viewSetting - view path program setting ");
                _console_IO.WriteLine("generatePassword - generate random string desired length");
                _console_IO.WriteLine("reEnter - allows you to re-enter the parameter");
                _console_IO.WriteLine("show - show something");
                _console_IO.WriteLine("update - update (rewrite) data in crypt file");
                _console_IO.WriteLine("add - add string in crypt file");
                _console_IO.WriteLine("initfiles - init files");
            }
            _console_IO.WriteLine("");
        }

        private async Task Search(string[] splitCommand)
        {
            _inputOutputFile.LoadDefaultParams();
            if (splitCommand.Length < 2)
            {
                _console_IO.WriteLineTooFewParameters();
                _console_IO.WriteLine("expected \"search keyWord\"");
            }
            if (_IndexOfInArray(splitCommand, "-cs") > -1)//case sensetive
            {
                _inputOutputFile.Toggle_caseSensitive();
            }
            if (_IndexOfInArray(splitCommand, "-se") > -1)//search everywhere
            {
                _inputOutputFile.Toggle_searchEverywhere();
            }
            //if (_IndexOfInArray(splitCommand, "-sh") > -1)// search In Header 
            //{
            //    _inputOutputFile.Toggle_searchInHeader();
            //}
            //if (_IndexOfInArray(splitCommand, "-st") > -1)//search in tegs
            //{
            //    _inputOutputFile.Toggle_searchInTegs();
            //}
            bool sufm = _searchSettings.SearchUntilFirstMatch;
            if (_IndexOfInArray(splitCommand, "-sufm") > -1)//search until first match
            {
                sufm = !sufm;
            }
            //if (_IndexOfInArray(splitCommand, "-vsi") > -1)//view service information
            //{
            //    _inputOutputFile.Toggle_viewServiceInformation();
            //}
            CheckPassword();
            LoadTheDatabaseIfNeeded();
            Filter filterShow = new Filter();
            filterShow.BlockName = splitCommand[splitCommand.Length - 1];
            if (sufm)
            {
                var res = _inputOutputFile.GetBlockData(filterShow);
                if (res == null)
                {
                    _console_IO.WriteLine("Nothing found");
                }
                else
                {
                    _console_IO.Show(_mapper.Map<CryptBlockModel, BlockDataDTO>(res));
                }
            }
            else
            {
                var res = _inputOutputFile.GetBlockDatas(filterShow);
                if (res.Count == 0)
                {
                    _console_IO.WriteLine("Nothing found");
                }
                else 
                { 
                    _console_IO.Show(_mapper.Map<List<CryptBlockModel>, List<BlockDataDTO>>(res)); 
                }

            }
        }

        private async Task Show(string[] splitCommand)
        {
            try
            {
                _inputOutputFile.LoadDefaultParams();
                if (splitCommand.Length < 2)
                {
                    _console_IO.WriteLineTooFewParameters();
                    //console_IO.WriteLine("expected \"search keyWord\"");
                }
                else
                {
                    if (_IndexOfInArray(splitCommand, "-cs") > -1)//case sensetive
                    {
                        _inputOutputFile.Toggle_caseSensitive();
                    }
                    if (_IndexOfInArray(splitCommand, "-allblocks") > -1)
                    {
                        CheckPassword();
                        LoadTheDatabaseIfNeeded();
                        List<CryptGroupModel> models = _cryptGroup.GetAll_List();                        
                        _console_IO.Show(_mapper.Map<List<CryptGroupModel>, List<GroupDataDTO>>(models));
                        
                    }
                    else if (_IndexOfInArray(splitCommand, "-allgroups") > -1)
                    {
                        CheckPassword();
                        LoadTheDatabaseIfNeeded();
                        List<CryptGroupModel> models = _cryptGroup.GetAll_List();
                        foreach(var group in _mapper.Map<List<CryptGroupModel>, List<GroupDataDTO>>(models))
                        {
                            _console_IO.WriteLine(group.ToString());
                            _console_IO.WriteLine("");
                        }
                    }
                    else if (_IndexOfInArray(splitCommand, "-b") > -1)
                    {
                        if (splitCommand.Length < 3)
                        {
                            _console_IO.WriteLineTooFewParameters();
                        }
                        else if ((_IndexOfInArray(splitCommand, "-g") > -1) && (splitCommand.Length < 3))
                        {
                            _console_IO.WriteLineTooFewParameters();
                        }
                        else
                        {
                            //int[] vs;
                            CheckPassword();
                            LoadTheDatabaseIfNeeded();
                            Filter filterShow = new Filter();
                            filterShow.BlockName = splitCommand[GetIndexInArray(ref splitCommand, "-b") + 1];
                            if (_IndexOfInArray(splitCommand, "-g") > -1)
                            {
                                filterShow.GroupName = splitCommand[GetIndexInArray(ref splitCommand, "-g") + 1];
                                var res = _inputOutputFile.GetBlockData(filterShow);
                                if (res == null)
                                {
                                    _console_IO.WriteLine("Nothing found");
                                }
                                else
                                {
                                    _console_IO.Show(_mapper.Map<CryptBlockModel, BlockDataDTO>(res));
                                }
                            }
                            else
                            {
                                var res = _inputOutputFile.GetBlockDatas(filterShow);
                                if (res.Count == 0)
                                {
                                    _console_IO.WriteLine("Nothing found");
                                }
                                else
                                {
                                    _console_IO.Show(_mapper.Map<List<CryptBlockModel>, List<BlockDataDTO>>(res));
                                }
                            }
                        }
                    }
                    //_console_IO.WriteLine("");
                }                
            }
            catch(Exception ex)
            {

            }
            
        }

        private async Task Add(string[] splitCommand)
        {
            if (splitCommand.Length < 2)
            {
                _console_IO.WriteLineTooFewParameters();
                //console_IO.WriteLine("expected \"search keyWord\"");
            }
            else
            {
                CheckPassword();
                LoadTheDatabaseIfNeeded();
                if (_IndexOfInArray(splitCommand, "-block") > -1)
                {
                    if (splitCommand.Length < 3)
                    {
                        _console_IO.WriteLineTooFewParameters();
                        return;
                    }
                    CryptGroupModel cryptGroupModel = _cryptGroup.Get(i => i.Name.ToLower()
                        .Contains(splitCommand[GetIndexInArray(ref splitCommand, "-block") + 1].ToLower()));
                    if(cryptGroupModel == null)
                    {
                        _console_IO.WriteLine("Sorry, group with this name was not found");
                    }
                    else
                    {
                        _console_IO.WriteLine($"Target group:\r\n" +
                            $"{cryptGroupModel.ToString()}\r\n");
                        CryptBlockModel cryptBlockModel = AddBlock();
                        cryptBlockModel.GroupId = cryptGroupModel.Id;
                        if (this.QuestionAgreeOrDissagry($"Add a block to group?"))
                        {
                            _cryptBlock.Add(cryptBlockModel);
                        }
                    }
                }
                else if (_IndexOfInArray(splitCommand, "-group") > -1)
                {
                    CryptGroupModel cryptGroupModel = AddGroup();
                    _cryptGroup.Add(cryptGroupModel);
                }
            }
        }

        private async Task GeneratePassword(string[] splitCommand)
        {
            if (splitCommand.Length < 2)
            {
                _console_IO.WriteLineTooFewParameters();
                _console_IO.WriteLine("expected \"generatePassword lengthPassword\"");
            }
            int passwordLength;
            if (!Int32.TryParse(splitCommand[1], out passwordLength))
            {
                _console_IO.WriteLine("parameter expected number");
            }
            _console_IO.WriteLine($"password: {CryptoWithoutTry.GeneratePassword(passwordLength)}");
        }

        private async Task ReEnter(string[] splitCommand)
        {
            if (splitCommand.Length < 2)
            {
                _console_IO.WriteLineTooFewParameters();
                //console_IO.WriteLine("expected \"generatePassword lengthPassword\"");
            }
            else
            {
                if (String.Equals(splitCommand[1], "-p"))
                {
                    _console_IO.WriteLine("Enter password");
                    password = _console_IO.GetHiddenInput();
                    _console_IO.WriteLine("Ok");
                }
                else
                {
                    _console_IO.WriteLineUnknownCommand("reenter");
                }
            }
        }

        private async Task ViewSettings(string[] splitCommand)
        {
            _console_IO.WriteLine($"Path {System.Reflection.Assembly.GetEntryAssembly().Location}");
            //console_IO.WriteLine($"DirCryptFile {_appSettings.GetDirCryptFile()}");
            //console_IO.WriteLine($"DirDecryptFile {settings.GetDirDecryptFile()}");
            //console_IO.WriteLine($"caseSensitive {settings.Get_caseSensitive()}");
            //console_IO.WriteLine($"searchInTegs {settings.Get_searchInTegs()}");
            //console_IO.WriteLine($"searchInHeader {settings.Get_searchInHeader()}");
            //console_IO.WriteLine($"searchUntilFirstMatch {settings.Get_searchUntilFirstMatch()}");
            //console_IO.WriteLine($"viewServiceInformation {settings.Get_viewServiceInformation()}");
            _console_IO.WriteLine("");
        }

        private async Task InitFiles(string[] splitCommand)
        {
            if (splitCommand.Length < 2)
            {
                _console_IO.WriteLineTooFewParameters();
                _console_IO.WriteLine("expected \"initfiles -c\" or \"initfiles -d\"");
            }
            CheckPassword();
            if (splitCommand[1] == "-c")
            {
                if(this.QuestionAgreeOrDissagry("Really initialize all encrypted files"))
                _inputOutputFile.InitCryptFiles(password); 
            }
            //else if (splitCommand[1] == "-d")
            //    _inputOutputFile.InitCryptFiles(password);
            _console_IO.WriteLine("Files init successsfully");
        }

        private async Task Set(string[] splitCommand)
        {
            if (splitCommand.Length < 3)
            {
                _console_IO.WriteLineTooFewParameters();
                //console_IO.WriteLine("expected \"search keyCrypt keyWord\"");
            }
            if (String.Equals(splitCommand[1], "-c"))
            {
                //HandleCallIntergaceMethods(settings.SetDirCryptFile(splitCommand[splitCommand.Length - 1]));
                //HandleCallIntergaceMethods(settings.SaveSetting());
            }
            else if (String.Equals(splitCommand[1], "-d"))
            {
                //HandleCallIntergaceMethods(settings.SetDirDecryptFile(splitCommand[splitCommand.Length - 1]));
                //HandleCallIntergaceMethods(settings.SaveSetting());
            }
            else
            {
                _console_IO.WriteLineUnknownCommand("set");
            }
        }

        private async Task Decrypt(string[] splitCommand)
        {
            if (splitCommand.Length < 2)
            {
                _console_IO.WriteLineTooFewParameters();
            }
            else
            {

                if (String.Equals(splitCommand[1], "-f"))
                {
                    CheckPassword();
                    HandleCallIntergaceMethods(_inputOutputFile.DecryptFile(password));
                }
                else
                {
                    _console_IO.WriteLineUnknownCommand("dectypt");
                }
            }
        }

        private async Task Crypt(string[] splitCommand)
        {
            if (splitCommand.Length < 2)
            {
                _console_IO.WriteLineTooFewParameters();
            }
            else
            {
                if (String.Equals(splitCommand[1], "-f"))
                {
                    CheckPassword();
                    HandleCallIntergaceMethods(_inputOutputFile.CryptFile(password));
                }
                else
                {
                    _console_IO.WriteLineUnknownCommand("ctypt");
                }
            }
        }


        private async Task Update(string[] splitCommand)
        {
            if (splitCommand.Length < 3)
            {
                _console_IO.WriteLineTooFewParameters();
            }
            else
            {
                _inputOutputFile.LoadDefaultParams();
                if (_searchSettings.ViewServiceInformation)
                    _inputOutputFile.Toggle_viewServiceInformation();
                if ((_IndexOfInArray(splitCommand, "-ln") > -1) && (splitCommand.Length < 3))
                {
                    _console_IO.WriteLineTooFewParameters();
                }
                else
                {
                    int[] vs;
                    CheckPassword();
                    if (_IndexOfInArray(splitCommand, "-ln") > -1)
                    {
                        _console_IO.WriteLine("Please, edit line and press \"Enter\"");
                        _console_IO.WriteLine(_inputOutputFile.GetBlockData(out vs, password,
                            Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-b") + 1]),
                            Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-ln") + 1])).TrimEnd(
                            new char[] { '\r', '\n' }));
                        _console_IO.WriteLine("");
                        string content = Console.ReadLine();
                        if (QuestionAgreeOrDissagry("Save this line? "))
                            HandleCallIntergaceMethods(_inputOutputFile.Update(password, content, vs));
                    }
                    else
                    {
                        _console_IO.WriteLine("");
                        _console_IO.WriteLine(_inputOutputFile.GetBlockData(out vs, password,
                            Convert.ToInt32(splitCommand[GetIndexInArray(ref splitCommand, "-b") + 1])));
                        _console_IO.WriteLine("");
                        string content = _console_IO.ConsoleReadMultiline();
                        if (QuestionAgreeOrDissagry("Save this line? "))
                            HandleCallIntergaceMethods(_inputOutputFile.Update(password, content, vs));
                    }

                }
            }
        }

        private CryptBlockModel AddBlock()
        {
            CryptBlockModel cryptBlockModel = new CryptBlockModel();
            _console_IO.WriteLine($"Fill {nameof(cryptBlockModel.Title)} and press \"Enter\"");
            cryptBlockModel.Title = _console_IO.ReadLine();
            _console_IO.WriteLine($"Fill {nameof(cryptBlockModel.Description)} and press \"Enter\"");
            cryptBlockModel.Description = _console_IO.ReadLine();
            _console_IO.WriteLine($"Fill {nameof(cryptBlockModel.Email)} and press \"Enter\"");
            cryptBlockModel.Email = _console_IO.ReadLine();
            _console_IO.WriteLine($"Fill {nameof(cryptBlockModel.UserName)} and press \"Enter\"");
            cryptBlockModel.UserName = _console_IO.ReadLine();
            _console_IO.WriteLine($"Fill {nameof(cryptBlockModel.Password)} and press \"Enter\"");
            cryptBlockModel.Password = _console_IO.ReadLine();
            _console_IO.WriteLine($"Fill {nameof(cryptBlockModel.Phone)} and press \"Enter\"");
            cryptBlockModel.Phone = _console_IO.ReadLine();            
            Console.Write($"Fill {nameof(cryptBlockModel.AdditionalInfo)} ");
            cryptBlockModel.AdditionalInfo = _console_IO.ConsoleReadMultiline();
            return cryptBlockModel;
        }

        private CryptGroupModel AddGroup()
        {
            CryptGroupModel cryptGroupModel = new CryptGroupModel();
            _console_IO.WriteLine($"Fill {nameof(cryptGroupModel.Name)} and press \"Enter\"");
            cryptGroupModel.Name = _console_IO.ReadLine();
            _console_IO.WriteLine($"Fill {nameof(cryptGroupModel.Description)} and press \"Enter\"");
            cryptGroupModel.Description = _console_IO.ReadLine();
            return cryptGroupModel;
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

        private void LoadTheDatabaseIfNeeded()
        {
            var settings = new CommonForCryptPasswordLibrary.Model.CryptDecryptSettings()
            {
                Key = password,
                Path = _appSettings.DefaultCryptFile.Path
            };
            if (!_cryptBlock.DataExist)
            {
                _cryptBlock.LoadData(settings);
            }
            if (!_cryptGroup.DataExist)
            {
                _cryptBlock.LoadData(settings);
            }
        }

        private void CheckPassword()
        {
            if(String.IsNullOrEmpty(password))
            {
                _console_IO.WriteLine("Enter password");
                password = _console_IO.GetHiddenInput();
                _console_IO.WriteLine("Ok");
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
                _console_IO.WriteLine($"errore {i_.ToString()}");
                return false;
            }
            _console_IO.WriteLine($"Ok");
            return true;
        }
    }
}
