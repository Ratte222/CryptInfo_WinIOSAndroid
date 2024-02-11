using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonForCryptPasswordLibrary.Helpers
{
    public static class BuilderDIHelperExtensions
    {
        public static void RegisterCryptographyPasswordLibrary(this IServiceCollection services, RegisterCryptographyPasswordLibraryOptions options)
        {
            services.AddSingleton<IEncryptDecryptService, EncryptDecryptService>();
            services.AddScoped<ICryptService, CryptService_Windows>();
            services.AddSingleton<IBlockService, BlockService>();
            services.AddSingleton<IGroupService, GroupService>();
            services.AddSingleton<IMainLogicService, MainLogicService>();
            services.AddScoped(typeof(IMyIO), options.IMyIO_ImplementationType);
        }
    }
}
