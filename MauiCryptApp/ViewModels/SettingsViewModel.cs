using AuxiliaryLib.Extensions;
using Backuper_Core.Configurations;
using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;

namespace MauiCryptApp.ViewModels
{
    public class SettingsViewModel:BaseViewModel, INotifyPropertyChanged
    {
        private IApplicationSettingsManagment _settingsManagment;
        #region applicationSettings
        private bool limitNumbersOfItemsInSearchResult;
        public bool LimitNumbersOfItemsInSearchResult
        {
            get { return limitNumbersOfItemsInSearchResult; }
            set { SetProperty(ref limitNumbersOfItemsInSearchResult, value); }
        }

        private int numberOfItemsInSearchResult;
        public int NumberOfItemsInSearchResult
        {
            get { return numberOfItemsInSearchResult; }
            set { SetProperty(ref numberOfItemsInSearchResult, value); }
        }

        private bool syncBeforeDecryptFile = true;
        public bool SyncBeforeDecryptFile
        {
            get { return syncBeforeDecryptFile; }
            set { SetProperty(ref syncBeforeDecryptFile, value); }
        }

        private bool syncAfterUpdateCreateItem = true;
        public bool SyncAfterUpdateCreateItem
        {
            get { return syncAfterUpdateCreateItem; }
            set { SetProperty(ref syncAfterUpdateCreateItem, value); }
        }

        private bool createBackupBeforeUpdateOrCreateItem = true;
        public bool CreateBackupBeforeUpdateOrCreateItem
        {
            get { return createBackupBeforeUpdateOrCreateItem; }
            set { SetProperty(ref createBackupBeforeUpdateOrCreateItem, value); }
        }
        #endregion
        #region SearchSettings

        private bool caseSensitive;
        public bool CaseSensitive
        {
            get { return caseSensitive; }
            set { SetProperty(ref caseSensitive, value); }
        }

        private bool searchInTegs;
        public bool SearchInTegs
        {
            get { return searchInTegs; }
            set { SetProperty(ref searchInTegs, value); }
        }

        private bool searchInHeader;
        public bool SearchInHeader
        {
            get { return searchInHeader; }
            set { SetProperty(ref searchInHeader, value); }
        }

        private bool searchUntilFirstMatch;
        public bool SearchUntilFirstMatch
        {
            get { return searchUntilFirstMatch; }
            set { SetProperty(ref searchUntilFirstMatch, value); }
        }

        private bool viewServiceInformation;
        public bool ViewServiceInformation
        {
            get { return viewServiceInformation; }
            set { SetProperty(ref viewServiceInformation, value); }
        }

        private bool searchEverywhere;
        public bool SearchEverywhere
        {
            get { return searchEverywhere; }
            set { SetProperty(ref searchEverywhere, value); }
        }
        #endregion
        #region appSettings
        private string appSettingsEditor;
        public string AppSettingsEditor
        {
            get { return appSettingsEditor;}
            set { SetProperty(ref appSettingsEditor, value);}
        }

        private string _selectedEncryptFile;
        public string SelectedEncryptedFile
        {
            get { return _selectedEncryptFile; }
            set
            {
                if (_selectedEncryptFile != value)
                {
                    _selectedEncryptFile = value;
                    OnPropertyChanged(nameof(SelectedEncryptedFile));
                    // Handle the update here
                    HandleUpdateCurrentCryptedFile();
                }
            }
        }
        private List<string> availableEncryptedFiles;
        public List<string> AvailableEncryptedFiles
        {
            get { return availableEncryptedFiles; }
            set { SetProperty(ref availableEncryptedFiles, value); }
        }
        #endregion
        #region BackuperSettings
        private string backuperSettingsEditor;
        public string BackuperSettingsEditor
        {
            get { return backuperSettingsEditor; }
            set { SetProperty(ref backuperSettingsEditor, value); }
        }

        private string _selectedBackuperProfile;
        public string SelectedBackuperProfile
        {
            get { return _selectedBackuperProfile; }
            set
            {
                if (_selectedBackuperProfile != value)
                {
                    _selectedBackuperProfile = value;
                    OnPropertyChanged(nameof(SelectedBackuperProfile));
                    // Handle the update here
                    //HandleUpdateCurrentCryptedFile();
                }
            }
        }
        private List<string> availableBacuperProfiles;
        public List<string> AvailableBacuperProfiles
        {
            get { return availableBacuperProfiles; }
            set { SetProperty(ref availableBacuperProfiles, value); }
        }

