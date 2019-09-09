using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Casting
{
    class GroomCast
    {
        public static EventEntities DB = new EventEntities();
        public static Entities.GroomEntity GroomEntity(Groom groom)
        {
            return new Entities.GroomEntity()
            {
                id = groom.id,
                name = groom.name,
                token = groom.token,
                url = groom.url
            };
        }
        // ומחזירה תמונה GroomEntites פונקציה שמקבלת 
        public static Groom GroomDal(Entities.GroomEntity groom)
        {
            return new Groom()
            {
                id = groom.id,
                name = groom.name,
                token = groom.token,
                url = groom.url
            };
        }
    }
}