using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using MauiCryptApp.Services;
using MauiCryptApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        //private string text;
        //private string description;
        //public string Id { get; set; }

        //public string Text
        //{
        //    get => text;
        //    set => SetProperty(ref text, value);
        //}

        //public string Description
        //{
        //    get => description;
        //    set => SetProperty(ref description, value);
        //}
        public Item _item { get; private set; } = new Item();
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }
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

        public delegate Task DisplayAlertHandler(string title, string body, string cancel);
        public event DisplayAlertHandler DisplayAlert;
        public delegate Task<bool> DisplayAlertHandler_(string title, string body, string ok, string cancel);
        public event DisplayAlertHandler_ DisplayAlert_;
        public Command OnUpdateCommand { get; }
        public Command CopyPasswordToClipboardCommand { get; }
        public Command CopyEmailToClipboardCommand { get; }
        private readonly IBackuperWrapperService _backuperWrapperService;
        public ItemDetailViewModel()
        {
            OnUpdateCommand = new Command(async () => await UpdateItem());
            CopyEmailToClipboardCommand = new Command(async () => await CopyPassword());
            CopyEmailToClipboardCommand = new Command(async () => await CopyEmail());
            _backuperWrapperService = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IBackuperWrapperService>();
        }

        public async void LoadItemId(string itemId)//ToDo: add ScrollView
        {
            try
            {
                _item = await BlockDataStore.GetItemAsync(itemId);
                MapModelToFields();
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }


        public void MapModelToFields()
        {
            Description = _item.Description;
            Title = _item.Title;
            Description = _item.Description;
            UserName = _item.UserName;
            Email = _item.Email;
            Password = _item.Password;
            Phone = _item.Phone;
            AdditionalInfo = _item.AdditionalInfo;
        }
        public void MapFieldsToModel()
        {
            _item.Title = Title;
            _item.Description = Description;
            _item.UserName = UserName;
            _item.Email = Email;
            _item.Password = Password;
            _item.Phone = Phone;
            _item.AdditionalInfo = string.Join(" \r\n ", AdditionalInfo.Split('\r').Select(x=>x.Trim()));
        }

        public async Task UpdateItem()
        {
            await _backuperWrapperService.MakeBackupBeforeUpdate();
            var result = await DisplayAlert_.Invoke("Info", "Logs create backup before update: "+Environment.NewLine+_backuperWrapperService.PrettyLogs, "ok", "cancel");
            if (result)//do updating of instance
            {
                MapFieldsToModel();
                await BlockDataStore.UpdateItemAsync(_item);
                _backuperWrapperService.CleanLogs();
                await _backuperWrapperService.Synchronize_Upload();
                await DisplayAlert.Invoke("Info", "Synchronization logs:"+Environment.NewLine+_backuperWrapperService.PrettyLogs, "ok");
            }
            
        }

        //async Task OnUpdate()
        //{
        //    //Routing.RegisterRoute("", typeof(ItemsPage));
        //    await Shell.Current.GoToAsync("..");
        //}

        async Task CopyPassword()
        {
            await Clipboard.SetTextAsync(password);
            await DisplayAlert.Invoke("Info", "Password copied to clipboard", "Ok");
        }

        async Task CopyEmail()
        {
            await Clipboard.SetTextAsync(email);
            await DisplayAlert.Invoke("Info", "Email copied to clipboard", "Ok");
        }
    }
}
