using CommonForCryptPasswordLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.Contracts
{
    public interface IAppSettingsConsole:IAppSettings
    {
        string PathToSettings { get; }
        string Editor { get; set; }
    }
}
