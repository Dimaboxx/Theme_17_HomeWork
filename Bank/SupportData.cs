using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNameSpase
{
    public enum RatesPeriods
    {
        Zero,
        Daily,
        Mouth,
        Year,
        End,
        Infinity
    }

    public enum AccauntsType
    {
        Simple,
        Deposite,
        Credit
    }

    public enum ClientType
    {
        Standart,
        VIP,
        Organisation
    }

    static class getEnumString<T>
    {
        public static string GetString(T val)
        {
        return val.ToString();

        }
    } 



}
