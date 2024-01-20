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
        public Command OnUpdateCommand { get; }

        public ItemDetailViewModel()
        {
            OnUpdateCommand = new Command(async () => await OnUpdate());
        }

        public async void LoadItemId(string itemId)//ToDo: add ScrollView
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                //Id = item.Id;
                //Text = item.Text;
                //Description = item.Description;
                Title = item.Title; 
                Description = item.Description;
                UserName = item.UserName;
                Email = item.Email;
                Password = item.Password;
                Phone = item.Phone;
                AdditionalInfo = item.AdditionalInfo;

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        async Task OnUpdate()
        {
            //Routing.RegisterRoute("", typeof(ItemsPage));
            await Shell.Current.GoToAsync("..");
        }
    }
}
