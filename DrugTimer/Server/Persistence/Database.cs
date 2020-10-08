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

        public static void SetConnInfo(string info)
        {
            _connectionInfo = $"DataSource={info}";

            if (!File.Exists(info))
                InitTables(info);
        }

        private static void InitTables(string info)
        {
            if (info.Contains("/"))
            {
                //split on /'s, take all but the last one and recombine into string
                string dirPath = info.Split('/').Take(info.Split('/').Length - 1).Aggregate((a, b) => a + b);
                Directory.CreateDirectory(dirPath);
            }

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
