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
        private Item _item = new Item();
        public string Title { get { return _item.Title; } set { SetProperty(_item.Title, value); } }
        public string Description
        {
            get { return _item.Description; }
            set { SetProperty(_item.Description, value); }
        }

        public string UserName
        {
            get { return _item.UserName; }
            set { SetProperty(_item.UserName, value); }
        }

        public string Email
        {
            get { return _item.Email; }
            set { SetProperty(_item.Email, value); }
        }

        public string Password
        {
            get { return _item.Password; }
            set { SetProperty(_item.Password, value); }
        }

        public string Phone
        {
            get { return _item.Phone; }
            set { SetProperty(_item.Phone, value); }
        }

        public string AdditionalInfo
        {
            get { return _item.AdditionalInfo; }
            set { SetProperty(_item.AdditionalInfo, value); }
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
                _item = await DataStore.GetItemAsync(itemId);
                //Id = item.Id;
                //Text = item.Text;
                //Description = item.Description;
                //Title = item.Title; 
                //Description = item.Description;
                //UserName = item.UserName;
                //Email = item.Email;
                //Password = item.Password;
                //Phone = item.Phone;
                //AdditionalInfo = item.AdditionalInfo;

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
