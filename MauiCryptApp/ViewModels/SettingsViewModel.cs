using AuxiliaryLib.Extensions;
using Backuper_Core.Configurations;
using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using Newtonsoft.Json;
using System.ComponentModel;

namespace MauiCryptApp.ViewModels
{
    public class SettingsViewModel:BaseViewModel, INotifyPropertyChanged
    {
        private ApplicationSettings _settings;
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

        private string appSettingsEditor;
        public string AppSettingsEditor
        {
            get { return appSettingsEditor;}
            set { SetProperty(ref appSettingsEditor, value);}
        }
        private string backuperSettingsEditor;
        public string BackuperSettingsEditor
        {
            get { return backuperSettingsEditor; }
            set { SetProperty(ref backuperSettingsEditor, value); }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

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
        public List<string> AvailableEncryptedFiles { get; set; }
        public Command ResetSettingsCommand { get; }
        public Command SaveSettingsCommand { get; }

        

        public SettingsViewModel()
        {
            
            _settings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<ApplicationSettings>();
            ResetSettingsCommand = new Command(ResetSettings);
            SaveSettingsCommand = new Command(SaveSettings);
            AvailableEncryptedFiles = _settings.AppSettings.DirCryptFile.Select(x=>StringConverterForPicker.FileModelInSettingToString(x.Name,x.Path)).ToList();
            SelectedEncryptedFile = StringConverterForPicker.FileModelInSettingToString(_settings.AppSettings.SelectedCryptFile.Name, _settings.AppSettings.SelectedCryptFile.Path);
            ResetSettings();
        }

        private void MapSearchSettingsIntoFields()
        {
            CaseSensitive = _settings.SearchSettings.CaseSensitive;
            SearchInTegs = _settings.SearchSettings.SearchInTegs;
            SearchInHeader = _settings.SearchSettings.SearchInHeader;
            SearchUntilFirstMatch = _settings.SearchSettings.SearchUntilFirstMatch;
            ViewServiceInformation = _settings.SearchSettings.ViewServiceInformation;
            SearchEverywhere = _settings.SearchSettings.SearchEverywhere;
        }

        private void MapAppSettingsIntoEditor()
        {
            AppSettingsEditor = _settings.AppSettings.Serialize();
        }

        private void MapBackupSettingsIntoEditor()
        {
            BackuperSettingsEditor = _settings.BackupSettings.Serialize();
        }

        private void MapEditorIntoBackupSettins()
        {
            if (!string.IsNullOrEmpty(backuperSettingsEditor))
            { _settings.BackupSettings = JsonConvert.DeserializeObject<CBackupSettings>(backuperSettingsEditor); }
        }

        private void MapEditorIntoAppSettings()
        {
            //_settings.AppSettings = null;
            //var temp = JsonConvert.DeserializeObject<MauiCryptApp.Helpers.AppSettings>(appSettingsEditor);
            if (!string.IsNullOrEmpty(appSettingsEditor))
            {
                _settings.AppSettings = JsonConvert.DeserializeObject<MauiCryptApp.Helpers.AppSettings>(appSettingsEditor);
            }
            
        }

        private void MapFieldsIntoSearchSettings()
        {
            _settings.SearchSettings.CaseSensitive = caseSensitive;
            _settings.SearchSettings.SearchInTegs = searchInTegs;
            _settings.SearchSettings.SearchInHeader = searchInHeader;
            _settings.SearchSettings.SearchUntilFirstMatch = searchUntilFirstMatch;
            _settings.SearchSettings.ViewServiceInformation = viewServiceInformation;
            _settings.SearchSettings.SearchEverywhere = searchEverywhere;
        }


        private void SaveSettings()
        {
            MapFieldsIntoSearchSettings();
            MapEditorIntoAppSettings();
            MapEditorIntoBackupSettins();
            Preferences.Default.Set(MauiProgram.applicationSettingsPreferencesKey, _settings.Serialize());
        }

        private void ResetSettings()
        {
            MapSearchSettingsIntoFields();
            MapAppSettingsIntoEditor();
            MapBackupSettingsIntoEditor();
        }

        private void HandleUpdateCurrentCryptedFile()
        {
            MapEditorIntoAppSettings();
            _settings.AppSettings.selected_crypr_file = StringConverterForPicker.GetName(SelectedEncryptedFile);
            MapAppSettingsIntoEditor();
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
