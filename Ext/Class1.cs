using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ext
{
    public static class MyExt
    {
        public static void SendMoneyTo(this Accaunt accaunt, Accaunt targetAccaunt, float moneyValue)
        {
            if ((accaunt.Balans >= moneyValue) && (moneyValue > 0))
            {
                targetAccaunt.PushMoney(moneyValue);
                accaunt.PopMoney(moneyValue);
            }
        }
    }
}