        #endregion
        //public event PropertyChangedEventHandler PropertyChanged;


        public delegate Task DisplayAlertHandler(string title, string body, string cancel);
        public event DisplayAlertHandler DisplayAlert;
        public Command ResetSettingsCommand { get; }
        public Command SaveSettingsCommand { get; }
        public Command ResetFileInfosCommand { get; }
        public Command ExecuteBackuperProfileCommand { get; }

        private FileInfos _fileInfos;
        private readonly IBackuperWrapperService _backuperWrapperService;
        public SettingsViewModel()
        {
            _backuperWrapperService = MauiProgram.ServiceScope.ServiceProvider.GetService<IBackuperWrapperService>();
            _settingsManagment = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IApplicationSettingsManagment>();
            _fileInfos = MauiProgram.ServiceScope.ServiceProvider.GetService<FileInfos>();
            ResetSettingsCommand = new Command(ResetSettings);
            SaveSettingsCommand = new Command(SaveSettings);
            ResetFileInfosCommand = new Command(ResetFileInfos);
            ExecuteBackuperProfileCommand = new Command(async () => await ExecuteBackuperProfile());
            SelectedEncryptedFile = StringConverterForPicker.FileModelInSettingToString(_settingsManagment.ApplicationSettings.AppSettings.SelectedCryptFile.Name, _settingsManagment.ApplicationSettings.AppSettings.SelectedCryptFile.Path);
            ResetSettings();
        }

        private void MapSearchSettingsIntoFields()
        {
            CaseSensitive = _settingsManagment.ApplicationSettings.SearchSettings.CaseSensitive;
            SearchInTegs = _settingsManagment.ApplicationSettings.SearchSettings.SearchInTegs;
            SearchInHeader = _settingsManagment.ApplicationSettings.SearchSettings.SearchInHeader;
            SearchUntilFirstMatch = _settingsManagment.ApplicationSettings.SearchSettings.SearchUntilFirstMatch;
            ViewServiceInformation = _settingsManagment.ApplicationSettings.SearchSettings.ViewServiceInformation;
            SearchEverywhere = _settingsManagment.ApplicationSettings.SearchSettings.SearchEverywhere;
        }
        private void MapApplicationSettingsIntoFields()
        {
            LimitNumbersOfItemsInSearchResult = _settingsManagment.ApplicationSettings.LimitNumbersOfItemsInSearchResult;
            NumberOfItemsInSearchResult = _settingsManagment.ApplicationSettings.NumberOfItemsInSearchResult;
            SyncBeforeDecryptFile = _settingsManagment.ApplicationSettings.SyncBeforeDecryptFile;
            SyncAfterUpdateCreateItem = _settingsManagment.ApplicationSettings.SyncAfterUpdateCreateItem;
            CreateBackupBeforeUpdateOrCreateItem = _settingsManagment.ApplicationSettings.CreateBackupBeforeUpdateOrCreateItem;
        }
        private void MapAppSettingsIntoEditor()
        {
            availableEncryptedFiles = _settingsManagment.ApplicationSettings.AppSettings.DirCryptFile.Select(x => StringConverterForPicker.FileModelInSettingToString(x.Name, x.Path)).ToList();
            AppSettingsEditor = _settingsManagment.ApplicationSettings.AppSettings.Serialize();
        }

        private void MapBackupSettingsIntoEditor()
        {
            AvailableBacuperProfiles = _settingsManagment.ApplicationSettings.BackupSettings.BackupSettings.Select(x=>x.Name).ToList();
            BackuperSettingsEditor = _settingsManagment.ApplicationSettings.BackupSettings.Serialize();
        }

