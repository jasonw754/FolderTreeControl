using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GeekJ.FolderTreeControl.Model
{
    public class IsCheckedConverter : IMultiValueConverter
    {

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // first value is the IsChecked property of the current checkbox
            bool? isChecked = values[0] as bool?;
            
            // second value tells where the current checkbox is explicity defined or just inherited
            bool isInherited = (bool) values[1];

            // third value is the IsChecked property of the checkbox in the parent tree item (unless this is the top level)
            if (values.Length < 3)
            {
                // top level, no parent to factor in
                return isChecked;
            }
            bool? isParentChecked = values[2] as bool?;

            if (isInherited)
            {
                return isParentChecked;
            }
            else
            {
                return isChecked;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            // the value maps directly to the IsChecked value, which is the first target
            bool? isChecked = value as bool?;

            if (targetTypes.Length > 2)
            {
                // parent IsChecked is the second target which should be left alone
                object isParentChecked = Binding.DoNothing;

                return new object[] { isChecked, false, Binding.DoNothing };
            }

            return new object[] { isChecked, false };
        }

        #endregion
    }
}
