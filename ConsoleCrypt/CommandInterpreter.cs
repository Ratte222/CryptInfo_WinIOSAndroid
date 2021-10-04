using System;
using System.Collections.Generic;
using System.Linq;
using CryptLibrary;
using CommonForCryptPasswordLibrary;
using CommonForCryptPasswordLibrary.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using CommonForCryptPasswordLibrary.Filters;
using CommonForCryptPasswordLibrary.Model;
using ConsoleCrypt.DTO;
using CommandLine;
using ConsoleCrypt.Commands;
using CommonForCryptPasswordLibrary.Exceptions;
//using System.Void;
namespace ConsoleCrypt
{
    public class CommandInterpreter
    {
        private string password;
        private string Password {
            get
            {
                return password;
            }
            set 
            {
                if (!String.IsNullOrEmpty(value) && !String.IsNullOrWhiteSpace(value))
                {
                    password = value;
                }
            }
        }
        private IMainLogicService _inputOutputFile;
        ImyIO_Console _console_IO;
        IAppSettings _appSettings;
        ISearchSettings _searchSettings;
        IMapper _mapper;
        IBlockService _cryptBlock;
        IGroupService _cryptGroup;
        public CommandInterpreter(IMainLogicService _InputOutputFile, ImyIO_Console _console_IO,
            IAppSettings appSettings, ISearchSettings searchSettings, IMapper mapper,
            IBlockService cryptBlock, IGroupService cryptGroup)
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
                //var parser = new FluentCommandLineParser<AppCommand>();
                //parser.Setup<bool>(arg => arg.CaseSensetive)
                //    .As("cs")
                //    .WithDescription("Case sensetive")
                //    .SetDefault(false);
                //parser.Setup<Search>("search")
                //    .
                //parser.SetupHelp("?", "h", "help")
                //.Callback(text => Console.WriteLine(text));
                //var parseResult = parser.Parse(splitCommand);


                string[] splitCommand = command.Split(' ');
                splitCommand[0] = splitCommand[0].ToLower();

