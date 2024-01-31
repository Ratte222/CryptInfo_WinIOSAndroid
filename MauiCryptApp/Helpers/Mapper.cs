using CommonForCryptPasswordLibrary.Model;
using MauiCryptApp.Models;
namespace MauiCryptApp.Helpers
{
    public static class Mapper
    {
        public static Item Map(this BlockModel blockModel)
        {
            return new Item()
            {
                Id = blockModel.Id,
                Title = blockModel.Title,
                Description = blockModel.Description,
                Email = blockModel.Email,
                Password = blockModel.Password,
                UserName = blockModel.UserName,
                Phone = blockModel.Phone,
                AdditionalInfo = blockModel.AdditionalInfo,
            };
        }

        public static BlockModel Map(this Item item)
        {
            return new BlockModel()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Email = item.Email,
                Password = item.Password,
                UserName = item.UserName,
                Phone = item.Phone,
                AdditionalInfo = item.AdditionalInfo,
            };
        }

        public static void UpdateBlockModel(this BlockModel blockModel, Item item)
        {
            blockModel.Id = item.Id;
            blockModel.Title = item.Title;
            blockModel.Description = item.Description;
            blockModel.Email = item.Email;
            blockModel.Password = item.Password;
            blockModel.UserName = item.UserName;
            blockModel.Phone = item.Phone;
            blockModel.AdditionalInfo = item.AdditionalInfo;
        }
    }
}
