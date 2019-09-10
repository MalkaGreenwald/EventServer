using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class GetFilteredImages
    {
        public static EventEntities DB = new EventEntities();
        public static List<string> IsBlur()
        {
            List<string> urlImage = new List<string>();
            foreach (var img in DB.images)
            {
                if (img.isBlur == true)
                    urlImage.Add(img.url);
            }
            return urlImage;
        }
        public static List<string> IsClosedEye()
        {
            List<string> urlImage = new List<string>();
            foreach (var img in DB.images)
            {
                if (img.isClosedEye == true)
                    urlImage.Add(img.url);
            }
            return urlImage;
        }
        public static List<string> IsCutFace()
        {
            List<string> urlImage = new List<string>();
            foreach (var img in DB.images)
            {
                if (img.isCutFace == true)
                    urlImage.Add(img.url);
            }
            return urlImage;
        }
        public static List<string> IsDark()
        {
            List<string> urlImage = new List<string>();
            foreach (var img in DB.images)
            {
                if (img.isDark == true)
                    urlImage.Add(img.url);
            }
            return urlImage;
        }
        public static List<string> IsGroom()
        {
            List<string> urlImage = new List<string>();
            foreach (var img in DB.images)
            {
                if (img.isGroom == true)
                    urlImage.Add(img.url);
            }
            return urlImage;
        }
        public static List<string> IsInside()
        {
            List<string> urlImage = new List<string>();
            foreach (var img in DB.images)
            {
                if (img.isIndoors == true)
                    urlImage.Add(img.url);
            }
            return urlImage;
        }
        public static List<string> IsLight()
        {
            List<string> urlImage = new List<string>();
            foreach (var img in DB.images)
            {
                if (img.isIndoors == true)
                    urlImage.Add(img.url);
            }
            return urlImage;
        }
        public static List<string> NumPerson(int Num)
        {
            List<string> urlImage = new List<string>();
            foreach (var img in DB.images)
            {
                if (img.numPerson == Num)
                    urlImage.Add(img.url);
            }
            return urlImage;
        }
        public static List<string> HasChildren(int Num)
        {
            List<string> urlImage = new List<string>();
            foreach (var img in DB.images)
            {
                if (img.hasChildren == true)
                    urlImage.Add(img.url);
            }
            return urlImage;
        }
        //public static List<string> HasYoung(int Num)
        //{
        //    List<string> urlImage = new List<string>();
        //    foreach (var img in DB.images)
        //    {
        //        if (img.hasYoung == true)
        //            urlImage.Add(img.url);
        //    }
        //    return urlImage;
        //}
        //public static List<string> HasAdults(int Num)
        //{
        //    List<string> urlImage = new List<string>();
        //    foreach (var img in DB.images)
        //    {
        //        if (img.hasAdults == true)
        //            urlImage.Add(img.url);
        //    }
        //    return urlImage;
        //}
    }
}
