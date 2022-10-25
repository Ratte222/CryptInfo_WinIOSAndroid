using Backuper_Core.Configurations;
using Backuper_Core.Services;
using CommonForCryptPasswordLibrary.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiCryptApp.Models;
using MauiCryptApp.ViewModels;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MauiCryptApp.Views;

public partial class SynchronizePage : ContentPage
{
    
    SynchronizeViewModel synchronizeViewModel { get; set; }
    public SynchronizePage()
	{
		InitializeComponent();
        BindingContext = synchronizeViewModel = new SynchronizeViewModel();
    }

}


