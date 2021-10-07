using CryptApp.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CryptApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string itemTitle;
        private string description;
        private string username;
        private string email;
        private string password;
        private string phone;
        private string additionalInfo;
        public string Id { get; set; }

        public string ItemTitle
        {
            get => itemTitle;
            set => SetProperty(ref itemTitle, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string UserName
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        public string AdditionalInfo
        {
            get => additionalInfo;
            set => SetProperty(ref additionalInfo, value);
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

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                ItemTitle = item.Title;
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
    }
}
