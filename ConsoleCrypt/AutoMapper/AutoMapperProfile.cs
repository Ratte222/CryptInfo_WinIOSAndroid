using AutoMapper;
using CommonForCryptPasswordLibrary.Model;
using ConsoleCrypt.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CryptBlockModel, BlockDataDTO>();
            CreateMap<CryptGroupModel, GroupDataDTO>();
        }
    }
}
