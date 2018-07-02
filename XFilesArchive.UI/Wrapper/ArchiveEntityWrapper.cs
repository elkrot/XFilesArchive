using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Wrapper
{
    public class ArchiveEntityWrapper : ModelWrapper<ArchiveEntity>
    {
        public ArchiveEntityWrapper(ArchiveEntity model) : base(model)
        {
        }
      
        #region ArchiveEntityKey
        public System.Int32 ArchiveEntityKey
        {
            get { return GetValue<System.Int32>(); }
            set { SetValue(value); }
        }

        public System.Int32 ArchiveEntityKeyOriginalValue => GetOriginalValue<System.Int32>(nameof(ArchiveEntityKey));

        public bool ArchiveEntityKeyIsChanged => GetIsChanged(nameof(ArchiveEntityKey));

        #endregion

        #region ParentEntityKey
        public System.Nullable<System.Int32> ParentEntityKey
        {
            get { return GetValue<System.Nullable<System.Int32>>(); }
            set { SetValue(value); }
        }

        public System.Nullable<System.Int32> ParentEntityKeyOriginalValue => GetOriginalValue<System.Nullable<System.Int32>>(nameof(ParentEntityKey));

        public bool ParentEntityKeyIsChanged => GetIsChanged(nameof(ParentEntityKey));
        #endregion

        #region DriveId
        public System.Nullable<System.Int32> DriveId
        {
            get { return GetValue<System.Nullable<System.Int32>>(); }
            set { SetValue(value); }
        }

        public System.Nullable<System.Int32> DriveIdOriginalValue => GetOriginalValue<System.Nullable<System.Int32>>(nameof(DriveId));

        public bool DriveIdIsChanged => GetIsChanged(nameof(DriveId));
        #endregion

        #region Title
        public System.String Title
        {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }

        public System.String TitleOriginalValue => GetOriginalValue<System.String>(nameof(Title));

        public bool TitleIsChanged => GetIsChanged(nameof(Title));
        #endregion

        #region EntityType
        public System.Byte EntityType
        {
            get { return GetValue<System.Byte>(); }
            set { SetValue(value); }
        }

        public System.Byte EntityTypeOriginalValue => GetOriginalValue<System.Byte>(nameof(EntityType));

        public bool EntityTypeIsChanged => GetIsChanged(nameof(EntityType));
        #endregion

        #region EntityPath
        public System.String EntityPath
        {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }

        public System.String EntityPathOriginalValue => GetOriginalValue<System.String>(nameof(EntityPath));

        public bool EntityPathIsChanged => GetIsChanged(nameof(EntityPath));
        #endregion

        #region EntityExtension
        public System.String EntityExtension
        {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }

        public System.String EntityExtensionOriginalValue => GetOriginalValue<System.String>(nameof(EntityExtension));

        public bool EntityExtensionIsChanged => GetIsChanged(nameof(EntityExtension));
        #endregion

        #region Description
        public System.String Description
        {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }

        public System.String DescriptionOriginalValue => GetOriginalValue<System.String>(nameof(Description));

        public bool DescriptionIsChanged => GetIsChanged(nameof(Description));
        #endregion

        #region FileSize
        public System.Nullable<long> FileSize
        {
            get { return GetValue<System.Nullable<long>>(); }
            set { SetValue(value); }
        }

        public System.Nullable<long> FileSizeOriginalValue => GetOriginalValue<System.Nullable<long>>(nameof(FileSize));

        public bool FileSizeIsChanged => GetIsChanged(nameof(FileSize));
        #endregion

        #region CreatedDate
        public System.DateTime CreatedDate
        {
            get { return GetValue<System.DateTime>(); }
            set { SetValue(value); }
        }

        public System.DateTime CreatedDateOriginalValue => GetOriginalValue<System.DateTime>(nameof(CreatedDate));

        public bool CreatedDateIsChanged => GetIsChanged(nameof(CreatedDate));
        #endregion

        #region Checksum
        public System.String Checksum
        {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }

        public System.String ChecksumOriginalValue => GetOriginalValue<System.String>(nameof(Checksum));

        public bool ChecksumIsChanged => GetIsChanged(nameof(Checksum));
        #endregion

        private byte[] EntityInfo { get { return GetValue<byte[]>(); } }

        private byte[] MFileInfo { get { return GetValue<byte[]>(); } }


        #region FileInfo
        public Dictionary<string, string> FileInfo
        {
            get
            {
                var data = EntityInfo;
                if (data == null || data.Count() <= 1) return default(Dictionary<string, string>);
                using (MemoryStream stream = new MemoryStream(data))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    object obj = formatter.Deserialize(stream);
                    if (!(obj is Dictionary<string, string>))
                        return null;
                    return (Dictionary<string, string>)obj;
                }
            }

        }

        #endregion

        #region MediaInfo
        public Dictionary<string, string> MediaInfo
        {
            get
            {
                var data = MFileInfo;
                if (data == null || data.Count() <= 1) return default(Dictionary<string, string>);
                using (MemoryStream stream = new MemoryStream(data))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (Dictionary<string, string>)formatter.Deserialize(stream);
                }
            }

        }

        #endregion

        public ChangeTrackingCollection<CategoryWrapper> Categories { get; private set; }

        public ChangeTrackingCollection<ImageWrapper> Images { get; private set; }

        public ChangeTrackingCollection<TagWrapper> Tags { get; private set; }


        protected override void InitializeCollectionProperties(ArchiveEntity model)
        {
            if (model.Images == null)
            {
                throw new ArgumentException("Images cannot be null");
            }

            Images = new ChangeTrackingCollection<ImageWrapper>(
              model.Images.Select(e => new ImageWrapper(e)));
            RegisterCollection(Images, model.Images.ToList());

            if (model.Tags == null)
            {
                throw new ArgumentException("Tags cannot be null");
            }

            Tags = new ChangeTrackingCollection<TagWrapper>(
              model.Tags.Select(e => new TagWrapper(e)));
            RegisterCollection(Tags, model.Tags.ToList());

            if (model.Categories == null)
            {
                throw new ArgumentException("Categorys cannot be null");
            }

            Categories = new ChangeTrackingCollection<CategoryWrapper>(
              model.Categories.Select(e => new CategoryWrapper(e)));
            RegisterCollection(Categories, model.Categories.ToList());

        }





    }
}
