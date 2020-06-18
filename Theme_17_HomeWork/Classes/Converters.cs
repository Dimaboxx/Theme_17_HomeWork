using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace DreamConvertions
{
    public class ConvertIsChecketToHidden : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool passedValue = (bool?)value == true;
            //return passedValue == true ? "True" : "False";
            string Inverse = parameter.ToString();
            if (Inverse.Equals("True"))
                return passedValue == true ? "Hidden" : "Visible";
            else
                return passedValue == true ? "Visible" : "Hidden";
        }
            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }

    public class ConverSelectedItemsToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null? "False":"True";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class ConvertcbTextToHidden : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool passedValue = (string)value == "Организация";
            //return passedValue == true ? "True" : "False";
            string Inverse = parameter.ToString();
            if (Inverse.Equals("True"))
                return passedValue == true ? "Hidden" : "Visible";
            else
                return passedValue == true ? "Visible" : "Hidden";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }

}
