using HomeArchiveX.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Infrastructure.Utilites;
using XFilesArchive.Model;

namespace XFilesArchive.Infrastructure.DataManager
{
    /// <summary>
    /// Класс для работы с данными SQL Server
    /// </summary>
    public class DataManager : IDataManager
    {
        private Dictionary<string, int> _directoryCash;
        private Dictionary<string, int> _imagesInDirectory;
        readonly int MAX_IMAGES_IN_DIRECTORY;
        const string ERROR_ARGUMENT_EXCEPTION_MSG = "Не верно указан параметр";
        private IConfiguration _configuration;
        ILogger _logger;
        IFIleManager _fileManager;
        public ILogger logger { get { return _logger; } }
        #region Конструктор
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="fileManager">Файл Менеджер</param>
        /// <param name="logger">Логгер</param>
        public DataManager(IConfiguration configuration, IFIleManager fileManager
            , ILogger logger, int maxImagesInDirectory = 99)
        {
            #region Guard
            if (configuration == null) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(configuration));
            if (fileManager == null) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(fileManager));
            if (logger == null) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(logger));
            if (maxImagesInDirectory < 0)
                throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(maxImagesInDirectory));
            #endregion

            _configuration = configuration;
            _logger = logger;
            _fileManager = fileManager;
            _directoryCash = new Dictionary<string, int>();
            _imagesInDirectory = new Dictionary<string, int>();
            MAX_IMAGES_IN_DIRECTORY = maxImagesInDirectory;
        }
        #endregion

        #region ClearCash
        public void ClearCash()
        {
            _directoryCash.Clear();
        }
        #endregion

        #region Создать описание папки
        /// <summary>
        /// Создать в БД запись с описанием папки, дирректории
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="driveId">Ключ</param>
        /// <returns></returns>
        public int CreateFolder(string path, int driveId)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(path));
            if (driveId <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(driveId));
            #endregion

            try
            {
                var di = _fileManager.GetDirectoryInfoByPath(path);
                var parentPath = di.Parent == null || di.Root.FullName == di.Parent.FullName ? null : di.Parent.FullName;
                int parentId = GetEntityIdByPath(parentPath, driveId, EntityType.Folder);
                #region Set Directory Info Dictionary
                var diDict = new Dictionary<string, string>();
                diDict.Add("CreationTime", string.Format("{0:dd.MM.yyyy hh:mm:ss}", di.CreationTime));
                diDict.Add("LastWriteTime", string.Format("{0:dd.MM.yyyy hh:mm:ss}", di.LastWriteTime));
                diDict.Add("Extension", di.Extension);
                diDict.Add("FullName", di.FullName);
                diDict.Add("Name", di.Name);
                #endregion

                var id = CreateArchiveEntity<Dictionary<string, string>>(
                    driveId, diDict, di.Name, 0, parentId, EntityType.Folder, path, "", "", "");
                _directoryCash.Add(path, id);
                return id;
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе CreateFolder. {0}", e.Message));
                throw new Exception("Ошибка в методе CreateFolder");
            }


        }
        #endregion

        #region Создать описание файла
        /// <summary>
        /// Создать в БД Запись с описанием файла
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="driveId">Ключ диска</param>
        /// <returns></returns>
        public int CreateFile(string path, int driveId)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(path));
            if (driveId <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(driveId));
            #endregion
            try
            {
                FileInfo fi = _fileManager.GetFileInfoByPath(path);
                int parentId = GetEntityIdByPath(fi.Directory.Parent == null ? null : fi.Directory.FullName
                    , driveId, EntityType.Folder);

                var fiDict = new Dictionary<string, string>();
                fiDict.Add("CreationTime", string.Format("{0:dd.MM.yyyy hh:mm:ss}", fi.CreationTime));
                fiDict.Add("LastWriteTime", string.Format("{0:dd.MM.yyyy hh:mm:ss}", fi.LastWriteTime));
                fiDict.Add("Extension", fi.Extension);
                fiDict.Add("FullName", fi.FullName);
                fiDict.Add("Name", fi.Name);
                fiDict.Add("Length", string.Format("{0}", fi.Length));
                var checksum = Utilites.Security.ComputeMD5Checksum(path);
                var id = CreateArchiveEntity<Dictionary<string, string>>(driveId, fiDict, fi.Name, (int)fi.Length, parentId
                    , EntityType.File, path, fi.Extension, "", checksum);
                return id;
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе CreateFile. {0}", e.Message));
                throw new Exception("Ошибка в методе CreateFile");
            }
        }
        #endregion

        #region Создать связь картинку - сущность
        /// <summary>
        /// Создать в БД запись - связь картинка - сущность
        /// </summary>
        /// <param name="ImagePath">Путь к картинке</param>
        /// <param name="entityId">Ключ сущности</param>
        /// <param name="driveId">Ключ диска</param>
        public void CreateImageToEntity(string ImagePath, int entityId, int driveId)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(ImagePath)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(ImagePath));
            if (driveId <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(driveId));
            if (entityId <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(entityId));
            #endregion

            try
            {
                int imageId = CreateImage(ImagePath, string.Format(@"drive{0}\img{1}", driveId, entityId));
                string queryString = @"insert into ImageToEntity(TargetEntityKey,ImageKey) values (@TargetEntityKey,@ImageKey)";

                using (SqlConnection ce = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var command = new SqlCommand(queryString, ce);
                    command.Parameters.Clear();
                    command.Parameters.Add("@TargetEntityKey", SqlDbType.Int);
                    command.Parameters.Add("@ImageKey", SqlDbType.Int);
                    command.Parameters["@TargetEntityKey"].Value = entityId;
                    command.Parameters["@ImageKey"].Value = imageId;
                    ce.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе CreateImageToEntity. {0}", e.Message));
                throw new Exception("Ошибка в методе CreateImageToEntity");
            }

        }
        #endregion

        #region Создать запись об изображении
        /// <summary>
        /// Создать запись об изображении
        /// </summary>
        /// <param name="imagePath">Путь к изопражению</param>
        /// <param name="targetDir">Путь назначение</param>
        /// <returns>Ключ рисунка</returns>
        public int CreateImage(string imagePath, string targetDir)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(imagePath)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(imagePath));
            if (string.IsNullOrWhiteSpace(targetDir)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(targetDir));

            #endregion

            try
            {
                var imgInfo = new FileInfo(imagePath);
                var imgDirPath = imgInfo.Directory.FullName;
                int imgCount = 0;
                imgCount = GetImgCountInDirectory(imgDirPath, imgCount);

                string newImgPath = "";
                string queryString = @"insert into Image( Thumbnail,ImagePath,ThumbnailPath,ImageTitle,HashCode) 
values (@Thumbnail,@ImagePath,@ThumbnailPath,@ImageTitle,@HashCode);
                select SCOPE_IDENTITY();";

                byte[] imageData = null;
                try
                {
                    if (!(imgCount > MAX_IMAGES_IN_DIRECTORY) && MAX_IMAGES_IN_DIRECTORY != 0 && imgCount != 0)
                    {
                        newImgPath = _fileManager.CopyImg(imagePath, targetDir);
                    }

                }

                // Catch exception if the file was already copied.
                catch (IOException copyError)
                {
                    _logger.Add(copyError.Message);
                }

                Bitmap bmp = _fileManager.GetThumb(imagePath);
                string thumbPath = _fileManager.SaveThumb(targetDir, _configuration.GetThumbDirName(), bmp, imgInfo.Name);
                imageData = _fileManager.GetImageData(bmp);

                using (SqlConnection ce = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var command = new SqlCommand(queryString, ce);
                    command.Parameters.Clear(); //Thumbnail,
                    command.Parameters.Add("@HashCode", SqlDbType.Int);
                    command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 255);
                    command.Parameters.Add("@ThumbnailPath", SqlDbType.NVarChar, 255);
                    command.Parameters.Add("@ImageTitle", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@Thumbnail", SqlDbType.Image);

                    command.Parameters["@HashCode"].Value = imgInfo.GetHashCode();
                    command.Parameters["@ImagePath"].Value = newImgPath;
                    command.Parameters["@ThumbnailPath"].Value = thumbPath;
                    command.Parameters["@ImageTitle"].Value = imgInfo.Name;
                    command.Parameters["@Thumbnail"].Value = imageData;
                    ce.Open();
                    string strid = command.ExecuteScalar().ToString();
                    return int.Parse(strid);
                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе CreateImageToEntity. {0}", e.Message));
                throw new Exception("Ошибка в методе CreateImageToEntity");
            }
        }

        /// <summary>
        /// Получить количество картинок в дирректории
        /// </summary>
        /// <param name="imgDirPath"></param>
        /// <param name="imgCount"></param>
        /// <returns></returns>
        private int GetImgCountInDirectory(string imgDirPath, int imgCount)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(imgDirPath)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(imgDirPath));
            // if (imgCount <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(imgCount));
            #endregion

            try
            {
                if (_imagesInDirectory.Keys.Contains(imgDirPath))
                {
                    _imagesInDirectory.TryGetValue(imgDirPath, out imgCount);
                    imgCount++;
                    _imagesInDirectory[imgDirPath] = imgCount;
                }
                else
                {
                    _imagesInDirectory.Add(imgDirPath, 1);
                }

                return imgCount;
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetImgCountInDirectory. {0}", e.Message));
                throw new Exception("Ошибка в методе GetImgCountInDirectory ");
            }
        }
        #endregion

        #region Получить источник по ИД
        /// <summary>
        /// Получить источник по ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DriveX GetDriveById(int id)
        {
            #region Guard
            if (id <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(id));
            #endregion

            try
            {
                DriveX drive = new DriveX();
                string queryString = "SELECT Title,DriveInfo FROM Drive where DriveId=@id;";

                using (SqlConnection ce = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var command = new SqlCommand(queryString, ce);
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("id", id);
                    ce.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            byte[] bytes = (byte[])reader[1];
                            drive = _fileManager.FillDrive(drive, bytes, (string)reader[0], id);
                        }
                    }
                }
                return drive;
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе . {0}", e.Message));
                throw new Exception("Ошибка в методе ");
            }
        }
        #endregion

        #region Вернуть ИД описания Файла, папки
        public int GetEntityIdByPath(string path, int driveId, EntityType entityType)
        {
            #region Guard
            if (driveId <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(driveId));
            if (path == null) return -1;
            #endregion

            if (_directoryCash.ContainsKey(path)) return _directoryCash[path];

            try
            {
                int id = 0;
                string queryString = @"select ArchiveEntityKey from ArchiveEntity where 
                                    DriveId=@DriveId and EntityPath=@EntityPath";
                using (SqlConnection ce = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var command = new SqlCommand(queryString, ce);
                    command.Parameters.Clear();
                    command.Parameters.Add("@DriveId", SqlDbType.Int);
                    command.Parameters.Add("@EntityPath", SqlDbType.NVarChar, 250);

                    command.Parameters["@DriveId"].Value = driveId;
                    command.Parameters["@EntityPath"].Value = path;
                    ce.Open();

                    var execResult = command.ExecuteScalar();

                    string strid = execResult == null ? "" : execResult.ToString();

                    int.TryParse(strid, out id);

                }
                if (id != 0)
                {
                    return id;
                }
                switch (entityType)
                {
                    case EntityType.Folder:
                        id = CreateFolder(path, driveId);
                        break;
                    case EntityType.File:
                        id = CreateFile(path, driveId);
                        break;
                    default:
                        break;
                }
                return id;
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetEntityIdByPath. {0}", e.Message));
                throw new Exception("Ошибка в методе GetEntityIdByPath");
            }
        }
        #endregion

        #region Создать запись об источнике
        /// <summary>
        /// Создать запись об источнике
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="title">Описание</param>
        /// <returns></returns>
        public int CreateDrive(string path, string title, string diskCode, Dictionary<string, object> addParams = null)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(path));
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(title));
            if (string.IsNullOrWhiteSpace(diskCode)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(diskCode));
            #endregion
            try
            {
                #region IsSecret
                object addParamObj;
                byte? IsSecret = 0;
                if (addParams != null)
                {
                    if (addParams.Keys.Contains("IsSecret"))
                    {
                        addParams.TryGetValue("IsSecret", out addParamObj);
                        IsSecret = addParamObj as byte?;
                    }
                }
                #endregion



                var di = new DriveInfo(path);
                if (!di.IsReady)
                {
                    _logger.Add("Устройство не готово для чтения");
                    return 0;
                }
                var hashCode = di.TotalSize.GetHashCode() ^ di.VolumeLabel.GetHashCode() ^ di.TotalFreeSpace.GetHashCode();

                //TODO: Изменить Алгоритм Срочно!!!

                var driveExist = IsDriveExist(hashCode, title);
                if (driveExist > 0)
                {
                    _logger.Add("Диск с таким хешем или наименованием существует");
                    return 0;
                }

                string queryString = @"insert into Drive(Title, HashCode, DriveInfo,DriveCode,IsSecret) 
values (@Title, @HashCode, @DriveInfo,@DriveCode,@IsSecret);
                select SCOPE_IDENTITY();";
                using (SqlConnection ce = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var command = new SqlCommand(queryString, ce);
                    command.Parameters.Clear();
                    command.Parameters.Add("@Title", SqlDbType.NVarChar, 100);
                    command.Parameters.Add("@HashCode", SqlDbType.Int);
                    command.Parameters.Add("@DriveInfo", SqlDbType.VarBinary, Int32.MaxValue);
                    command.Parameters.Add("@DriveCode", SqlDbType.NVarChar, 20);
                    command.Parameters.Add("@IsSecret", SqlDbType.Bit);

                    command.Parameters["@DriveInfo"].Value = _fileManager.GetBinaryData<DriveInfo>(di);
                    command.Parameters["@Title"].Value = title;
                    command.Parameters["@HashCode"].Value = hashCode;
                    command.Parameters["@DriveCode"].Value = diskCode;
                    command.Parameters["@IsSecret"].Value = IsSecret;
                    ce.Open();
                    // command.ExecuteNonQuery();
                    string strid = command.ExecuteScalar().ToString();

                    return int.Parse(strid);
                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе CreateDrive. {0}", e.Message));
                throw new Exception("Ошибка в методе CreateDrive");
            }
        }
        #endregion

        #region Проверка сужествования диска по Наименованию, Хешу
        /// <summary>
        /// Проверка сужествования диска по Наименованию, Хешу
        /// </summary>
        /// <param name="hashCode">Хеш Код</param>
        /// <param name="title">Наименование</param>
        /// <returns></returns>
        public int IsDriveExist(int hashCode, string title)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(title));
            if (hashCode == 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(hashCode));
            #endregion

            try
            {
                string queryString = @"
select (SELECT count(1) FROM Drive where 
Title=@title)+case when ( SELECT count(1) FROM Drive 
where  HashCode = @HashCode)>0 then 2 else 0 end vl";
                using (SqlConnection ce = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var command = new SqlCommand(queryString, ce);
                    command.Parameters.Clear();
                    command.Parameters.Add("@Title", SqlDbType.NVarChar, 100);
                    command.Parameters.Add("@HashCode", SqlDbType.Int);

                    command.Parameters["@Title"].Value = title;
                    command.Parameters["@HashCode"].Value = hashCode;

                    ce.Open();

                    string strid = command.ExecuteScalar().ToString();

                    return int.Parse(strid);
                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе IsDriveExist. {0}", e.Message));
                throw new Exception("Ошибка в методе IsDriveExist");
            }
        }
        #endregion

        #region Очистить БД
        /// <summary>
        /// Очистить БД
        /// </summary>
        public void TruncateTables()
        {
            try
            {
                string sqlExpression = "TruncateTables";
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    connection.Open();
                    var command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.ExecuteScalar();

                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе TruncateTables. {0}", e.Message));
                throw new Exception("Ошибка в методе TruncateTables");
            }
        }
        #endregion

        #region Создать запись о файле, папке
        /// <summary>
        /// Создать запись о файле, папке
        /// </summary>
        /// <typeparam name="T">Тип DirectoryInfo , FileInfo</typeparam>
        /// <param name="driveId">Ид Источника</param>
        /// <param name="entity">Объект</param>
        /// <param name="title">Имя файла, папки</param>
        /// <param name="hashCode">Хеш</param>
        /// <param name="parentEntityKey">ИД предка</param>
        /// <param name="entityType"></param>
        /// <param name="entityPath">Полный путь</param>
        /// <param name="extension">расшрение</param>
        /// <param name="description">описание</param>
        /// <returns></returns>
        public int CreateArchiveEntity<T>(int driveId, T entity, string title,
           int fileSize, int parentEntityKey, EntityType entityType, string entityPath, string extension, string description,
           string checksum)
        {

            #region Guard
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(title));
            if (string.IsNullOrWhiteSpace(entityPath)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(entityPath));
            if (driveId <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(driveId));
            //if (FileSize <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(FileSize));
            #endregion


            try
            {
                string queryString = @"insert into ArchiveEntity( 
                ParentEntityKey,DriveId,Title,EntityType ,EntityPath,EntityExtension ,Description
,FileSize ,EntityInfo, MFileInfo,Checksum)
values ( 
@ParentEntityKey,@DriveId,@Title,@EntityType,@EntityPath,@EntityExtension,@Description
,@FileSize,@EntityInfo,@MFileInfo,@Checksum);
                                     select SCOPE_IDENTITY();";
                using (SqlConnection ce = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var command = new SqlCommand(queryString, ce);
                    command.Parameters.Clear();

                    command.Parameters.Add("@ParentEntityKey", SqlDbType.Int);

                    command.Parameters.Add("@DriveId", SqlDbType.Int);
                    command.Parameters.Add("@EntityType", SqlDbType.Int);
                    command.Parameters.Add("@EntityPath", SqlDbType.NVarChar, 100);
                    command.Parameters.Add("@EntityExtension", SqlDbType.NVarChar, 20);
                    command.Parameters.Add("@Description", SqlDbType.NVarChar, 100);
                    command.Parameters.Add("@FileSize", SqlDbType.Int);
                    command.Parameters.Add("@EntityInfo", SqlDbType.VarBinary, Int32.MaxValue);
                    command.Parameters.Add("@MFileInfo", SqlDbType.VarBinary, Int32.MaxValue);
                    command.Parameters.Add("@Title", SqlDbType.NVarChar, 250);
                    command.Parameters.Add("@Checksum", SqlDbType.NVarChar, 250);
                    var mfi = MFIFactory.GetMediaFileInfoDictionary(extension, entityPath);

                    command.Parameters["@MFileInfo"].Value = _fileManager.GetBinaryData<Dictionary<string, string>>(mfi);
                    command.Parameters["@EntityInfo"].Value = _fileManager.GetBinaryData<T>(entity);
                    command.Parameters["@Title"].Value = title;
                    command.Parameters["@FileSize"].Value = fileSize;
                    command.Parameters["@Checksum"].Value = checksum;
                    if (parentEntityKey != -1)
                    {
                        command.Parameters["@ParentEntityKey"].Value = parentEntityKey;
                    }
                    else
                    {
                        command.Parameters["@ParentEntityKey"].Value = DBNull.Value;
                    }
                    command.Parameters["@DriveId"].Value = driveId;
                    command.Parameters["@EntityType"].Value = (int)entityType;
                    command.Parameters["@EntityPath"].Value = entityPath;
                    command.Parameters["@EntityExtension"].Value = extension;
                    command.Parameters["@Description"].Value = description;
                    ce.Open();
                    //  command.ExecuteNonQuery();
                    string strid = command.ExecuteScalar().ToString();
                    if (_fileManager.IsImage(extension))
                    {
                        CreateImageToEntity(entityPath, int.Parse(strid), driveId);
                    }
                    return int.Parse(strid);
                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе . {0}", e.Message));
                throw new Exception("Ошибка в методе ");
            }
        }
        #endregion
        /*-------------------------------*/

        #region FillDirectoriesInfo
        public void FillDirectoriesInfo(int driveId, string pathDrive)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(pathDrive)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(pathDrive));
            if (driveId <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(driveId));
            #endregion
            MethodResult<int> result = _fileManager.FillDirectoriesInfo(driveId, pathDrive, CreateFolder);
        }
        #endregion

        #region FillFilesInfo
        public void FillFilesInfo(int driveId, string pathDrive)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(pathDrive)) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(pathDrive));
            if (driveId <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(driveId));
            #endregion
            MethodResult<int> result = _fileManager.FillFilesInfo(driveId, pathDrive, CreateFile);
        }
        #endregion

        #region GetImageById
        public System.Drawing.Image GetImageById(int id)
        {
            #region Guard
            if (id <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(id));
            #endregion
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    connection.Open();
                    string sql = "select Thumbnail FROM [dbo].[Image] where imageKey=@imageKey";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("imageKey", id);
                    connection.Open();
                    object obj = command.ExecuteScalar();
                    return _fileManager.GetDataFromBinary<System.Drawing.Image>((byte[])obj);

                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetImageById. {0}", e.Message));
                throw new Exception("Ошибка в методе GetImageById");
            }
        }
        #endregion

        #region Все диски
        public string[] GetDrives()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var result = new List<string>();
                    connection.Open();
                    string sql = " select rtrim(ltrim(str(DriveId))+'. '+Title) descr FROM Drive descr ";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }

                    return result.ToArray();

                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе . {0}", e.Message));
                throw new Exception("Ошибка в методе ");
            }
        }
        #endregion

        #region Получить Дирректории
        public string[] GetDirectories(int id)
        {
            #region Guard
            if (id <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(id));
            #endregion

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var result = new List<string>();
                    connection.Open();
                    string sql = @"select rtrim(ltrim(str(ArchiveEntityKey)))+'. '+EntityPath descr 
                               from ArchiveEntity where EntityType=1 and DriveId = @id";
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
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetDirectories. {0}", e.Message));
                throw new Exception("Ошибка в методе GetDirectories");
            }
        }
        #endregion

        #region Получить файлы
        public string[] GetFiles(int id)
        {
            #region Guard
            if (id <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(id));
            #endregion

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var result = new List<string>();
                    connection.Open();
                    string sql = @"select rtrim(ltrim(str(ArchiveEntityKey)))+'. '+EntityPath descr 
                               from ArchiveEntity where EntityType=2 and DriveId = @id";
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
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetFiles. {0}", e.Message));
                throw new Exception("Ошибка в методе GetFiles");
            }
        }
        #endregion

        #region Получить Ключи файлов по ИД Диска 
        public int[] GetFilesByDestinationKey(int driveId = 0)
        {
            #region Guard
            //  if (driveId <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(driveId));
            #endregion

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var result = new List<int>();
                    connection.Open();
                    string addWhere = "";
                    if (driveId > 0)
                    {
                        addWhere = "and DriveId = @id";
                    }

                    string sql = @"select ltrim(str(ArchiveEntityKey)) from ArchiveEntity where EntityType=2 " + addWhere;
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Clear();
                    if (driveId > 0)
                    {
                        command.Parameters.AddWithValue("id", driveId);
                    }

                    SqlDataReader reader = command.ExecuteReader();
                    int key = 0;

                    while (reader.Read())
                    {
                        int.TryParse(reader.GetString(0), out key);
                        if (key > 0)
                        {
                            result.Add(key);
                        }
                    }

                    return result.ToArray();

                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetFiles. {0}", e.Message));
                throw new Exception("Ошибка в методе GetFiles");
            }
        }
        #endregion

        #region GetDriveInfoById Описание диска по ИД
        public DriveInfo GetDriveInfoById(int id)
        {
            #region Guard
            if (id <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(id));
            #endregion

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    connection.Open();
                    string sql = "select DriveInfo FROM Drive where DriveId=@id";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("id", id);

                    object obj = command.ExecuteScalar();
                    return _fileManager.GetDataFromBinary<DriveInfo>((byte[])obj);

                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetDriveInfoById . {0}", e.Message));
                throw new Exception("Ошибка в методе GetDriveInfoById ");
            }
        }
        #endregion

        #region GetFileInfoById Информация о файле по ИД
        public Dictionary<string, string> GetFileInfoById(int id)
        {
            #region Guard
            if (id <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(id));
            #endregion
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    connection.Open();
                    string sql = "select EntityInfo FROM ArchiveEntity where ArchiveEntityKey=@id and EntityType=2";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("id", id);

                    object obj = command.ExecuteScalar();
                    if (obj is System.DBNull) return default(Dictionary<string, string>);
                    return _fileManager.GetDataFromBinary<Dictionary<string, string>>((byte[])obj);

                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetFileInfoById. {0}", e.Message));
                throw new Exception("Ошибка в методе GetFileInfoById");
            }
        }
        #endregion

        #region GetMediaFileInfoById Медиа информация о файле.
        public Dictionary<string, string> GetMediaFileInfoById(int id)
        {
            #region Guard
            if (id <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(id));
            #endregion
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    connection.Open();
                    string sql = "select MFileInfo FROM ArchiveEntity where ArchiveEntityKey=@id and EntityType=2";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("id", id);

                    object obj = command.ExecuteScalar();

                    if (obj is System.DBNull) return new Dictionary<string, string>();
                    return _fileManager.GetDataFromBinary<Dictionary<string, string>>((byte[])obj);

                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetMediaFileInfoById. {0}", e.Message));
                throw new Exception("Ошибка в методе GetMediaFileInfoById");
            }
        }
        #endregion

        #region GetDirectoryInfoById Информация о дирректории по Ид
        public Dictionary<string, string> GetDirectoryInfoById(int id)
        {
            #region Guard
            if (id <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(id));
            #endregion

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    connection.Open();
                    string sql = "select EntityInfo FROM ArchiveEntity where ArchiveEntityKey=@id and EntityType=1";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("id", id);

                    object obj = command.ExecuteScalar();
                    if (obj is System.DBNull) return default(Dictionary<string, string>);
                    return _fileManager.GetDataFromBinary<Dictionary<string, string>>((byte[])obj);

                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetDirectoryInfoById. {0}", e.Message));
                throw new Exception("Ошибка в методе GetDirectoryInfoById");
            }
        }
        #endregion

        #region GetExiststingFilesByHashCode Информация о Существующих файлах
        public Dictionary<int, string> GetExiststingFilesByHashCode(int hashCode)
        {
            #region Guard
            if (hashCode == 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(hashCode));
            #endregion

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    connection.Open();
                    string sql = "select ArchiveEntityKey,Title FROM ArchiveEntity where where HashCode=@hashCode and EntityType=2";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("hashCode", hashCode);

                    SqlDataReader reader = command.ExecuteReader();

                    var result = new Dictionary<int, string>();


                    while (reader.Read())
                    {
                        int tmp = 0;
                        int.TryParse(reader.GetString(0), out tmp);
                        result.Add(tmp, reader.GetString(1));
                    }

                    return result;

                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetDirectoryInfoById. {0}", e.Message));
                throw new Exception("Ошибка в методе GetDirectoryInfoById");
            }
        }
        #endregion
        //*********************
        #region SetFileSizeByKeys
        public void SetFileSizeByKeys(int driveId = 0)
        {
            int[] keys = GetFilesByDestinationKey(driveId);
            foreach (var ArchiveEntityKey in keys)
            {
                Dictionary<string, string> info = GetFileInfoById(ArchiveEntityKey);

                if (info.ContainsKey("FileSize"))
                {
                    int fileSize = 0;
                    string strFileSize = "";
                    info.TryGetValue("FileSize", out strFileSize);
                    int.TryParse(strFileSize, out fileSize);
                    if (fileSize > 0)
                    {
                        SetFileSize(ArchiveEntityKey, fileSize);
                    }

                }
            }



        }

        #endregion
        #region SetFileSize
        public void SetFileSize(int archiveEntityKey, int fileSize)
        {
            #region Guard
            if (archiveEntityKey <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(archiveEntityKey));
            if (fileSize <= 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(fileSize));
            #endregion

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var result = new List<string>();
                    connection.Open();
                    string sql = @"update ArchiveEntity set  fileSize=@fileSize
                               from ArchiveEntity where  ArchiveEntityKey = @archiveEntityKey";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("archiveEntityKey", archiveEntityKey);
                    command.Parameters.AddWithValue("fileSize", fileSize);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetFiles. {0}", e.Message));
                throw new Exception("Ошибка в методе GetFiles");
            }
        }

        #endregion
        #region CheckFilesByHashOrTitle
        public string[] CheckFilesByHashOrTitle(int fileSize, string checksum, string title)
        {
            #region Guard
            if (fileSize == 0) throw new ArgumentNullException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(fileSize));
            #endregion

            try
            {

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString()))
                {
                    var result = new List<string>();
                    connection.Open();
                    string sql = @"  
                                    select rtrim(ltrim(str(ae.ArchiveEntityKey)))+'::'+rtrim(ae.Title)+'::'+d.Title+'::'+d.DriveCode
                                 from ArchiveEntity ae 
								 join Drive d on ae.DriveId=d.DriveId
								 where ae.EntityType=2 and ae.Checksum=@Checksum
								 union all
								 select * from(select '=============by Filesize============' fld
								 union all
                                    select rtrim(ltrim(str(ae.ArchiveEntityKey)))+'::'+rtrim(ae.Title)+'::'+d.Title+'::'+d.DriveCode
                                 from ArchiveEntity ae 
								 join Drive d on ae.DriveId=d.DriveId
								 where ae.EntityType=2 and ae.FileSize=@FileSize)z where exists (select 1 from ArchiveEntity
								 where EntityType=2 and FileSize=@FileSize )
								 union all
								 select * from (select '=============by name============' fld
								 union all
								 select rtrim(ltrim(str(ae.ArchiveEntityKey)))+'::'+rtrim(ae.Title)+'::'+d.Title+'::'+d.DriveCode
                                 from ArchiveEntity ae 
								 join Drive d on ae.DriveId=d.DriveId
								 where ae.EntityType=2 and ae.FileSize=@FileSize and ae.Title=@title) as x 
								 where exists (select 1 from ArchiveEntity
								 where EntityType=2 and FileSize=@FileSize and Title=@title)";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("Checksum", checksum);
                    command.Parameters.AddWithValue("FileSize", fileSize);
                    command.Parameters.AddWithValue("title", title);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }

                    return result.ToArray();

                }
            }
            catch (Exception e)
            {
                _logger.Add(string.Format("Ошибка в методе GetFiles. {0}", e.Message));
                throw new Exception("Ошибка в методе GetFiles");
            }
        }

        #endregion

        #region BulkCopy
        public void BulkCopyArchiveEntity(IEnumerable<DestinationItem> items, int DriveId)
        {
            var cs = _configuration.GetConnectionString();
            var table = new DataTable();
            using (SqlConnection sc = new SqlConnection(cs))
            {
                sc.Open();
                using (var adapter = new SqlDataAdapter($"SELECT TOP 0 * FROM ArchiveEntity", sc))
                {
                    adapter.Fill(table);
                };
                foreach (var item in items)
                {
                    var row = table.NewRow();
                    row["EntityExtension"] = item.EntityExtension;
                    row["EntityPath"] = item.EntityPath;
                    row["EntityType"] = item.EntityType;
                    row["FileSize"] = item.FileSize ?? 0;
                    row["ParentGuid"] = item.ParentGuid.ToString();
                    row["Title"] = item.Title;
                    row["UniqGuid"] = item.UniqGuid.ToString();
                    row["CreatedDate"] = DateTime.Now;
                    row["DriveId"] = DriveId;
                    if (item.EntityType == 2)
                    {
                        row["Checksum"] = item.Checksum;
                    }
                    table.Rows.Add(row);
                }
                using (var bulk = new SqlBulkCopy(sc))
                {
                    bulk.DestinationTableName = "ArchiveEntity";
                    bulk.WriteToServer(table);
                }
                string sql = @"update a1 set a1.[ParentEntityKey] = a2.[ArchiveEntityKey]
  FROM [HmeArhX].[dbo].[ArchiveEntity] a1
  left join [HmeArhX].[dbo].[ArchiveEntity] a2 on a1.[ParentGuid] = a2.[UniqGuid]
   where a1.[DriveId]= @DriveId";
                SqlCommand command = new SqlCommand(sql, sc);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("DriveId", DriveId);
                command.ExecuteNonQuery();
                sc.Close();
            }
        }

        public void BulkCopyImage(IEnumerable<ImageDto> items)
        {
            var cs = _configuration.GetConnectionString();
            using (SqlConnection sc = new SqlConnection(cs))
            {
                sc.Open();
                #region  Image
                var ImageTable = new DataTable();

                using (var adapter = new SqlDataAdapter($"SELECT TOP 0 * FROM Image", sc))
                {
                    adapter.Fill(ImageTable);
                };

                foreach (var item in items)
                {
                    var row = ImageTable.NewRow();
                    row["UniqGuid"] = item.UniqGuid;
                    row["HashCode"] = item.HashCode;
                    row["ImagePath"] = item.ImagePath;
                    row["ImageTitle"] = item.ImageTitle;
                    row["Thumbnail"] = item.Thumbnail;
                    row["ThumbnailPath"] = item.ThumbnailPath;
                    row["CreatedDate"] = DateTime.Now;
                    ImageTable.Rows.Add(row);
                }

                using (var bulk = new SqlBulkCopy(sc))
                {
                    bulk.DestinationTableName = "Image";
                    bulk.WriteToServer(ImageTable);
                }
                #endregion

                #region ImageToEntity
                var ImageUniqGuidTable = new DataTable();

                

                using (var adapter = new SqlDataAdapter($"SELECT TOP 0 UniqGuid FROM [Image]", sc))
                {
                    adapter.Fill(ImageTable);
                };

                foreach (var item in items)
                {
                    var row = ImageUniqGuidTable.NewRow();
                    row["UniqGuid"] = item.UniqGuid;
                    ImageUniqGuidTable.Rows.Add(row);
                }

                SqlCommand cmd = new SqlCommand(@"if object_id('tempdb..##ImageUniqGuid') is not null drop table ##ImageUniqGuid
create table ##ImageUniqGuid (UniqGuid uniqueidentifier)", sc);
                cmd.ExecuteNonQuery();
                using (var bulk = new SqlBulkCopy(sc))
                {
                    bulk.DestinationTableName = "##ImageUniqGuid";
                    bulk.WriteToServer(ImageTable);
                }

                cmd = new SqlCommand(@"if object_id('tempdb..##ImageUniqGuid') is not null drop table ##ImageUniqGuid", sc);
                cmd.ExecuteNonQuery();
                #endregion

                string sql = @"
insert into ImageToEntity(TargetEntityKey,ImageKey)
select i.ImageKey ,a.ArchiveEntityKey
from [Image] i 
join ##Image m on i.UniqGuid = m.UniqGuid
join [dbo].[ArchiveEntity] a on a.UniqGuid = m.UniqGuid ";
                SqlCommand command = new SqlCommand(sql, sc);
                command.Parameters.Clear();
                // command.Parameters.AddWithValue("DriveId", DriveId);
                command.ExecuteNonQuery();
                sc.Close();
            }
        }
        #endregion

        #region SetMinfo
        public void SetMinfo(Guid uniqGuid, byte[] bMinfo)
        {
            var cs = _configuration.GetConnectionString();
            using (SqlConnection sc = new SqlConnection(cs))
            {
                sc.Open();
                string sql = @"update ArchiveEntity set MFileInfo=@MFileInfo 
                  where UniqGuid=@UniqGuid and EntityType=2";
                SqlCommand command = new SqlCommand(sql, sc);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("MFileInfo", bMinfo);
                command.Parameters.AddWithValue("UniqGuid", uniqGuid);
                object obj = command.ExecuteNonQuery();
                sc.Close();
            }
        }
        #endregion


    }
}
