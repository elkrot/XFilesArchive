using System;
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.Deployment.WindowsInstaller;

namespace SqlActionLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomActions
    {
        #region Public Members

        /// <summary>
        /// Выполняет проверку подключения к SQL-серверу.
        /// </summary>
        /// <param name="session">Объект сессии, который контролирует процесс установки.</param>
        /// <returns>Возвращает статус выполнения пользовательского действия.</returns>
        [CustomAction]
        public static ActionResult SqlConnect(Session session)
        {
            session.Log("Begin SqlConnect");
            string connectionString;
            // Получаем тип входа
            var logonType = session["DATABASE_LOGON_TYPE"];
            if (logonType.Equals("DatabaseIntegratedAuth"))
            {
                // Доверенные (проверка подлинности Windows)
                connectionString = String.Format("Data Source={0};Integrated Security=SSPI", session["DATABASE_SERVER"]);

            }
            else if (logonType.Equals("DatabaseAccount"))
            {
                // Задать имя пользователя и пароль (проверка подлинности SQL)
                connectionString = String.Format("Data Source={0};User Id={1}; Password={2}", session["DATABASE_SERVER"], session["DATABASE_USERNAME"], session["DATABASE_PASSWORD"]);
            }
            else
            {
                // Не известный тип входа
                session["CONNECTION_ESTABLISHED"] = "";
                session.Log("SqlConnect is not successful");
                return ActionResult.NotExecuted;
            }
            // Выполняем подключение к SQL-серверу
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                }
                session["CONNECTION_ESTABLISHED"] = "1";
                session.Log("SqlConnect is successful");
                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                session["CONNECTION_ESTABLISHED"] = "";
                session["ERROR"] = ex.Message;
                session.Log(ex.Message);
                return ActionResult.NotExecuted;
            }

        }

        /// <summary>
        /// Получает список доступных SQL-серверов.
        /// </summary>
        /// <param name="session">Объект сессии, который контролирует процесс установки.</param>
        /// <returns>Возвращает статус выполнения пользовательского действия.</returns>
        [CustomAction]
        public static ActionResult GetSqlServers(Session session)
        {
            try
            {
                // Получаем список SQL-серверов
                var sqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance;
                var servers = sqlDataSourceEnumerator.GetDataSources().Rows;
                // Получаем список существующих значений
                var exists = GetTableValues(session, "ComboBox", "DATABASE_SERVER");
                var order = 2;
                // Проходим список всех найденных серверов и добавляем их в ComboBox
                foreach (DataRow server in servers)
                {
                    var sql = server["ServerName"].ToString();
                    var instance = server["InstanceName"].ToString();
                    if (!String.IsNullOrEmpty(instance)) sql = sql + "\\" + instance;
                    if (exists.Contains(instance)) continue;
                    InsertRecord(session, "ComboBox", new object[] { "DATABASE_SERVER", order, sql, sql });
                    order++;
                }
                // Устанавливаем элемент по умолчанию
                if (servers.Count > 0)
                {
                    var server = servers[0];
                    var sql = server["ServerName"].ToString();
                    var instance = server["InstanceName"].ToString();
                    if (!String.IsNullOrEmpty(instance)) sql = sql + "\\" + instance;
                    session["DATABASE_SERVER"] = sql;
                }
                return ActionResult.Success;
            }
            catch (Exception)
            {
                return ActionResult.SkipRemainingActions;
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="table"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static IList GetTableValues(Session session, String table, String property)
        {
            var database = session.Database;
            return database.ExecuteQuery("SELECT Value FROM {0} WHERE Property = '{1}'", table, property);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="table"></param>
        /// <param name="objects"></param>
        private static void InsertRecord(Session session, String table, Object[] objects)
        {
            var database = session.Database;
            var sqlInsertSring = database.Tables[table].SqlInsertString + " TEMPORARY";
            var view = database.OpenView(sqlInsertSring);
            view.Execute(new Record(objects));
            view.Close();
        }

        #endregion
    }
}
