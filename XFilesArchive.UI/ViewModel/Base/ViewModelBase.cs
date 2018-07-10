using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XFilesArchive.UI.ViewModel
{
    //DependencyObject
    public class ViewModelBase :  INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string _propertyName =null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_propertyName));
        }
    }
}
