using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace XFilesArchive.UI.Converters
{
    public class MultiTupleConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {


            Tuple<string, string> tuple = new Tuple<string, string>((string)values[0], (string)values[1]);
            return (object)tuple;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MultiTupleDoubleConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {


            Tuple<double?, double?> tuple = new Tuple<double?, double?>((double?)values[0], (double?)values[1]);
            return (object)tuple;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MultiTupleIntegerConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Tuple<int?, int?> tuple = new Tuple<int?, int?>((int?)values[0], (int?)values[1]);
            return (object)tuple;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MultiTuplePasswordBoxConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Tuple<string, PasswordBox> tuple = new Tuple<string, PasswordBox>((string)values[0], (PasswordBox)values[1]);
            return (object)tuple;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    



}
