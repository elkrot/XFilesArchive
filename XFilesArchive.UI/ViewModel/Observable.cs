using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XFilesArchive.UI.ViewModel
{
    public class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string _propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(_propertyName));
            }
        }
    }
}
