using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
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
        //public Command OnUpdateCommand { get; }
        //public Command CopyPasswordToClipboard { get; }
        //public Command CopyEmailToClipboard { get; }
        public ItemDetailViewModel()
        {
            //OnUpdateCommand = new Command(async () => await OnUpdate());
            //CopyEmailToClipboard = new Command(async () => await CopyPassword());
            //CopyEmailToClipboard = new Command(async () => await CopyEmail());
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
            _item.AdditionalInfo = AdditionalInfo;
        }

        public async Task UpdateItem()
        {
            MapFieldsToModel();
            await BlockDataStore.UpdateItemAsync(_item);
        }

        //async Task OnUpdate()
        //{
        //    //Routing.RegisterRoute("", typeof(ItemsPage));
        //    await Shell.Current.GoToAsync("..");
        //}

        //async Task CopyPassword()
        //{
        //    await Clipboard.SetTextAsync(password);

        //}

        //async Task CopyEmail()
        //{
        //    await Clipboard.SetTextAsync(email);
        //}
    }
}
