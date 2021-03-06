using System;
using System.Diagnostics;
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.Deployment.WindowsInstaller;

namespace SqlActionLibrary
{
    public class CustomActions
    {
        #region Public Members

        /// <summary>
        /// ��������� �������� ����������� � SQL-�������.
        /// </summary>
        /// <param name="session">������ ������, ������� ������������ ������� ���������.</param>
        /// <returns>���������� ������ ���������� ����������������� ��������.</returns>
        [CustomAction]
        public static ActionResult SqlConnect(Session session)
        {
            session.Log("Begin SqlConnect");
            string connectionString;
            // �������� ��� �����
            var logonType = session["DATABASE_LOGON_TYPE"];
            if (logonType.Equals("DatabaseIntegratedAuth"))
            {
                // ���������� (�������� ����������� Windows)
                connectionString = String.Format("Data Source={0};Integrated Security=SSPI", session["DATABASE_SERVER"]);

            }
            else if (logonType.Equals("DatabaseAccount"))
            {
                // ������ ��� ������������ � ������ (�������� ����������� SQL)
                connectionString = String.Format("Data Source={0};User Id={1}; Password={2}", session["DATABASE_SERVER"], session["DATABASE_USERNAME"], session["DATABASE_PASSWORD"]);
            }
            else
            {
                // �� ��������� ��� �����
                session["CONNECTION_ESTABLISHED"] = "";
                session.Log("SqlConnect is not successful");
                return ActionResult.NotExecuted;
            }
            // ��������� ����������� � SQL-�������
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
        /// �������� ������ ��������� SQL-��������.
        /// </summary>
        /// <param name="session">������ ������, ������� ������������ ������� ���������.</param>
        /// <returns>���������� ������ ���������� ����������������� ��������.</returns>
        [CustomAction]
        public static ActionResult GetSqlServers(Session session)
        {
            try
            {
                // �������� ������ SQL-��������
                var sqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance;
                var servers = sqlDataSourceEnumerator.GetDataSources().Rows;
                // �������� ������ ������������ ��������
                var exists = GetTableValues(session, "ComboBox", "DATABASE_SERVER");
                var order = 2;
                // �������� ������ ���� ��������� �������� � ��������� �� � ComboBox
                foreach (DataRow server in servers)
                {
                    var sql = server["ServerName"].ToString();
                    var instance = server["InstanceName"].ToString();
                    if (!String.IsNullOrEmpty(instance)) sql = sql + "\\" + instance;
                    if (exists.Contains(instance)) continue;
                    InsertRecord(session, "ComboBox", new object[] { "DATABASE_SERVER", order, sql, sql });
                    order++;
                }
                // ������������� ������� �� ���������
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

        internal static bool IsLocalDBInstalled()
        {
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/C sqllocaldb info";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string sOutput = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            //If LocalDb is not installed then it will return that 'sqllocaldb' is not recognized as an internal or external command operable program or batch file.
            if (sOutput == null || sOutput.Trim().Length == 0 || sOutput.Contains("not recognized"))
                return false;
            if (sOutput.ToLower().Contains("mssqllocaldb")) //This is a defualt instance in local DB
                return true;
            return false;
        }
    }
}
