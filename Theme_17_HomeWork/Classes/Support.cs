using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Theme_17_HomeWork.Classes
{
    static class Support
    {
        //public static Support()
        //{

        //}

        
        public static float GetUserValue(string stringunput)
        {
            float value = 0;
            if (!float.TryParse(stringunput, out value))
                MessageBox.Show($"Введенное число некоректно: {stringunput}\n " +
    $"в системе установлен разделитель длобной и целой части \"{System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator}\""
    );
            else if (value <= 0)
            {
                MessageBox.Show("Введено отрицательное число или введенное число ноль.");
            }
            return value;
        }
    }
}
