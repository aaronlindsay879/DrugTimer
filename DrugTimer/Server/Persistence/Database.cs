using DrugTimer.Shared;
using DrugTimer.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace DrugTimer.Server.Persistence
{
    /// <summary>
    /// A class to allow writing to and reading from a database
    /// </summary>
    public static partial class Database
    {
        private static string _connectionInfo;
        public static Dictionary<string, bool> StateHasChanged = new Dictionary<string, bool>();

        /// <summary>
        /// Sets connection info for the database, creates file if needed
        /// </summary>
        /// <param name="info">Path to the database</param>
        public static void SetConnInfo(string info)
        {
            _connectionInfo = $"DataSource={info}";

            if (!File.Exists(info))
                InitTables(info);
        }

        /// <summary>
        /// Creates the database with all needed tables
        /// </summary>
        /// <param name="info">Path to the database</param>
        private static void InitTables(string info)
        {
            //if the path contains a slash (ie. a directory)
            if (info.Contains("/"))
            {
                //split on /'s, take all but the last one and recombine into string. this gives a string with all directories but not the last file
                //e.g. folderOne/folderTwo/database.db -> folderOne/folderTwo
                string dirPath = info.Split('/').Take(info.Split('/').Length - 1).Aggregate((a, b) => a + "/" + b);

                //creates the directory at the given path
                Directory.CreateDirectory(dirPath);
            }

            //creates a file at the path, now directory exists
            SQLiteConnection.CreateFile(info);

            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            var command = connection.CreateCommand();

            //create tblDrugInfo
            command.CommandText = @"CREATE TABLE tblDrugInfo (
                                        Guid TEXT PRIMARY KEY,
                                        DrugName,
	                                    Info TEXT,
                                        User TEXT,
	                                    TimeBetweenDoses REAL,
	                                    ExpectedDoses INTEGER,
                                        NumberLeft REAL,
	                                    DiscordWebHook TEXT,
	                                    DiscordWebHookEnabled INTEGER,
	                                    NotificationsEnabled INTEGER
                                    )";
            command.ExecuteNonQuery();

            //create tblDosageInfo
            command.CommandText = @"CREATE TABLE tblDosageInfo (
                                        Guid TEXT,
                                        Drug TEXT,
	                                    Dosage INTEGER,
	                                    FOREIGN KEY(Guid) REFERENCES tblDrugInfo(Guid) ON DELETE CASCADE
                                    )";
            command.ExecuteNonQuery();

            //create tblDrugEntries
            command.CommandText = @"CREATE TABLE tblDrugEntries (
                DrugGuid TEXT,
                EntryGuid TEXT,
	            Time TEXT,
	            Count REAL,
                Notes TEXT,
	            FOREIGN KEY(DrugGuid) REFERENCES tblDrugInfo(Guid) ON DELETE CASCADE
            )";
            command.ExecuteNonQuery();
        }
    }
}
