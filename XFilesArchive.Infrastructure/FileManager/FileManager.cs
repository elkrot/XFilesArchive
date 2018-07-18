
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using HomeArchiveX.Infrastructure;


namespace XFilesArchive.Infrastructure
{

    public class FileManager : IFIleManager
    {
        const string ERROR_ARGUMENT_EXCEPTION_MSG = "Не верно указан параметр";
        IConfiguration _configuration;
        ILogger _logger;
        public FileManager(IConfiguration configuration, ILogger logger)
        {
            #region Guard
            if (configuration == null) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(configuration));
            if (logger == null) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(logger));
            #endregion

            _configuration = configuration;
            _logger = logger;
        }

        #region Изменение размера
        /// <summary>
        /// Изменение размера изображения
        /// </summary>
        /// <param name="img">Изображение</param>
        /// <param name="nWidth">Высота</param>
        /// <param name="nHeight">Ширина</param>
        /// <returns></returns>
        public Image ResizeImg(Image img, int nWidth, int nHeight)
        {
            #region Guard
            if (img == null) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(img));
            if (nWidth <= 0)
                throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(nWidth));
            if (nHeight <= 0)
                throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(nHeight));
            #endregion
            Image result;
            try
            {
                result = new Bitmap(nWidth, nHeight);
                using (Graphics g = Graphics.FromImage((Image)result))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(img, 0, 0, nWidth, nHeight);
                    g.Dispose();
                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе ResizeImg. {0}", e.Message));
                throw new Exception("Ошибка в методе ResizeImg");
            }

            return result;

        }
        #endregion

        #region Копировать картинку 
        /// <summary>
        /// Копировать картинку 
        /// </summary>
        /// <param name="imgPath">Что копировать</param>
        /// <param name="targetDir">Куда копировать</param>
        /// <returns></returns>
        public string CopyImg(string imgPath, string targetDir)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(imgPath))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(imgPath));
            if (string.IsNullOrWhiteSpace(targetDir))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(targetDir));
            #endregion
            var result = "";

            try
            {
                var fi = new FileInfo(imgPath);
                string NewimgPath = "";

                if (IsImage(fi.Extension))
                {
                    var dir = Path.Combine(_configuration.GetTargetImagePath(), targetDir);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    NewimgPath = Path.Combine(dir, fi.Name);
                    if (!File.Exists(Path.Combine(dir, fi.Name)))
                    {
                        File.Copy(imgPath, NewimgPath, true);
                    }

                }
                result = string.IsNullOrWhiteSpace(NewimgPath) ? "" : Path.Combine(targetDir, fi.Name);
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе CopyImg. {0}", e.Message));
                throw new Exception("Ошибка в методе CopyImg");
            }


            return result;
        }
        #endregion

        #region Создать эскиз
        /// <summary>
        /// Создать эскиз
        /// </summary>
        /// <param name="imgPath">Путь к изображению</param>
        /// <returns></returns>
        public Bitmap GetThumb(string imgPath)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(imgPath))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(imgPath));
            #endregion
            Bitmap bmp;
            try
            {
                var img = Image.FromFile(imgPath);// метод держит файл
                                                  /*    FileStream fs = new FileStream(item, FileMode.Open);
                                                      Image img = Image.FromStream(fs);
                                                      fs.Close();*/
                decimal h = (img.Height * _configuration.ThumbnailWidth) / img.Width;

                if (h != 0)
                {
                    bmp = new Bitmap(img, _configuration.ThumbnailWidth, (int)h);
                }
                else { bmp = new Bitmap(img, img.Height, img.Width); }
            }
            catch (Exception)
            {
                _logger.Add(string.Format("Не удалось выполнить конвертацию {0}", imgPath));
                bmp = new Bitmap(1, 1);
            }

            return bmp;

        }
        #endregion

        #region Сохранить на диске Эскиз
        /// <summary>
        /// Сохранить на диске Эскиз
        /// </summary>
        /// <param name="targetRootDir">Корневая дирректоря программы</param>
        /// <param name="thumbDir">Дирректория с эскизами</param>
        /// <param name="bmp">Изображение</param>
        /// <param name="thumbName">Наименование эскиза</param>
        /// <returns></returns>
        public string SaveThumb(string targetRootDir, string thumbDir, Bitmap bmp, string thumbName)
        {

            #region Guard
            if (string.IsNullOrWhiteSpace(targetRootDir))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(targetRootDir));
            if (string.IsNullOrWhiteSpace(thumbDir))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(thumbDir));
            if (string.IsNullOrWhiteSpace(thumbName))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(thumbName));
            if (bmp == null)
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(bmp));
            #endregion
            string fullPath = "";
            string result = "";

            try
            {
                var dir_t = Path.Combine(_configuration.GetTargetImagePath(), targetRootDir, thumbDir);
                if (!File.Exists(Path.Combine(dir_t, thumbName)))
                {
                    if (!Directory.Exists(dir_t))
                    {
                        Directory.CreateDirectory(dir_t);
                    }
                    fullPath = Path.Combine(dir_t, thumbName);
                    bmp.Save(fullPath);
                }
                result = Path.Combine(targetRootDir, thumbDir, thumbName);
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе SaveThumb. {0}", e.Message));
                throw new Exception("Ошибка в методе SaveThumb");
            }


            return result;
        }
        #endregion

        #region Является ли файл картинкой
        /// <summary>
        /// Является ли файл картинкой
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public bool IsImage(string ext)
        {
            string[] ar = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return ar.Contains(ext.ToLower());
        }
        #endregion

        #region Заполнить информацию о папках
        /// <summary>
        /// Заполнить информацию о папках
        /// </summary>
        /// <param name="driveId">Ключ диска</param>
        /// <param name="pathDrive">Путь диска</param>
        /// <param name="CreateFolder">Функция создания</param>
        /// <returns>ModalResult Количество папок</returns>
        public MethodResult<int> FillDirectoriesInfo(int driveId, string pathDrive, Func<string, int, int> CreateFolder)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(pathDrive))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(pathDrive));
            if (driveId <= 0)
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(driveId));
            if (CreateFolder == null)
                throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(CreateFolder));

            #endregion

            var result = new MethodResult<int>(0);

            string[] directories = null;
            try
            {
                directories = Directory.GetDirectories(pathDrive, "*.*", SearchOption.AllDirectories);
            }
            catch (UnauthorizedAccessException e)
            {
                result.Success = false;
                result.Messages.Add(e.Message);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                result.Success = false;
                result.Messages.Add(e.Message);
            }
            int i = 0;

            try
            {
                foreach (var path in directories)
                {
                    int id = CreateFolder(path, driveId);
                    i++;
                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе FillDirectoriesInfo. {0}", e.Message));
                throw new Exception("Ошибка в методе FillDirectoriesInfo");
            }

            result.Result = i;
            return result;
        }
        #endregion

        #region Заполнить описание диска
        /// <summary>
        /// Заполнить описание диска
        /// </summary>
        /// <param name="drive">Диск Инфо</param>
        /// <param name="bytes">Байт массив</param>
        /// <param name="title">Наименование</param>
        /// <param name="id">Ключ диска</param>
        /// <returns></returns>
        public DriveX FillDrive(DriveX drive, byte[] bytes, string title, int id)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(title));
            if (id <= 0)
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(id));
            if (drive == null)
                throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(drive));

            #endregion

            try
            {
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    DriveInfo di = (DriveInfo)formatter.Deserialize(stream);
                    drive.title = title;
                    drive.driveInfo = di;
                    drive.id = id;

                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе FillDirectoriesInfo. {0}", e.Message));
                throw new Exception("Ошибка в методе FillDirectoriesInfo");
            }

            return drive;
        }
        #endregion

        #region Заполнить информацию о файлах в дирректории
        /// <summary>
        /// Заполнить информацию о файлах в дирректории
        /// </summary>
        /// <param name="driveId">Ключ Диска</param>
        /// <param name="pathDrive">Путь к диску</param>
        /// <param name="CreateFile">Функция создания</param>
        /// <returns>Количество</returns>
        public MethodResult<int> FillFilesInfo(int driveId, string pathDrive, Func<string, int, int> CreateFile)
        {

            #region Guard
            if (string.IsNullOrWhiteSpace(pathDrive))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(pathDrive));
            if (driveId <= 0)
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(driveId));
            if (CreateFile == null)
                throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(CreateFile));

            #endregion
            var result = new MethodResult<int>(0);
            string[] files = null;
            try
            {
                files = Directory.GetFiles(pathDrive, "*.*", SearchOption.AllDirectories);
            }
            catch (UnauthorizedAccessException e)
            {
                result.Success = false;
                result.Messages.Add(e.Message);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                result.Success = false;
                result.Messages.Add(e.Message);
            }
            int i = 0;
            try
            {
                foreach (var path in files)
                {
                    int id = CreateFile(path, driveId);
                    i++;
                }
                result.Result = i;
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе FillFilesInfo. {0}", e.Message));
                throw new Exception("Ошибка в методе FillFilesInfo");
            }


            return result;
        }
        #endregion

        #region Вернуть описание папки
        /// <summary>
        /// Вернуть описание папки
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns></returns>
        public DirectoryInfo GetDirectoryInfoByPath(string path)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(path));
            #endregion

            if (!System.IO.Directory.Exists(path))
            {
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(path));
            }
            var di = new DirectoryInfo(path);
            return di;
        }
        #endregion

        #region Вернуть описание файла
        /// <summary>
        /// Вернуть описание файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>FileInfo</returns>
        public FileInfo GetFileInfoByPath(string path)
        {
            #region Guard
            if (!File.Exists(path) || string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(path));
            }
            #endregion

            FileInfo fi = null;
            try
            {
                fi = new FileInfo(path);
            }
            catch (System.IO.FileNotFoundException e)
            {
                _logger.Add(e.Message);
            }
            return fi;
        }
        #endregion

        #region Вернуть картинку в binary
        /// <summary>
        /// Вернуть картинку в binary
        /// </summary>
        /// <param name="bmp">Изображение</param>
        /// <returns>Бинарное представление</returns>
        public byte[] GetImageData(Bitmap bmp)
        {
            #region Guard
            if (bmp == null)
            {
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(bmp));
            }
            #endregion
            byte[] bytes;
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    bmp.Save(memoryStream, ImageFormat.Jpeg);
                    bytes = memoryStream.GetBuffer();
                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetImageData. {0}", e.Message));
                throw new Exception("Ошибка в методе GetImageData");
            }

            return bytes;

        }
        #endregion

        #region Получить двоичное представление объекта
        public byte[] GetBinaryData<T>(T obj)
        {

            if (obj == null) return new byte[1];
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, obj);
                    return stream.GetBuffer();
                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetBinaryData. {0}", e.Message));
                throw new Exception("Ошибка в методе GetBinaryData");
            }

        }
        #endregion

        #region Получить объект из двоичного представления
        public T GetDataFromBinary<T>(byte[] data)
        {
            if (data == null || data.Count() <= 1) return default(T);

            try
            {
            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetDataFromBinary. {0}", e.Message));
                throw new Exception("Ошибка в методе GetDataFromBinary");
            }

        }

        #endregion










    }
}
