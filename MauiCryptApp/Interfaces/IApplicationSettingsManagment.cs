using MauiCryptApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Interfaces
{
    internal interface IApplicationSettingsManagment
    {
        ApplicationSettings ApplicationSettings { get; }
        void Save();
        void Restore();
    }
}
