using _4n2h0ny.Steam.GUI.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Linq;

namespace _4n2h0ny.Steam.GUI
{
    public class SqliteDataAccess
    {
        public static List<SteamUrlModel> GetAllUrls()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionnString()))
            {
                var output = dbConnection.Query<SteamUrlModel>("SELECT * FROM Profile", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveUrl(SteamUrlModel steamUrl)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionnString()))
            {
                dbConnection.Execute("INSERT INTO Profile (Url) VALUES (@Url)", steamUrl);
            }
        }

        public static void DeleteUrl(SteamUrlModel steamUrl)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionnString()))
            {
                dbConnection.Execute("DELETE FROM Profile WHERE Url = @Url", steamUrl);
            }
        }

        public static void ResetProfileTable()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionnString()))
            {
                dbConnection.Execute("DELETE FROM Profile WHERE Id > 0");
            }
        }

        private static string LoadConnectionnString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
