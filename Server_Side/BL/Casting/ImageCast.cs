using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Casting
{
    class ImageCast
    {
        public static EventEntities DB = new EventEntities();
        //imangeEntity פונקציית המרה שמקבלת תמונה ומחזירה 
        public static ImageEntity GetImageEntity(image img)
        {
            return new ImageEntity()
            {
                id = img.id,
                name = img.name,
                url = img.url,
                isGroom = img.isGroom,
                isCutFace = img.isCutFace,
                isBlur = img.isBlur,
                isClosedEye = img.isClosedEye,
                isDark = img.isDark,
                isIndoors = img.isIndoors,
                isOutdoors=img.isOutdoors,
                isLight = img.isLight,
                hasChildren = img.hasChildren,
                numPerson = img.numPerson,
                isInRecycleBin = img.isInRecycleBin
            };
        }
        // ומחזירה תמונה ImageEntites פונקציה שמקבלת 
        public static ImageEntity GetImage(ImageEntity imgE)
        {
            return new ImageEntity()
            {
                id = imgE.id,
                name = imgE.name,
                url = imgE.url,
                isGroom = imgE.isGroom,
                isCutFace = imgE.isCutFace,
                isBlur = imgE.isBlur,
                isClosedEye = imgE.isClosedEye,
                isDark = imgE.isDark,
                isIndoors = imgE.isIndoors,
                isOutdoors = imgE.isOutdoors,
                isLight = imgE.isLight,
                hasChildren = imgE.hasChildren,
                numPerson = imgE.numPerson,
                isInRecycleBin = imgE.isInRecycleBin
            };
        }
    }
}
