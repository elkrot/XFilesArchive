using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace XFilesArchive.Infrastructure.Utilites
{
    #region MIFactory
    public static class MFIFactory
    {
        public static Dictionary<string, string> GetMediaFileInfoDictionary(string extension, string filePath)
        {

            MIFileType MIFileType = MIManager.GetMediaFileType(extension);
            MIManager miManager;


            switch (MIFileType)
            {
                case MIFileType.PictureFile:
                    miManager = new PictureFileInfoManager(extension, filePath);
                    return miManager.MFIDictionary;
                case MIFileType.VideoFile:
                    miManager = new VideoFileInfoManager(extension, filePath);
                    return miManager.MFIDictionary;
                case MIFileType.AudioFile:
                    miManager = new AudioFileInfoManager(extension, filePath);
                    return miManager.MFIDictionary;
                case MIFileType.NoMediaFile:
                    break;
                default:

                    break;
            }

            return null;


        }
    }
    #endregion


    #region Базовый класс MIManager
    public abstract class MIManager
    {
        protected string Extension { get; set; }
        protected string FilePath { get; set; }
        protected TagLib.File TagLibFile;
        //  protected MediaFile MediaFile;
        protected MediaInfoLib.MediaInfo MI;

        public Dictionary<string, string> MFIDictionary;
        public MIFileType miFileType { get; set; }

        private static string[] PictureExtensions = { ".PNG", ".BMP", ".JPG", ".JPEG", ".GIF", ".TIFF" };
        private static string[] VideoExtensions = { ".MPEG4", ".AVI", ".MOV", ".MKV", ".MP4", ".WMV", ".MPG", "VOB" };
        private static string[] AudioExtensions = { ".MP3", ".OGG", ".FLAC", ".WAV" };
        public static string[] AllExtension { get { return PictureExtensions.Concat(VideoExtensions).ToArray().Concat(AudioExtensions).ToArray(); } }
        public static bool IsMediaFile(string extension)
        {
            return AllExtension.Contains(extension);
        }
        #region GetMediaFileType
        public static MIFileType GetMediaFileType(string extension)
        {
            if (PictureExtensions.Contains(extension.ToUpper()))
            {
                return MIFileType.PictureFile;
            }
            else if (VideoExtensions.Contains(extension.ToUpper()))
            {
                return MIFileType.VideoFile;
            }
            else if (AudioExtensions.Contains(extension.ToUpper()))
            {
                return MIFileType.AudioFile;
            }
            else return MIFileType.NoMediaFile;

        }
        #endregion

        #region MIManager
        public MIManager(string extension, string filePath)
        {
            MFIDictionary = new Dictionary<string, string>();
            Extension = extension;
            miFileType = GetMediaFileType(extension);
            if (miFileType != MIFileType.NoMediaFile)
            {
                TrySetTagLibFile(filePath);
                TrySetMediaInfo(filePath);
            }
            FilePath = filePath;
            FillTheMFIDictionary();
        }
        #endregion

        #region TrySetMediaInfo
        private void TrySetMediaInfo(string filePath)
        {
            try
            {
                MI = new MediaInfoLib.MediaInfo();

                #region Media Info Init
                FileStream From = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                //From: preparing a memory buffer for reading
                byte[] From_Buffer = new byte[64 * 1024];
                int From_Buffer_Size; //The size of the read file buffer

                //Preparing to fill MediaInfo with a buffer
                MI.Open_Buffer_Init(From.Length, 0);

                //The parsing loop
                do
                {
                    //Reading data somewhere, do what you want for this.
                    From_Buffer_Size = From.Read(From_Buffer, 0, 64 * 1024);

                    //Sending the buffer to MediaInfo
                    System.Runtime.InteropServices.GCHandle GC = System.Runtime.InteropServices.GCHandle.Alloc(From_Buffer, System.Runtime.InteropServices.GCHandleType.Pinned);
                    IntPtr From_Buffer_IntPtr = GC.AddrOfPinnedObject();
                    Status Result = (Status)MI.Open_Buffer_Continue(From_Buffer_IntPtr, (IntPtr)From_Buffer_Size);
                    GC.Free();
                    if ((Result & Status.Finalized) == Status.Finalized)
                        break;

                    //Testing if MediaInfo request to go elsewhere
                    if (MI.Open_Buffer_Continue_GoTo_Get() != -1)
                    {
                        Int64 Position = From.Seek(MI.Open_Buffer_Continue_GoTo_Get(), SeekOrigin.Begin); //Position the file
                        MI.Open_Buffer_Init(From.Length, Position); //Informing MediaInfo we have seek
                    }
                }
                while (From_Buffer_Size > 0);

                //Finalizing
                MI.Open_Buffer_Finalize(); //This is the end of the stream, MediaInfo must finnish some work
                #endregion
            }
            catch (Exception ex)
            {
                var c = ex.Message;

            }
        }
        #endregion

        #region TrySetTagLibFile
        private void TrySetTagLibFile(string filePath)
        {
            try
            {
                TagLibFile = TagLib.File.Create(filePath);
            }
            catch (Exception)
            {

            }

        }
        #endregion


        abstract public MFileInfo GetMIFileInfo();
        abstract protected void FillTheMFIDictionary();
    }
    public enum MIFileType
    {
        PictureFile,
        VideoFile,
        AudioFile,
        NoMediaFile
    }
    [FlagsAttribute]
    public enum Status
    {
        Accepted = 0x01,
        Filled = 0x02,
        Updated = 0x04,
        Finalized = 0x08
    };
    #endregion

    // Конкретные создатели

    #region Создатель описания картинки
    public class PictureFileInfoManager : MIManager
    {
        BitmapMetadata metadata;
        public PictureFileInfoManager(string extension, string filePath) : base(extension, filePath)
        {
            using (FileStream f = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BitmapDecoder decoder = BitmapDecoder.Create(f, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
                metadata = (BitmapMetadata)decoder.Frames[0].Metadata;

            }
        }
        public override MFileInfo GetMIFileInfo()
        {
            return new MIPictureFileInfo(MFIDictionary);
        }



        protected override void FillTheMFIDictionary()
        {
            MFIDictionary.Add("Format", metadata == null ? "" : metadata?.Format ?? "");
            MFIDictionary.Add("Rating", metadata == null ? "" : metadata?.Rating.ToString() ?? "");
            MFIDictionary.Add("Title", metadata == null ? "" : metadata?.Title ?? "");
            MFIDictionary.Add("Keywords", metadata == null ? "" : String.Join(", ", metadata.Keywords));
            MFIDictionary.Add("PhotoHeight", TagLibFile?.Properties == null ? "" : TagLibFile.Properties.PhotoHeight.ToString().Trim());
            MFIDictionary.Add("PhotoWidth", TagLibFile?.Properties == null ? "" : TagLibFile.Properties.PhotoWidth.ToString().Trim());
        }


    }
    #endregion

    #region Создатель описания Видео файла
    public class VideoFileInfoManager : MIManager
    {
        public VideoFileInfoManager(string extension, string filePath) : base(extension, filePath)
        {

        }
        public override MFileInfo GetMIFileInfo()
        {
            return new MIVideoFileInfo(MFIDictionary);
        }

        #region MediaInfo

        private static string ReadMediaInfo()
        {
            //Initilaizing MediaInfo
            MediaInfoLib.MediaInfo MI = new MediaInfoLib.MediaInfo();

            //From: preparing an example file for reading
            FileStream From = new FileStream(@"d:\temp\test.mp4", FileMode.Open, FileAccess.Read);

            //From: preparing a memory buffer for reading
            byte[] From_Buffer = new byte[64 * 1024];
            int From_Buffer_Size; //The size of the read file buffer

            //Preparing to fill MediaInfo with a buffer
            MI.Open_Buffer_Init(From.Length, 0);

            //The parsing loop
            do
            {
                //Reading data somewhere, do what you want for this.
                From_Buffer_Size = From.Read(From_Buffer, 0, 64 * 1024);

                //Sending the buffer to MediaInfo
                System.Runtime.InteropServices.GCHandle GC = System.Runtime.InteropServices.GCHandle.Alloc(From_Buffer, System.Runtime.InteropServices.GCHandleType.Pinned);
                IntPtr From_Buffer_IntPtr = GC.AddrOfPinnedObject();
                Status Result = (Status)MI.Open_Buffer_Continue(From_Buffer_IntPtr, (IntPtr)From_Buffer_Size);
                GC.Free();
                if ((Result & Status.Finalized) == Status.Finalized)
                    break;

                //Testing if MediaInfo request to go elsewhere
                if (MI.Open_Buffer_Continue_GoTo_Get() != -1)
                {
                    Int64 Position = From.Seek(MI.Open_Buffer_Continue_GoTo_Get(), SeekOrigin.Begin); //Position the file
                    MI.Open_Buffer_Init(From.Length, Position); //Informing MediaInfo we have seek
                }
            }
            while (From_Buffer_Size > 0);

            //Finalizing
            MI.Open_Buffer_Finalize(); //This is the end of the stream, MediaInfo must finnish some work

            //Get() example
            return "Container format is " + MI.Get(MediaInfoLib.StreamKind.General, 0, "Format");

        }

        private static void DisplayMediaInfo()
        {
            String ToDisplay;
            MediaInfoLib.MediaInfo MI = new MediaInfoLib.MediaInfo();

            ToDisplay = MI.Option("Info_Version", "0.7.0.0;MediaInfoDLL_Example_CS;0.7.0.0");
            if (ToDisplay.Length == 0)
            {
                Console.Write("MediaInfo.Dll: this version of the DLL is not compatible");
                return;
            }

            //Information about MediaInfo
            ToDisplay += "\r\n\r\nInfo_Parameters\r\n";
            ToDisplay += MI.Option("Info_Parameters");

            ToDisplay += "\r\n\r\nInfo_Capacities\r\n";
            ToDisplay += MI.Option("Info_Capacities");

            ToDisplay += "\r\n\r\nInfo_Codecs\r\n";
            ToDisplay += MI.Option("Info_Codecs");

            //An example of how to use the library
            ToDisplay += "\r\n\r\nOpen\r\n";
            ToDisplay += "\r\n\r\nClose\r\n==========================";
            ToDisplay += "\r\n\r\nClose\r\n==========================";
            MI.Open("d:\temp\test.mp4");

            ToDisplay += "\r\n\r\nInform with Complete=false\r\n";
            MI.Option("Complete");
            ToDisplay += MI.Inform();

            ToDisplay += "\r\n\r\nInform with Complete=true\r\n";
            MI.Option("Complete", "1");
            ToDisplay += MI.Inform();

            ToDisplay += "\r\n\r\nCustom Inform\r\n";
            MI.Option("Inform", "General;File size is %FileSize% bytes");
            ToDisplay += MI.Inform();

            ToDisplay += "\r\n\r\nGet with Stream=General and Parameter='FileSize'\r\n";
            ToDisplay += MI.Get(0, 0, "FileSize");

            ToDisplay += "\r\n\r\nGet with Stream=General and Parameter=46\r\n";
            ToDisplay += MI.Get(0, 0, 46);

            ToDisplay += "\r\n\r\nCount_Get with StreamKind=Stream_Audio\r\n";
            ToDisplay += MI.Count_Get(MediaInfoLib.StreamKind.Audio);

            ToDisplay += "\r\n\r\nGet with Stream=General and Parameter='AudioCount'\r\n";
            ToDisplay += MI.Get(MediaInfoLib.StreamKind.General, 0, "AudioCount");

            ToDisplay += "\r\n\r\nGet with Stream=Audio and Parameter='StreamCount'\r\n";
            ToDisplay += MI.Get(MediaInfoLib.StreamKind.Audio, 0, "StreamCount");

            ToDisplay += "\r\n\r\nClose\r\n";
            MI.Close();
            ToDisplay += "\r\n\r\nClose\r\n==========================";
            //Example with a stream
            ToDisplay += "\r\n" + ReadMediaInfo() + "\r\n";

            //Displaying the text
            Console.Write(ToDisplay);
        }



        [FlagsAttribute]
        public enum Status
        {
            Accepted = 0x01,
            Filled = 0x02,
            Updated = 0x04,
            Finalized = 0x08
        };
        #endregion

        #region Заполнить словарь аттрибутов
        protected override void FillTheMFIDictionary()
        {
            if (TagLibFile != null)
            {
                MFIDictionary.Add("General.VideoHeight", TagLibFile.Properties.VideoHeight.ToString().Trim());
                MFIDictionary.Add("General.VideoWidth", TagLibFile.Properties.VideoWidth.ToString().Trim());
            }
            if (MI != null)
            {
                FillMFDictionary();
            }
        }

        private void FillMFDictionary()
        {

            /*
1	General
2	Video
3	Audio
4	Image
5	Text
6	Menu
7	Generic
8	Other
             */

            FillMIDictionaryByType(1, "General");

            var strAudioCount = MI.Get(MediaInfoLib.StreamKind.General, 0, "AudioCount");
            int AudioCount = 0;
            int.TryParse(strAudioCount, out AudioCount);
            var strVideoCount = MI.Get(MediaInfoLib.StreamKind.General, 0, "VideoCount");
            int VideoCount = 0;
            int.TryParse(strVideoCount, out VideoCount);
            if (AudioCount > 0)
            {
                FillMIDictionaryByType(3, "Audio");
            }

            if (VideoCount > 0)
            {
                FillMIDictionaryByType(2, "Video");
            }
        }

        private void FillMIDictionaryByType(int idParamsType, string paramTypeTitle)
        {
            foreach (var item in GetMIParameters(idParamsType))
            {
                var tmp = MI.Get(MediaInfoLib.StreamKind.General, 0, item);
                if (!string.IsNullOrWhiteSpace(tmp))
                {

                    MFIDictionary.Add(string.Format("{0}.{1}", paramTypeTitle, item), tmp);
                }
            }
        }

        private static string[] GetMIParameters(int id)
        {
            IConfiguration _configuration = new ConfigurationData();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
            {
                var result = new List<string>();
                connection.Open();
                string sql = @"SELECT Title FROM MediaInfoParameter where MediaInfoTypeId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("id", id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(reader.GetString(0));
                }

                return result.ToArray();

            }
        }
        #endregion

    }


    #endregion

    #region Создатель описания аудио файла
    public class AudioFileInfoManager : MIManager
    {
        public AudioFileInfoManager(string extension, string filePath) : base(extension, filePath)
        {

        }
        public override MFileInfo GetMIFileInfo()
        {
            return new MIAudioFileInfo(MFIDictionary);
        }
        #region Заполнить словарь аттрибутов
        protected override void FillTheMFIDictionary()
        {
            MFIDictionary.Add("Artist", String.Join(", ", TagLibFile.Tag.Performers));
            MFIDictionary.Add("Tracknumber", TagLibFile.Tag.Track.ToString().Trim());
            MFIDictionary.Add("Title", TagLibFile.Tag.Title);
            MFIDictionary.Add("Album", TagLibFile.Tag.Album);
            MFIDictionary.Add("Year", TagLibFile.Tag.Year.ToString().Trim());
            MFIDictionary.Add("Genre", TagLibFile.Tag.FirstGenre);
            MFIDictionary.Add("Bitrate", TagLibFile.Properties.AudioBitrate.ToString().Trim());
            MFIDictionary.Add("Channels", TagLibFile.Properties.AudioChannels.ToString().Trim());
            MFIDictionary.Add("Duration", TagLibFile.Properties.Duration.ToString("mm\\:ss"));

        }
        #endregion
    }
    #endregion

    // Продукты

    #region Базовый класс продуктов
    [Serializable]
    abstract public class MFileInfo
    {
        public Dictionary<string, string> MFIDictionary;
        public MFileInfo(Dictionary<string, string> mfiDictionary)
        {
            MFIDictionary = mfiDictionary;
        }

    }
    #endregion

    #region Картинки
    [Serializable]
    public class MIPictureFileInfo : MFileInfo
    {

        public MIPictureFileInfo(Dictionary<string, string> mfiDictionary) : base(mfiDictionary)
        {

        }

    }
    #endregion

    #region Видео
    [Serializable]
    public class MIVideoFileInfo : MFileInfo
    {
        public MIVideoFileInfo(Dictionary<string, string> mfiDictionary) : base(mfiDictionary)
        {

        }

    }
    #endregion

    #region Аудио
    [Serializable]
    public class MIAudioFileInfo : MFileInfo
    {
        public MIAudioFileInfo(Dictionary<string, string> mfiDictionary) : base(mfiDictionary)
        {

        }

    }
    #endregion

}
