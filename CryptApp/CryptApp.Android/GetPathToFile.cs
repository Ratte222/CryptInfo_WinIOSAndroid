using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(CryptApp.Droid.GetPathToFile))]
namespace CryptApp.Droid
{
    public class GetPathToFile : IGetPathToFile
    {
        public string GetPathToCryptFile()
        {
            bool isReadonly = Android.OS.Environment.MediaMountedReadOnly.Equals(Android.OS.Environment.ExternalStorageState);
            bool isWriteable = Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState);
            bool _isWriteable = Android.OS.Environment.DirectoryDocuments.Equals(Android.OS.Environment.ExternalStorageState);
            //if (Android.OS.Environment.MediaMountedReadOnly.Equals(Android.OS.Environment.ExternalStorageState))
            string path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).Path;
            string pathCa = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).CanonicalPath;
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;
            //else return null;

            
        }
    }
}