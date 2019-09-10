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
            List<ImageEntity> listEntity = null;
            if (DB.images.Count() > 0)
            {
                List<image> listImage = DB.images.Where(img => img.isInRecycleBin == null || img.isInRecycleBin == false).ToList();
                listEntity = new List<ImageEntity>();
                if (listImage != null)
                    foreach (var image in listImage)
                        listEntity.Add(Casting.ImageCast.GetImageEntity(image));
                return listEntity;
            }
            return listEntity;
        }

        public static bool DeleteImg(string url)
        {
            //image img = DB.images.FirstOrDefault(image => image.url == url);
            //if (img != null)
            //    DB.images.Remove(img);
            DB.images.Where(image => image.url == url).ToList().ForEach(f => f.isInRecycleBin = true);
            DB.SaveChanges();
            return true;
        }

        public static List<ImageEntity> getRecycleBin()
        {
            List<ImageEntity> rec = new List<ImageEntity>();
            List<image> images = DB.images.Where(r => r.isInRecycleBin != null && r.isInRecycleBin == true).ToList();

            if (images != null)
                foreach (var img in images)
                {
                    rec.Add(Casting.ImageCast.GetImageEntity(img));
                }
            //rec.ForEach(f => rec.Add(Casting.ImageCast.GetImageEntity(f)));
            return rec;
        }
    }

}