using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ImageEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Nullable<bool> isBlur { get; set; }
        public Nullable<bool> isClosedEye { get; set; }
        public Nullable<bool> isDark { get; set; }
        public Nullable<bool> isCutFace { get; set; }
        public Nullable<bool> isGroom { get; set; }
        public Nullable<bool> isLight { get; set; }
        public Nullable<bool> isOutdoors { get; set; }
        public Nullable<bool> isIndoors { get; set; }
        public Nullable<bool> hasChildren { get; set; }
        public Nullable<int> numPerson { get; set; }
        public Nullable<bool> isInRecycleBin { get; set; }
    }
}
