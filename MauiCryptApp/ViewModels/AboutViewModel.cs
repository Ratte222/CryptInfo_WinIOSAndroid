using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            PackageName = AppInfo.Current.PackageName; 
            Author = "Ratte222"; 
        }

        public string AssemblyVersion { get; }
        public string PackageName { get; }
        public string Author { get; }
    }
}
