using System.Data.SqlClient;

namespace hwj.CommonLibrary.Function
{
    public class ConnectionString
    {
        #region MSSQL
        /// <summary>
        /// 生成数据库连接字符串
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="isWindowsVerification"></param>
        /// <returns></returns>
        public static string GetMSSQLConnectionString(string server, string database, string user, string password, bool isWindowsVerification)
        {
            SqlConnectionStringBuilder connStrBuilder = GetMSSQLConnectionStringBuilder(server, database, user, password, isWindowsVerification, null);
            if (connStrBuilder != null)
                return connStrBuilder.ToString();
            return string.Empty;
        }
        /// <summary>
        /// 生成SqlConnectionStringBuilder
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="isWindowsVerification"></param>
        /// <returns></returns>
        public static SqlConnectionStringBuilder GetMSSQLConnectionStringBuilder(string server, string database, string user, string password, bool isWindowsVerification)
        {
            return GetMSSQLConnectionStringBuilder(server, database, user, password, isWindowsVerification, null);
        }

        private static SqlConnectionStringBuilder GetMSSQLConnectionStringBuilder(string server, string database, string user, string password, bool isWindowsVerification, int? connectTimeout)
        {
            SqlConnectionStringBuilder connStrBuilder = new SqlConnectionStringBuilder();
            connStrBuilder.DataSource = server;
            connStrBuilder.InitialCatalog = database;
            if (!isWindowsVerification)
            {
                connStrBuilder.UserID = user;
                connStrBuilder.Password = password;
            }
            connStrBuilder.IntegratedSecurity = isWindowsVerification;
            if (connectTimeout.HasValue)
                connStrBuilder.ConnectTimeout = connectTimeout.Value;

            return connStrBuilder;
        }

        #endregion MSSQL
    }
}