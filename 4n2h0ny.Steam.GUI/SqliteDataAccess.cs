using _4n2h0ny.Steam.GUI.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using System.Collections.ObjectModel;

namespace _4n2h0ny.Steam.GUI
{
    public class SqliteDataAccess
    {
        #region Profile
        public static List<SteamUrlModel> GetAllUrls()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = dbConnection.Query<SteamUrlModel>("SELECT * FROM Profile", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveUrl(SteamUrlModel steamUrl)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                dbConnection.Execute("INSERT INTO Profile (Url) VALUES (@Url)", steamUrl);
            }
        }

        public static void DeleteUrl(SteamUrlModel steamUrl)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                dbConnection.Execute("DELETE FROM Profile WHERE Url = @Url", steamUrl);
            }
        }

        public static void ResetProfileTable()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                dbConnection.Execute("DELETE FROM Profile WHERE Id > 0");
            }
        }

        #endregion Profile

        #region ManualProfile
        public static ObservableCollection<SteamUrlModel> GetAllManualUrls()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = dbConnection.Query<SteamUrlModel>("SELECT * FROM ManualProfile", new DynamicParameters());
                ObservableCollection<SteamUrlModel> manualUrlObservableCollection = new(output);
                return manualUrlObservableCollection;
            }
        }

        public static void SaveManualUrl(SteamUrlModel steamUrl)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                dbConnection.Execute("INSERT INTO ManualProfile (Url) VALUES (@Url)", steamUrl);
            }
        }

        public static void DeleteManualUrl(SteamUrlModel steamUrl)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                dbConnection.Execute("DELETE FROM ManualProfile WHERE Url = @Url", steamUrl);
            }
        }

        public static void ResetManualProfileTable()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                dbConnection.Execute("DELETE FROM ManualProfile WHERE Id > 0");
            }
        }
        #endregion ManualProfile

        #region ExclusionProfile

        public static ObservableCollection<SteamUrlModel> GetAllExcludedUrls()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = dbConnection.Query<SteamUrlModel>("SELECT * FROM ExclusionProfile", new DynamicParameters());
                ObservableCollection<SteamUrlModel> manualUrlObservableCollection = new(output);
                return manualUrlObservableCollection;
            }
        }
        public static void SaveExcludedUrl(SteamUrlModel steamUrl)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                dbConnection.Execute("INSERT INTO ExclusionProfile (Url) VALUES (@Url)", steamUrl);
            }
        }

        public static void DeleteExcludedUrl(SteamUrlModel steamUrl)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString()))
            {
                dbConnection.Execute("DELETE FROM ExclusionProfile WHERE Url = @Url", steamUrl);
            }
        }

        #endregion ExclusionProfile

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
