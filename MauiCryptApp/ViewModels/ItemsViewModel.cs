using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using MauiCryptApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.ViewModels
{
    [QueryProperty(nameof(Password), nameof(Password))]
    public class ItemsViewModel:BaseViewModel
    {
        
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                ExecuteSearchItemsCommand().GetAwaiter();
            }
        }

        private string _searchText = "";
        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                LoadItemsCommand.Execute(this);
            }
        }
        
        private bool _orderByLstModifyTime = false;
        public bool OrderByLstModifyTime
        {
            get { return _orderByLstModifyTime; }
            set { 
                _orderByLstModifyTime = value;
                ExecuteSearchItemsCommand().GetAwaiter();
            }
        }

        private bool _showDiagnosticInformation = false;
        public bool ShowDiagnosticInformation
        {
            get { return _showDiagnosticInformation; }
            set { 
                _showDiagnosticInformation = value;
            }
        }

        private string _password;

        private Item _selectedItem;

        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }
        public Command SearchCommand { get; }

        public delegate Task DisplayAlertHandler(string title, string body, string cancel);
        public event DisplayAlertHandler DisplayAlert;

        private readonly ApplicationSettings _applicationSettings;
        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SearchCommand = new Command(async () => await ExecuteSearchItemsCommand());
            ItemTapped = new Command<Item>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            
            _applicationSettings = MauiProgram.ServiceScope.ServiceProvider.GetService<IApplicationSettingsManagment>().ApplicationSettings;
        }

        async Task ExecuteSearchItemsCommand()
        {
            IsBusy = true;

            try
            {
                
                //if ((await BlockDataStore.GetItemsAsync()).Count() > 0)
                //{
                Items.Clear();
                IEnumerable<Item> items = null;
                var filter = new SearchFilter() { OrderByLastModifyDate = OrderByLstModifyTime };
                if (_applicationSettings.LimitNumbersOfItemsInSearchResult)
                {
                    items = (await BlockDataStore.Search(_searchText, filter));
                    if (OrderByLstModifyTime)
                    {
                        items = items.OrderByDescending(x => x.LastModifiedAt);
                    }
                    items = items.Take(_applicationSettings.NumberOfItemsInSearchResult);
                }
                else
                {
                    items = (await BlockDataStore.Search(_searchText, filter));
                    if (OrderByLstModifyTime)
                    {
                        items = items.OrderByDescending(x => x.LastModifiedAt);
                    }
                }
                
                foreach (var item in items)
                {
                    Items.Add(item);
                }
                
                //}

            }
            catch (Exception ex)
            {
                await DisplayAlert.Invoke("Error", $"The error occurred while searching command was executing. \r\nMessage: {ex.Message}", "ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                
                if(BlockDataStore.SetKey(_password) && Items.Count == 0)
                {
                    Items.Clear();
                    IEnumerable<Item> items = null;
                    if (_applicationSettings.LimitNumbersOfItemsInSearchResult)
                    {
                        items = (await BlockDataStore.GetItemsAsync()).Take(_applicationSettings.NumberOfItemsInSearchResult);
                    }
                    else
                    {
                        items = (await BlockDataStore.Search(_searchText, new SearchFilter() { OrderByLastModifyDate = OrderByLstModifyTime}));
                    }
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                }
                GroupDataStore.SetKey(_password);
            }
            catch (Exception ex)
            {
                await DisplayAlert.Invoke("Error", $"The error occurred while the loadItems command was executing. \r\nMessage: {ex.Message}", "ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                //SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AddItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            https://docs.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation
            //string route = $"/{nameof(ItemDetailPage)}";
            //Routing.RegisterRoute(route, typeof(ItemDetailPage));
            await Shell.Current.GoToAsync($"/{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

        
    }
}
