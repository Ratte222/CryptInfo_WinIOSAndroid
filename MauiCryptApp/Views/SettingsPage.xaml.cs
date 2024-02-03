using MauiCryptApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Views
{
    public partial class SettingsPage:ContentPage
    {
        SettingsViewModel viewModel;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SettingsViewModel();
            viewModel.DisplayAlert += DisplayAlert;
        }
    }
}
