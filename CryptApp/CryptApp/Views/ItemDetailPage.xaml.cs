using CryptApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace CryptApp.Views
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