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

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                bool isValid = double.TryParse(e.NewTextValue, out double _);
                ((Entry)sender).Text = isValid ? e.NewTextValue : e.OldTextValue;
            }
        }

    }
}