                work = InterpretCommand(splitCommand);
                //if (!String.IsNullOrWhiteSpace(command) && !String.IsNullOrEmpty(command))
                //{
                //    work = InterpretCommand(splitCommand);
                //}
                //else
                //{
                //    //_console_IO.WriteLine("Command isNull Or WriteSpace Or Empty");
                //    Help(null);
                //}
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
            try
            {
                //CommandLine.Parser.Default.ParseArguments<SearchCommand, ShowCommand>(splitCommand)
                      //.WithParsed<SearchCommand, ShowCommand>(Search, Show);
                 var result = Parser.Default.ParseArguments<SearchCommand, ShowCommand, CreateCommand, UpdateCommand, DecryptCommand,
                     EncryptCommand, InitCommand, ReEnterCommand, GeneratePasswordCommand, ViewSettingsCommand>(splitCommand);
                result
                   .WithParsed<SearchCommand>(Search)
                   .WithParsed<ShowCommand>(Show)
                   .WithParsed<CreateCommand>(Create)
                   .WithParsed<UpdateCommand>(Update)
                   .WithParsed<DecryptCommand>(Decrypt)
                   .WithParsed<EncryptCommand>(Encrypt)
                   .WithParsed<InitCommand>(Init)
                   .WithParsed<ReEnterCommand>(ReEnter)                   
                   .WithParsed<GeneratePasswordCommand>(GeneratePassword)
                   .WithParsed<ViewSettingsCommand>(ViewSettings);

                    //.MapResult(
                    //    (SearchCommand opts) => { Search(opts); return true; },
                    //    (ShowCommand opts) => { Show(opts); return true; },
                    //    (CreateCommand opts) => { Create(opts); return true; },
                    //    _ => {  UnknownCommand(); return true; }
                    //    );
            }
            catch(ValidationException ex)
            {
#if DEBUG
                _console_IO.HandleMessage("", ex);
#else
                _console_IO.WriteLine($"{ex?.Message}");
#endif
            }
            catch(ReadFromCryptFileException ex)
            {
#if DEBUG
                _console_IO.HandleMessage("", ex);
#else
                _console_IO.WriteLine($"{ex?.Message}");
#endif
            }
            catch (TheFileIsDamagedException ex)
            {
#if DEBUG
                _console_IO.HandleMessage("", ex);
#else
                _console_IO.WriteLine($"{ex?.Message}");
#endif
            }
            catch (Exception ex)
            {
#if DEBUG
                _console_IO.HandleMessage("", ex);
#else
                _console_IO.WriteLine("Oops! An unexpected error occurred.");
#endif

            }
            //var handler = splitCommand[0].ToLower() switch
            //{
            //    "help" => Help(splitCommand),
            //    "show" => Show(splitCommand),
            //    "create" => Create(splitCommand),
            //    "search" => Search(splitCommand),
            //    "set" => Set(splitCommand),
            //    "decrypt" => Decrypt(splitCommand),
            //    "encrypt" => Encrypt(splitCommand),
            //    "viewsetting" => ViewSettings(splitCommand),
            //    "generatepassword" => GeneratePassword(splitCommand),
            //    "reenter" => ReEnter(splitCommand),
            //    "update" => Update(splitCommand),
            //    "initfiles" => InitFiles(splitCommand),
            //    _ => UnknownCommand(splitCommand)
            //};
            //handler.GetAwaiter();           
            //            try 
            //            {
            //                switch (splitCommand[0].ToLower())
            //                {
            //                    case "help":
            //                        Help(splitCommand);
            //                        break;
            //                    case "show":
            //                        Show(splitCommand);
            //                        break;
            //                    case "search":
            //                        Search(splitCommand);
            //                        break;
            //                    case "create":
            //                        Create(splitCommand);
            //                        break;
            //                    case "set":
            //                        Set(splitCommand);
            //                        break;
            //                    case "decrypt":
            //                        Decrypt(splitCommand);
            //                        break;
            //                    case "encrypt":
            //                        Encrypt(splitCommand);
            //                        break;
            //                    case "viewsetting":
            //                        ViewSettings(splitCommand);
            //                        break;
            //                    case "generatepassword":
            //                        GeneratePassword(splitCommand);
            //                        break;
            //                    case "reenter":
            //                        ReEnter(splitCommand);
            //                        break;
            //                    case "update":
            //                        Update(splitCommand);
            //                        break;
            //                    case "initfiles":
            //                        InitFiles(splitCommand);
            //                        break;
            //                    default:
            //                        Help(splitCommand);
            //                        break;
            //                }
            //            }
            //            catch(Exception ex)
            //            {
            //#if DEBUG
            //                _console_IO.HandleMessage("", ex);
            //#endif
            //            }


            return true;
        }

        private void UnknownCommand()
        {
            _console_IO.WriteLine("->Unknown command");
        }

