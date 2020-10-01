using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using CryptWinIOSAndroid.Models;
using CryptWinIOSAndroid.Views;

namespace CryptWinIOSAndroid.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private BlockOfInformation _selectedBlockOfInformation;

        public ObservableCollection<BlockOfInformation> blockOfInformation { get; }
        public Command LoadBlockOfInformationCommand { get; }
        public Command AddBlockOfInformationCommand { get; }
        public Command<BlockOfInformation> BlockOfInformationTapped { get; }

        public ItemsViewModel()
        {
            Title = "Browse";
            blockOfInformation = new ObservableCollection<BlockOfInformation>();
            LoadBlockOfInformationCommand = new Command(async () => await ExecuteLoadItemsCommand());

            BlockOfInformationTapped = new Command<BlockOfInformation>(OnItemSelected);

            AddBlockOfInformationCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                blockOfInformation.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    blockOfInformation.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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

        public BlockOfInformation SelectedItem
        {
            get => _selectedBlockOfInformation;
            set
            {
                SetProperty(ref _selectedBlockOfInformation, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(BlockOfInformation item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}