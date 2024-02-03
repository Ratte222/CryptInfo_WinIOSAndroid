using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.ViewModels
{
    internal class AddItemViewModel:BaseViewModel
    {
        Item _item = new Item();
        private string title;
        public string Title { get { return title; } set { SetProperty(ref title, value); } }
        private string description;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        private string phone;
        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }

        private string additionalInfo;
        public string AdditionalInfo
        {
            get { return additionalInfo; }
            set { SetProperty(ref additionalInfo, value); }
        }

        private string _selectedGroup;
        public string SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (_selectedGroup != value)
                {
                    _selectedGroup = value;
                    OnPropertyChanged(nameof(SelectedGroup));
                }
            }
        }
        public List<string> AvailableGroups { get; set; }
        private Group[] groups;

        public Command SaveNewItemCommand { get; }
        public delegate Task DisplayAlertHandler(string title, string body);
        public event DisplayAlertHandler DisplayAlert; 
        public AddItemViewModel()
        {
            SaveNewItemCommand = new Command(async () => { await SaveNewItem(); });
            groups = GroupDataStore.GetItemsAsync().GetAwaiter().GetResult().ToArray();
            AvailableGroups = groups.Select(x => x.Name).ToList();
        }

        public void MapFieldsToModel()
        {
            _item.Title = Title;
            _item.Description = Description;
            _item.UserName = UserName;
            _item.Email = Email;
            _item.Password = Password;
            _item.Phone = Phone;
            _item.AdditionalInfo = AdditionalInfo;
        }

        public async Task SaveNewItem()
        {
            if(string.IsNullOrWhiteSpace(SelectedGroup))
            {
                await DisplayAlert.Invoke("Info", "Select group");
                return;
            }
            MapFieldsToModel();
            _item.GroupId = groups.Single(x => x.Name == SelectedGroup).Id;
            await BlockDataStore.AddItemAsync(_item);
            await DisplayAlert.Invoke("Info", "Item added successfully");
            await Shell.Current.GoToAsync("..");
        }

        
    }
}
