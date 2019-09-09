using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;

namespace BL
{
    public class Images
    {
        public static EventEntities DB = new EventEntities();
        public static List<ImageEntity> GetImages()
        {
            var listImage = DB.images.ToList();
            List<ImageEntity> listEntity = new List<ImageEntity>();
            foreach (var image in listImage)
                listEntity.Add(Casting.ImageCast.GetImageEntity(image));
            return listEntity;
        }
    }
}