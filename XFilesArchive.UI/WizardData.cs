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
        byte _saveImages ;
        byte _saveThumbnails ;
        byte _saveThumbnailsToDb ;
        byte _saveMedia;
        

        #region DriveTitle
        public string DriveTitle
        {
            get { return _driveTitle; }
            set
            {
                _driveTitle = value;
                OnPropertyChanged("DriveTitle");
            }
        }
        #endregion

        #region DriveCode
        public string DriveCode
        {
            get { return _driveCode; }
            set
            {
                _driveCode = value;
                OnPropertyChanged("DriveCode");
            }
        }
        #endregion

        #region DriveLetter
        public string DriveLetter
        {
            get { return _driveLetter; }
            set
            {
                _driveLetter = value;
                OnPropertyChanged("DriveLetter");
            }
        }
        #endregion

        #region MaxImagesInDirectory
        public int MaxImagesInDirectory
        {
            get { return _maxImagesInDirectory; }
            set
            {
                _maxImagesInDirectory = value;
                OnPropertyChanged("MaxImagesInDirectory");
            }
        }
        #endregion

        #region IsSecret
        public byte IsSecret
        {
            get { return _isSecret; }
            set
            {
                _isSecret = value;
                OnPropertyChanged("IsSecret");
            }
        }
        #endregion

        #region SaveImages
        public byte SaveImages
        {
            get { return _saveImages; }
            set
            {
                _saveImages = value;
                OnPropertyChanged("SaveImages");
            }
        }
        #endregion

        #region SaveThumbnails
        public byte SaveThumbnails
        {
            get { return _saveThumbnails; }
            set
            {
                _saveThumbnails = value;
                OnPropertyChanged("SaveThumbnails");
            }
        }
        #endregion

        #region SaveThumbnailsToDb
        public byte SaveThumbnailsToDb
        {
            get { return _saveThumbnailsToDb; }
            set
            {
                _saveThumbnailsToDb = value;
                OnPropertyChanged("SaveThumbnailsToDb");
            }
        }
        #endregion
       
        #region SaveMedia
        public byte SaveMedia
        {
            get { return _saveMedia; }
            set
            {
                _saveMedia = value;
                OnPropertyChanged("SaveMedia");
            }
        }
        #endregion

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
