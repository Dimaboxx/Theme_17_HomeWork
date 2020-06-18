using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNameSpase
{
    public class PercentsView
    {
        DateTime date;
        float balanstoendofperriod;
        float extrabalans;
        /// <summary>
        /// default empty constructor
        /// </summary>
        public PercentsView()
        {

        }

        public DateTime Date { get { return date; } set { date = value; } }
        public float BalansToEndofPerriod { get { return balanstoendofperriod; } set { balanstoendofperriod = value; } }
        public float ExtraBalans { get { return extrabalans; } set {extrabalans = value; } }  

        public float Cache { get; set; }
    }
}
