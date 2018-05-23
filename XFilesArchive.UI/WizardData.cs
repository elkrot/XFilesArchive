using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.UI
{
    public class WizardData : INotifyPropertyChanged
    {
        string _driveTitle;
        string _driveCode;
        string _driveLetter;
        int _maxImagesInDirectory;
        byte _isSecret;

        public string DriveTitle
        {
            get { return _driveTitle; }
            set
            {
                _driveTitle = value;
                OnPropertyChanged("DriveTitle");
            }
        }
        public string DriveCode
        {
            get { return _driveCode; }
            set
            {
                _driveCode = value;
                OnPropertyChanged("DriveCode");
            }
        }



        public string DriveLetter
        {
            get { return _driveLetter; }
            set
            {
                _driveLetter = value;
                OnPropertyChanged("DriveLetter");
            }
        }

        public int MaxImagesInDirectory
        {
            get { return _maxImagesInDirectory; }
            set
            {
                _maxImagesInDirectory = value;
                OnPropertyChanged("MaxImagesInDirectory");
            }
        }

        public byte IsSecret
        {
            get { return _isSecret; }
            set
            {
                _isSecret = value;
                OnPropertyChanged("IsSecret");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }

}
