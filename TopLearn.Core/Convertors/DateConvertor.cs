using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TopLearn.Core.Convertors
{
    //baraye sakhte extention method va class bayad static bashe
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime Value)
        {
            PersianCalendar pc =new PersianCalendar();
            return pc.GetYear(Value) + "/" + pc.GetMonth(Value).ToString("00") + "/" + pc.GetDayOfMonth(Value).ToString("00");
        }
    }
}