        private void MapEditorIntoBackupSettins()
        {
            if (!string.IsNullOrEmpty(backuperSettingsEditor))
            { _settingsManagment.ApplicationSettings.BackupSettings = JsonConvert.DeserializeObject<CBackupSettings>(backuperSettingsEditor); }
        }
        private void MapFieldsIntoApplicationSettings()
        {
            _settingsManagment.ApplicationSettings.LimitNumbersOfItemsInSearchResult = LimitNumbersOfItemsInSearchResult;
            _settingsManagment.ApplicationSettings.NumberOfItemsInSearchResult = NumberOfItemsInSearchResult;
            _settingsManagment.ApplicationSettings.SyncBeforeDecryptFile = SyncBeforeDecryptFile;
            _settingsManagment.ApplicationSettings.SyncAfterUpdateCreateItem = SyncAfterUpdateCreateItem;
            _settingsManagment.ApplicationSettings.CreateBackupBeforeUpdateOrCreateItem = CreateBackupBeforeUpdateOrCreateItem;
        }
        private void MapEditorIntoAppSettings()
        {
            //_settings.AppSettings = null;
            //var temp = JsonConvert.DeserializeObject<MauiCryptApp.Helpers.AppSettings>(appSettingsEditor);
            if (!string.IsNullOrEmpty(appSettingsEditor))
            {
                availableEncryptedFiles = _settingsManagment.ApplicationSettings.AppSettings.DirCryptFile.Select(x => StringConverterForPicker.FileModelInSettingToString(x.Name, x.Path)).ToList();
                _settingsManagment.ApplicationSettings.AppSettings = JsonConvert.DeserializeObject<MauiCryptApp.Helpers.AppSettings>(appSettingsEditor);
            }
            
        }

        private void MapFieldsIntoSearchSettings()
        {
            _settingsManagment.ApplicationSettings.SearchSettings.CaseSensitive = caseSensitive;
            _settingsManagment.ApplicationSettings.SearchSettings.SearchInTegs = searchInTegs;
            _settingsManagment.ApplicationSettings.SearchSettings.SearchInHeader = searchInHeader;
            _settingsManagment.ApplicationSettings.SearchSettings.SearchUntilFirstMatch = searchUntilFirstMatch;
            _settingsManagment.ApplicationSettings.SearchSettings.ViewServiceInformation = viewServiceInformation;
            _settingsManagment.ApplicationSettings.SearchSettings.SearchEverywhere = searchEverywhere;
        }


        private void SaveSettings()
        {
            MapFieldsIntoApplicationSettings();
            MapFieldsIntoSearchSettings();
            MapEditorIntoAppSettings();
            MapEditorIntoBackupSettins();
            _settingsManagment.Save();
            //Preferences.Default.Set(MauiProgram.APPLICATION_SETTINGS_PREFERENCES_KEY, _settingsManagment.ApplicationSettings.Serialize());
            DisplayAlert.Invoke("Info", "Settings saved", "Ok");
        }

        private void ResetSettings()
        {
            MapApplicationSettingsIntoFields();
            MapSearchSettingsIntoFields();
            MapAppSettingsIntoEditor();
            MapBackupSettingsIntoEditor();
        }

        private void HandleUpdateCurrentCryptedFile()
        {
            MapEditorIntoAppSettings();
            _settingsManagment.ApplicationSettings.AppSettings.selected_crypr_file = StringConverterForPicker.GetName(SelectedEncryptedFile);
            MapAppSettingsIntoEditor();
        }

        private void ResetFileInfos()
        {
            _fileInfos.FileInfosData = new List<Backuper_Core.Configurations.FileInfo>();
            _fileInfos.Save();//need for correct work synchronize
            DisplayAlert.Invoke("Info", "FileInfos reset successfully", "Ok");
        }

        private async Task ExecuteBackuperProfile()
        {
            if (string.IsNullOrEmpty(SelectedBackuperProfile))
            {
                await DisplayAlert.Invoke("Info", "Please, select backuper profile", "ok");
                return;
            }
            await _backuperWrapperService.MakeBackup(SelectedBackuperProfile);
            await DisplayAlert.Invoke("Info", $"Backuper profile ({SelectedBackuperProfile}) successfully executed. Logs:{_backuperWrapperService.PrettyLogs}", "Ok");
        }

        static class StringConverterForPicker
        {
            const string SEPARATOR = ";(";
            public static string FileModelInSettingToString(string name, string path)
            {
                return $"{name}{SEPARATOR}{path})";
            }
            
            public static string GetName(string value)
            {
                return value.Split(SEPARATOR).First();
            }
        }
    }
}
