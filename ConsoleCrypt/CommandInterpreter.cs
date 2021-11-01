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
using CryptLibrariStandart.Exceptions;
using CryptLibrariStandart.AsymmetricCryptography;
using ConsoleCrypt.Contracts;
using System.Diagnostics;
using System.IO;
//using System.Void;
namespace ConsoleCrypt
{
    public class CommandInterpreter
    {
        private string password;
        private RSACryptoFile AsymmetricCryptography = new RSACryptoFile();
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

        private bool loopMode = true;

        private IMainLogicService _inputOutputFile;
        ImyIO_Console _console_IO;
        IAppSettingsConsole _appSettings;
        ISearchSettings _searchSettings;
        IMapper _mapper;
        IBlockService _cryptBlock;
        IGroupService _cryptGroup;
        public CommandInterpreter(IMainLogicService _InputOutputFile, ImyIO_Console _console_IO,
            IAppSettingsConsole appSettings, ISearchSettings searchSettings, IMapper mapper,
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
            do
            {
                Console.Write("cc> ");
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

                InterpretCommand(splitCommand);
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
            while (loopMode);
        }

        public void InterpretCommand(string[] splitCommand)
        {
            //if (String.Equals(splitCommand[0].ToLower(), "quit")
            //    || String.Equals(splitCommand[0].ToLower(), 'q'))
            //{
            //    //_appSettings.Save();
            //    //_searchSettings.Save();
            //    return false;
            //}
            try
            {
                //CommandLine.Parser.Default.ParseArguments<SearchCommand, ShowCommand>(splitCommand)
                      //.WithParsed<SearchCommand, ShowCommand>(Search, Show);
                 var result = Parser.Default.ParseArguments<SearchCommand, ShowCommand, CreateCommand, UpdateCommand, DecryptCommand,
                     EncryptCommand, InitCommand, ReEnterCommand, GeneratePasswordCommand, ViewSettingsCommand,
                     EncryptDecryptFileCommand, QuitCommand, SwitchCommand, ConfigCommand>(splitCommand);
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
                   .WithParsed<ViewSettingsCommand>(ViewSettings)
                   .WithParsed<EncryptDecryptFileCommand>(EncryptDecryptFile)
                   .WithParsed<SwitchCommand>(Switch)
                   .WithParsed<ConfigCommand>(Config)
                   .WithParsed<QuitCommand>(Quit);

                    //.MapResult(
                    //    (SearchCommand opts) => { Search(opts); return true; },
                    //    (ShowCommand opts) => { Show(opts); return true; },
                    //    (CreateCommand opts) => { Create(opts); return true; },
                    //    _ => {  UnknownCommand(); return true; }
                    //    );
            }
            catch(ValidationException ex)
            {
                _console_IO.HandleMessage("", ex);
            }
            catch(ReadFromCryptFileException ex)
            {
                _console_IO.HandleMessage("", ex);
            }
            catch (TheFileIsDamagedException ex)
            {
                _console_IO.HandleMessage("", ex);
            }
            catch (CryptoException ex)
            {
                _console_IO.HandleMessage("", ex);
            }
            catch (CryptoArgumentNullException ex)
            {
                _console_IO.HandleMessage("", ex);
            }
            catch (Exception ex)
            {
                if ((ex?.Source == "System.Diagnostics.Process") && (ex.HResult == -2147467259))
                {
                    _console_IO.HandleMessage($"{ex.Message}", ex);
                }
                else
                {
                    _console_IO.HandleMessage("Oops! An unexpected error occurred.", ex);
                }                
            }
        }

        private void Quit(QuitCommand quitCommand)
        {
            loopMode = false;
        }


        private void Config(ConfigCommand command)
        {
            if((!String.IsNullOrEmpty(command.ConfigParameter)) && (!String.IsNullOrEmpty(command.ParameterValue)))
            {
                switch(command.ConfigParameter.ToLower().Trim())
                {
                    case "editor":
                        _appSettings.Editor = command.ParameterValue.Replace(@"\*", " ");
                        _appSettings.Save();
                        break;
                    default:
                        _console_IO.WriteLine("Parameter not found");
                        break;
                }
            }
            else if(command.OpenInEditor)
            {
                //using (Process process = new Process())
                //{
                //    process.StartInfo.FileName = _appSettings.Editor;
                //    process.StartInfo.Arguments = _appSettings.PathToSettings;
                //    process.Start();
                //    process.WaitForExit();
                //}
                Process process = null;
                try
                {
                    process = Process.Start(_appSettings.Editor, _appSettings.PathToSettings);
                    process.WaitForExit();
                }                
                finally
                {
                    process?.Dispose();
                }
            }
        }

        /// <summary>
        /// use save appsetting
        /// </summary>
        private void Switch(SwitchCommand command)
        {
            if(command.EncryptPasswordFile)
            {
                SwitchEncryptDecryptFile(_appSettings.DirCryptFile, i=> _appSettings.selected_crypr_file = i, 
                    _appSettings.selected_crypr_file, _appSettings.SelectedCryptFile, command.Name, command.Password);
            }
            if (command.DecryptPasswordFile)
            {
                SwitchEncryptDecryptFile(_appSettings.DirDecryptFile, i => _appSettings.selected_decrypr_file = i,
                    _appSettings.selected_decrypr_file, _appSettings.SelectedDecryptFile, command.Name, command.Password);
            }
        }

        private void SwitchEncryptDecryptFile(List<FileModelInSettings> _files, Action<string> Set_selected, string _selected,
            FileModelInSettings selectedFile, string _searchName, string _password)
        {
            List<FileModelInSettings> files = _appSettings.DirCryptFile.Where(i => i.Name.ToLower().Contains(_searchName.ToLower()))
                .Except(new[] { selectedFile }).ToList();
            if (files.Count == 0)
            {
                _console_IO.WriteLine($"File with name \"{_searchName}\" does not exist in appSettings.DirCryptFile");
            }
            else
            {

                for (int i = 0; i < files.Count; i++)
                {
                    _console_IO.WriteLine(files[i].ToString());

                    if (this.QuestionAgreeOrDissagry("Selected this file?"))
                    {
                        FileModelInSettings file = files[i];
                        Set_selected.Invoke(file.Name);
                        this.password = null;
                        CheckPassword(_password);
                        ReloadTheDatabase();
                        if (QuestionAgreeOrDissagry("Save this file as default?\r\n" +
                            "You will be able to work with the selected file anyway "))
                        {
                            _appSettings.Save();
                        }
                        else
                        {
                            Set_selected.Invoke(_selected);
                        }
                        break;
                    }
                }


            }
        }
        private void EncryptDecryptFile(EncryptDecryptFileCommand command)
        {
            if(command.CreateKey)
            {
                //if(String.IsNullOrEmpty(command.AsymmetricKey))
                //{
                //    _console_IO.WriteLine($"{nameof(command.AsymmetricKey)} is null or empty");
                //    return;
                //}
                _console_IO.WriteLine(AsymmetricCryptography.CreateAsmKeys(command.AsymmetricKey));
            }
            if (command.ImporPublicKey)
            {
                _console_IO.WriteLine(AsymmetricCryptography.ImportPublicKey(command.PathFrom));
            }
            if (command.ExportPublicKey)
            {
                AsymmetricCryptography.ExportPublicKey(command.PathTo);
                _console_IO.WriteLine("Key successfully exported");
            }
            if (command.Encrypt)
            {
                _console_IO.WriteLine(AsymmetricCryptography.CreateAsmKeys(command.AsymmetricKey));
                _console_IO.WriteLine(AsymmetricCryptography.EncryptFile(command.PathFrom, command.PathTo));
            }
            if (command.Decrypt)
            {
                _console_IO.WriteLine(AsymmetricCryptography.CreateAsmKeys(command.AsymmetricKey));
                _console_IO.WriteLine(AsymmetricCryptography.DecryptFile(command.PathFrom, command.PathTo));
            }  
            
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
                    cryptBlockModel.DateTimeCreate = DateTime.UtcNow;
                    cryptBlockModel.DateTimeUpdate = DateTime.UtcNow;
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
                    cryptGroupModel.DateTimeCreate = DateTime.UtcNow;
                    cryptGroupModel.DateTimeUpdate = DateTime.UtcNow;
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
            _console_IO.WriteLine($"{nameof(_appSettings.Editor)} = {_appSettings.Editor}" );
            _console_IO.WriteLine($"{nameof(_appSettings.PathToSettings)} = {_appSettings.PathToSettings}" );
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
                            blockModel.DateTimeUpdate = DateTime.UtcNow;
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
                    if (QuestionAgreeOrDissagry("Save this group? "))
                    {
                        groupModel.Name = temp.Name;
                        groupModel.Description = temp.Description;
                        groupModel.DateTimeUpdate = DateTime.UtcNow;
                        _cryptGroup.Update(groupModel);
                        _console_IO.WriteLine($"Group \"{groupModel.Name}\" updated successfully");
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
            //if (!_cryptGroup.DataExist)
            //{
            //    _cryptGroup.LoadData(settings);
            //}
        }

        private void ReloadTheDatabase()
        {
            var settings = new CommonForCryptPasswordLibrary.Model.EncryptDecryptSettings()
            {
                Key = Password,
                Path = _appSettings.SelectedCryptFile.Path
            };
            _cryptBlock.LoadData(settings);
            //if (!_cryptGroup.DataExist)
            //{
            //    _cryptGroup.LoadData(settings);
            //}
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