        private void Help(string[] splitCommand)
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
                else if (String.Equals(splitCommand[1], "encrypt"))
                {
                    _console_IO.WriteLine("-f - encrypt file. Example: encrypt -f ");
                }
                else if (String.Equals(splitCommand[1], "search"))
                {
                    _console_IO.WriteLine("search [-cs -se -sufm] [searchWord]");
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
                    _console_IO.WriteLine("There must be at least one parameter ([-b] or [-g] or [-allblocks] or [-allgroups]) ");
                    _console_IO.WriteLine("-b - show blocks. There must also be aparameter [-g]. " +
                        "After [-b] it should contain the name of the block.  Example: show -g work -b google ");
                    _console_IO.WriteLine("-g - show block in group. After [-g] it should contain the name of the group. Example: show -b google -ln work ");
                    _console_IO.WriteLine("-allblocks - show all blocks and groups in crypt file. Example: show -allblocks");
                    _console_IO.WriteLine("-allgroups - show all groups in crypt file. Example: show -allgroups");
                    _console_IO.WriteLine("-cs  (optional, toggle default param) case sensetive." +
                        " Example: show -g Work -cs");
                }
                else if (String.Equals(splitCommand[1], "update"))
                {
                    _console_IO.WriteLine("There must be at least one parameter ([-b] or [-g]) ");
                    _console_IO.WriteLine("-b - update block fields. There must also be aparameter [-g]. " +
                        "After [-b] it should contain the name of the block.  Example: show -g work -b google ");
                    _console_IO.WriteLine("-g - update group fields. After [-g] it should contain the name of the group." +
                        " Example: show -b google -ln work ");
                    //_console_IO.WriteLine("-ln - update (rewrite) line in block in crypt file. Example: update -b 5 -ln 4 ");
                }
                else if (String.Equals(splitCommand[1], "generatepassword"))
                {
                    _console_IO.WriteLine("Example: generatePassword 10");
                }
                else if (String.Equals(splitCommand[1], "create"))
                {
                    _console_IO.WriteLine("There must be at least one parameter ([-block] or [-group]) ");
                    _console_IO.WriteLine("-block - add block data to selected group. Example: add -block [name group]");
                    _console_IO.WriteLine("-group - add group data. Example: add -group");
                    //_console_IO.WriteLine("-inblock - coming soon");
                }
                else
                {
                    _console_IO.WriteLine("set - set some params");
                    _console_IO.WriteLine("decrypt - decrypt something");
                    _console_IO.WriteLine("encrypt - encrypt something");
                    _console_IO.WriteLine("search - search in crypt file ");
                    _console_IO.WriteLine("viewSetting - view path program setting ");
                    _console_IO.WriteLine("generatePassword - generate random string desired length");
                    _console_IO.WriteLine("reEnter - allows you to re-enter the parameter");
                    _console_IO.WriteLine("show - show something");
                    _console_IO.WriteLine("update - update (rewrite) data in crypt file");
                    _console_IO.WriteLine("create - create something");
                    _console_IO.WriteLine("initfiles - init files");
                }
            }
            else
            {
                _console_IO.WriteLine("set - set some params");
                _console_IO.WriteLine("decrypt - decrypt something");
                _console_IO.WriteLine("encrypt - encrypt something");
                _console_IO.WriteLine("search - search in crypt file ");
                _console_IO.WriteLine("viewSetting - view path program setting ");
                _console_IO.WriteLine("generatePassword - generate random string desired length");
                _console_IO.WriteLine("reEnter - allows you to re-enter the parameter");
                _console_IO.WriteLine("show - show something");
                _console_IO.WriteLine("update - update (rewrite) data in crypt file");
                _console_IO.WriteLine("create - create something");
                _console_IO.WriteLine("initfiles - init files");
            }
            _console_IO.WriteLine("");
        }

        private void Search(SearchCommand command)
        {
            _inputOutputFile.LoadDefaultParams();
            if(command.CaseSensetive) _inputOutputFile.Toggle_caseSensitive();
            if(command.SearchEverywhere) _inputOutputFile.Toggle_searchEverywhere();
            CheckPassword(command.Password);
            LoadTheDatabaseIfNeeded();
            Filter filterShow = new Filter();
            filterShow.BlockName = command.KeyWord;
            command.SearchUntilFirstMatch = !command.SearchUntilFirstMatch;
            if (command.SearchUntilFirstMatch)
            {
                var res = _inputOutputFile.GetBlockData(filterShow);
                if (res == null)
                {
                    _console_IO.WriteLine("Nothing found");
                }
                else
                {
                    if(command.ShowGroup)
                    {
                        var group = _cryptGroup.GetAll_List().Find(i => i.Id == res.GroupId);
                        if(group is null)
                        {
                            _console_IO.WriteLine($"Block \"{res.Title}\" does not belond to the group");                            
                        }
                        else
                        {
                            _console_IO.Show(_mapper.Map<GroupModel, GroupDataDTO>(group), false);
                        }
                    }
                    _console_IO.Show(_mapper.Map<BlockModel, BlockDataDTO>(res));
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
                    if(command.ShowGroup)
                    {
                        var temp = res.GroupBy(p => p.GroupId);
                        foreach (var ig in temp)
                        {
                            var group = _cryptGroup.GetAll_List().Find(i => i.Id == ig.Key);
                            if (group is null)
                            {
                                _console_IO.WriteLine($"Block \"{ig.FirstOrDefault()?.Title}\" does not belond to the group");
                            }
                            else
                            {
                                _console_IO.Show(_mapper.Map<GroupModel, GroupDataDTO>(group), false);
                            }
                            foreach (var b in ig)
                            {
                                _console_IO.Show(_mapper.Map<BlockModel, BlockDataDTO>(b));
                            }

                        }
                    }
                    else
                    {
                        _console_IO.Show(_mapper.Map<List<BlockModel>, List<BlockDataDTO>>(res));
                    }
                    
                }

            }
        }

        
        private void Show(ShowCommand command)
        {
            _inputOutputFile.LoadDefaultParams();
            if (command.CaseSensetive) _inputOutputFile.Toggle_caseSensitive();
            CheckPassword(command.Password);
            LoadTheDatabaseIfNeeded();
            if (command.AllBlocks)
            {
                
                List<GroupModel> models = _cryptGroup.GetAll_List();
                _console_IO.Show(_mapper.Map<List<GroupModel>, List<GroupDataDTO>>(models));

            }
            else if (command.AllGroups)
            {                
                List<GroupModel> models = _cryptGroup.GetAll_List();
                foreach (var group in _mapper.Map<List<GroupModel>, List<GroupDataDTO>>(models))
                {
                    _console_IO.WriteLine(group.ToString());
                    _console_IO.WriteLine("");
                }
            }
            else 
            {
                Filter filterShow = new Filter();
                filterShow.BlockName = command.Block;
                if (!String.IsNullOrWhiteSpace(command.Group))
                {
                    filterShow.GroupName = command.Group;
                    var res = _inputOutputFile.GetBlockData(filterShow);
                    if (res == null)
                    {
                        _console_IO.WriteLine("Nothing found");
                    }
                    else
                    {
                        _console_IO.Show(_mapper.Map<BlockModel, BlockDataDTO>(res));
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
                        _console_IO.Show(_mapper.Map<List<BlockModel>, List<BlockDataDTO>>(res));
                    }
                }
            }
        }

        private void Create(CreateCommand command)
        {
            CheckPassword(command.Password);
            LoadTheDatabaseIfNeeded();
            if (command.Block)
            {
                _console_IO.WriteLine("Entered group name (full or partial)");
                string groupName = _console_IO.ReadLine();
                GroupModel cryptGroupModel = _cryptGroup.Get(i => i.Name.ToLower()
                    .Contains(groupName.ToLower()));                
                if (cryptGroupModel == null)
                {
                    _console_IO.WriteLine("Sorry, group with this name was not found");
                }
                else
                {
                    //_console_IO.WriteLine($"Target group:\r\n" +
                    //    $"{cryptGroupModel.ToString()}\r\n");
                    _console_IO.Show(_mapper.Map<GroupModel, GroupDataDTO>(cryptGroupModel), false);
                    if (!QuestionAgreeOrDissagry($"Is this the right group? "))
                    {
                        return;
                    }
                    BlockModel cryptBlockModel = AddBlock();
                    cryptBlockModel.GroupId = cryptGroupModel.Id;
                    if (this.QuestionAgreeOrDissagry($"Add a block to group?"))
                    {
                        _cryptBlock.Add(cryptBlockModel);
                        _console_IO.WriteLine($"Block \"{cryptBlockModel.Title}\" created successfully");
                    }
                }
            }
            else if (command.Group)
            {
                GroupModel cryptGroupModel = AddGroup();
                if (this.QuestionAgreeOrDissagry($"Add a group?"))
                {
                    _cryptGroup.Add(cryptGroupModel);
                    _console_IO.WriteLine($"Group \"{cryptGroupModel.Name}\" created successfully");
                }
            }
        }

        private void GeneratePassword(GeneratePasswordCommand command)
        {            
            _console_IO.WriteLine($"password: {CryptoWithoutTry.GeneratePassword(command.Length)}");
        }

        private void ReEnter(ReEnterCommand command)
        {
            if (command.ReenterPassword)
            {
                    _console_IO.WriteLine("Enter password");
                    Password = _console_IO.GetHiddenInput();
                    _console_IO.WriteLine("Ok");
            }
                
        }

        private void ViewSettings(ViewSettingsCommand command)
        {
            _console_IO.WriteLine($"Path {System.Reflection.Assembly.GetEntryAssembly().Location}");
            //console_IO.WriteLine($"DirCryptFile {_appSettings.GetDirCryptFile()}");
            //console_IO.WriteLine($"DirDecryptFile {settings.GetDirDecryptFile()}");
            //console_IO.WriteLine($"caseSensitive {settings.Get_caseSensitive()}");
            //console_IO.WriteLine($"searchInTegs {settings.Get_searchInTegs()}");
            //console_IO.WriteLine($"searchInHeader {settings.Get_searchInHeader()}");
            //console_IO.WriteLine($"searchUntilFirstMatch {settings.Get_searchUntilFirstMatch()}");
            //console_IO.WriteLine($"viewServiceInformation {settings.Get_viewServiceInformation()}");
            _console_IO.WriteLine($"{nameof(_appSettings.SelectedCryptFile)}: Name = " +
                $"{_appSettings.SelectedCryptFile.Name} path = {_appSettings.SelectedCryptFile.Path}");
            _console_IO.WriteLine($"{nameof(_appSettings.SelectedDecryptFile)}: Name = " +
                $"{_appSettings.SelectedDecryptFile.Name} path = {_appSettings.SelectedDecryptFile.Path}");
            _console_IO.WriteLine("");
        }

        private void Init(InitCommand command)
        {
            CheckPassword(command.Password);
            if (command.EncryptedFile)
            {
                _console_IO.WriteLine(_inputOutputFile.InitCryptFile(Password));
                //_console_IO.WriteLine("File init successsfully");
            }
            if (command.EncryptedFiles)
            {
                _console_IO.WriteLine(_inputOutputFile.InitCryptFiles(Password));
                //_console_IO.WriteLine("Files init successsfully");
            }
            
        }

        private void Set(string[] splitCommand)
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

        private void Decrypt(DecryptCommand command)
        {
            if (command.File)
            {
                CheckPassword(command.Password);
                LoadTheDatabaseIfNeeded();
                _inputOutputFile.DecryptFile();
                _console_IO.WriteLine($"File decrypted successfully! " +
                    $"From {_appSettings.SelectedCryptFile.Path} to {_appSettings.SelectedDecryptFile.Path}\r\n");
            }
        }

        private void Encrypt(EncryptCommand command)
        {
            if (command.File)
            {
                CheckPassword(command.Password);
                _inputOutputFile.EncryptFile(Password);
                _console_IO.WriteLine($"File encrypted successfully! " +
                    $"From {_appSettings.SelectedDecryptFile.Path} to {_appSettings.SelectedCryptFile.Path}");
            }
        }


        private void Update(UpdateCommand command)
        {
            _inputOutputFile.LoadDefaultParams();
            CheckPassword(command.Password);
            LoadTheDatabaseIfNeeded();
            if (!String.IsNullOrEmpty(command.Block))
            {
                _console_IO.WriteLine("");
                GroupModel groupModel = _cryptGroup.Get(i => i.Name.ToLower()
                    .Contains(command.Group));
                if (groupModel == null)
                {
                    _console_IO.WriteLine("Group not found");
                }
                else
                {
                    //_console_IO.WriteLine($"Target group:\r\n" +
                    //    $"{groupModel.ToString()}\r\n");
                    _console_IO.Show(_mapper.Map<GroupModel, GroupDataDTO>(groupModel), false);
                    if (!QuestionAgreeOrDissagry($"Is this the right group? "))
                    {
                        return;
                    }
                    BlockModel blockModel = groupModel.CryptBlockModels.Find(i => i.Title.ToLower()
                        .Contains(command.Block));
                    if (blockModel == null)
                    {
                        _console_IO.WriteLine("Block not found");
                    }
                    else
                    {
                        _console_IO.WriteLine(groupModel.ToString());
                        _console_IO.WriteLine("");
                        _console_IO.WriteLine(blockModel.ToString());
                        UpdateBlock(ref blockModel);
                        if (QuestionAgreeOrDissagry("Save this block? "))
                        {
                            _cryptBlock.Update(blockModel);
                            _console_IO.WriteLine($"Block \"{blockModel.Title}\" updated successfully");
                        }
                    }

                }
            }
            else
            {
                _console_IO.WriteLine("");
                GroupModel groupModel = _cryptGroup.Get(i => i.Name.ToLower()
                    .Contains(command.Group));
                if (groupModel == null)
                {
                    _console_IO.WriteLine("Group not found");
                }
                else
                {
                    _console_IO.WriteLine(groupModel.ToString());
                    _console_IO.WriteLine("");
                    var temp = AddGroup();
                    groupModel.Name = temp.Name;
                    groupModel.Description = temp.Description;
                    if (QuestionAgreeOrDissagry("Save this group? "))
                    {
                        _cryptGroup.Update(groupModel);
                    }
                }

            }
        }

        private BlockModel AddBlock()
        {
            BlockModel cryptBlockModel = new BlockModel();
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

        private GroupModel AddGroup()
        {
            GroupModel cryptGroupModel = new GroupModel();
            _console_IO.WriteLine($"Fill {nameof(cryptGroupModel.Name)} and press \"Enter\"");
            cryptGroupModel.Name = _console_IO.ReadLine();
            _console_IO.WriteLine($"Fill {nameof(cryptGroupModel.Description)} and press \"Enter\"");
            cryptGroupModel.Description = _console_IO.ReadLine();
            return cryptGroupModel;
        }

        private BlockModel UpdateBlock(ref BlockModel blockModel)
        {
            if(QuestionAgreeOrDissagry($"Update field {nameof(blockModel.Title)}?"))
            {
                _console_IO.WriteLine($"Fill {nameof(blockModel.Title)} and press \"Enter\"");
                blockModel.Title = _console_IO.ReadLine();
            }
            if (QuestionAgreeOrDissagry($"Update field {nameof(blockModel.Description)}?"))
            {
                _console_IO.WriteLine($"Fill {nameof(blockModel.Description)} and press \"Enter\"");
                blockModel.Description = _console_IO.ReadLine();
            }
            if (QuestionAgreeOrDissagry($"Update field {nameof(blockModel.Email)}?"))
            {
                _console_IO.WriteLine($"Fill {nameof(blockModel.Email)} and press \"Enter\"");
                blockModel.Email = _console_IO.ReadLine();
            }
            if (QuestionAgreeOrDissagry($"Update field {nameof(blockModel.UserName)}?"))
            {
                _console_IO.WriteLine($"Fill {nameof(blockModel.UserName)} and press \"Enter\"");
                blockModel.UserName = _console_IO.ReadLine();
            }
            if (QuestionAgreeOrDissagry($"Update field {nameof(blockModel.Password)}?"))
            {
                _console_IO.WriteLine($"Fill {nameof(blockModel.Password)} and press \"Enter\"");
                blockModel.Password = _console_IO.ReadLine();
            }
            if (QuestionAgreeOrDissagry($"Update field {nameof(blockModel.Phone)}?"))
            {
                _console_IO.WriteLine($"Fill {nameof(blockModel.Phone)} and press \"Enter\"");
                blockModel.Phone = _console_IO.ReadLine();
            }
            if (QuestionAgreeOrDissagry($"Update field {nameof(blockModel.AdditionalInfo)}?"))
            {
                Console.Write($"Fill {nameof(blockModel.AdditionalInfo)} ");
                blockModel.AdditionalInfo = _console_IO.ConsoleReadMultiline();
            }
            return blockModel;
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
            var settings = new CommonForCryptPasswordLibrary.Model.EncryptDecryptSettings()
            {
                Key = Password,
                Path = _appSettings.SelectedCryptFile.Path
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

        private void CheckPassword(string passwrd = "")
        {
            Password = passwrd;
            if(String.IsNullOrEmpty(Password))
            {
                _console_IO.WriteLine("Enter password");
                Password = _console_IO.GetHiddenInput();
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
