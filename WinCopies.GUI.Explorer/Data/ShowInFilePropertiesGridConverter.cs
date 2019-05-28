using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util.Data;

namespace WinCopies.GUI.Explorer.Data
{
    public class ShowInFilePropertiesGridConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value!=null)
                // todo:
            Console.WriteLine("truc: " + (!(value is IO.ArchiveItemInfo)).ToString());
            return !(value is IO.ArchiveItemInfo);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
