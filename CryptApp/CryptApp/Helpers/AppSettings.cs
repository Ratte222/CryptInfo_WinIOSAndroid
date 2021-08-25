using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(CryptApp.Helpers.AppSettings))]
namespace CryptApp.Helpers
{
    public class AppSettings
    {
        public string Password { get; set; }
        public bool TestMode { get; set; }
    }
}
