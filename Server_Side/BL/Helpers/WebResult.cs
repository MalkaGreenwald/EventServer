using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helpers
{
    //מחלקה זו מיועדת לכל תוצאה שהיא שמוחזרת לקליינט
    public class WebResult<T>
    {
        public bool Status { get; set; }//האם הצליח

        public string Message { get; set; }//הודעה

        public T Value { get; set; }//הערך המוחזר
    }
}
