using System.ComponentModel;
using Xamarin.Forms;
using CryptWinIOSAndroid.ViewModels;

namespace CryptWinIOSAndroid.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}