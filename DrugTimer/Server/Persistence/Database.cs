using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using DrugTimer.Client.Extensions;
using DrugTimer.Shared.Extensions;

namespace DrugTimer.Server.Persistence
{
    /// <summary>
    /// A class to allow writing to and reading from a database
    /// </summary>
    public static class Database
    {
        private static string _connectionInfo;
        public static Dictionary<string, bool> StateHasChanged = new Dictionary<string, bool>();

        public static void SetConnInfo(string info)
        {
            _connectionInfo = info;
        }

        /// <summary>
        /// Add a given DrugInfo to the database
        /// </summary>
        /// <param name="info">DrugInfo to add</param>
        public static void AddDrugInfo(DrugInfo info)
        {
            //create and open the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create a command, set the text and set all parameters to given DrugInfo
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO tblDrugInfo
                                    VALUES($drugName, $info, $timeBetweenDose, $expectedDoses, $discordWebHook, $discordWebHookEnabled)";

            command.Parameters.AddWithValue("$drugName", info.Name);
            command.Parameters.AddWithValue("$info", info.Info);
            command.Parameters.AddWithValue("$timeBetweenDose", info.TimeBetweenDoses);
            command.Parameters.AddWithValue("$expectedDoses", info.ExpectedDoses);
            command.Parameters.AddWithValue("$discordWebHook", info.DrugSettings.DiscordWebHook);
            command.Parameters.AddWithValue("$discordWebHookEnabled", info.DrugSettings.DiscordWebHookEnabled);

            //add all assosciated dosages
            foreach (DosageInfo dosageInfo in info.Dosages)
                AddDosageInfo(dosageInfo);

            //write to database
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets a list of all DrugInfos stored within the database
        /// </summary>
        /// <returns>A list of DrugInfo</returns>
        public static List<DrugInfo> GetDrugInfo()
        {
            //create and open the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create the command, set the text and create a reader from that command
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM tblDrugInfo";

            var reader = command.ExecuteReader();

            //read a list of DrugInfos from the table
            List<DrugInfo> info = new List<DrugInfo>();
            while (reader.Read())
            {
                var drug = new DrugInfo()
                {
                    Name = (string)reader["DrugName"],
                    Info = reader["Info"].HandleNull<string>(),
                    TimeBetweenDoses = reader["TimeBetweenDoses"].HandleNull<decimal?>(),
                    ExpectedDoses = reader["ExpectedDoses"].HandleNull<int?>()
                };

                drug.DrugSettings.DiscordWebHook = reader["DiscordWebHook"].HandleNull<string>();
                drug.DrugSettings.DiscordWebHookEnabled = reader["DiscordWebHookEnabled"].HandleNull<bool>();

                //find all DrugEntries associated with the DrugInfo
                drug.Entries = GetDrugEntries(drug);
                drug.Dosages = GetDosageInfos(drug);

                info.Add(drug);
            }

            return info;
        }

        /// <summary>
        /// Removes a given drug info from the database, along with all associated entries and dosages
        /// </summary>
        /// <param name="drugInfo">DrugInfo to remove</param>
        public static void RemoveDrugInfo(DrugInfo drugInfo)
        {
            //creates and opens the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create a command, set the text and set all parameters to given DrugInfo
            var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM tblDrugInfo
                                    WHERE DrugName LIKE $drugName";

            command.Parameters.AddWithValue("$drugName", drugInfo.Name);
            command.Parameters.AddWithValue("$timeBetweenDoses", drugInfo.TimeBetweenDoses);
            command.Parameters.AddWithValue("$info", drugInfo.Info);

            //write to database
            command.ExecuteNonQuery();

            //then remove all drug entries with same name
            command.CommandText = @"DELETE FROM tblDrugEntries
                                    WHERE DrugName LIKE $drugName";

            command.Parameters.AddWithValue("$drugName", drugInfo.Name);
            command.ExecuteNonQuery();

            //then remove all dosage info with same name
            command.CommandText = @"DELETE FROM tblDosageInfo
                                    WHERE DrugName LIKE $drugName";

            command.Parameters.AddWithValue("$drugName", drugInfo.Name);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates a given DrugInfo, uses drug name as comparator
        /// </summary>
        /// <param name="drugInfo">DrugInfo to update</param>
        public static void UpdateDrugInfo(DrugInfo drugInfo)
        {
            //creates and opens the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create a command, set the text and set all parameters to given DrugInfo
            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE tblDrugInfo
                                       SET DiscordWebHook = $webHook,
                                           DiscordWebHookEnabled = $webHookEnabled
                                     WHERE DrugName LIKE $drugName";

            command.Parameters.AddWithValue("$webHook", drugInfo.DrugSettings.DiscordWebHook);
            command.Parameters.AddWithValue("$webHookEnabled", drugInfo.DrugSettings.DiscordWebHookEnabled);
            command.Parameters.AddWithValue("$drugName", drugInfo.Name);

            //write to database
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Add a given DrugEntry to the database
        /// </summary>
        /// <param name="info">DrugEntry to add</param>
        public static void AddDrugEntry(DrugEntry entry)
        {
            //creates and opens the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create a command, set the text and set all parameters to given DrugEntry
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO tblDrugEntries
                                    VALUES($drugName, $time, $count)";

            command.Parameters.AddWithValue("$drugName", entry.DrugName);
            command.Parameters.AddWithValue("$time", entry.Time.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("$count", entry.Count);

            //write to database
            command.ExecuteNonQuery();
        }


        /// <summary>
        /// Gets a list of all times associated with a given DrugInfo
        /// </summary>
        /// <returns>A list of DateTimes</returns>
        public static List<DrugEntry> GetDrugEntries(DrugInfo drugInfo)
        {
            //creates and opens the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create a command, set the text and set all parameters to given DrugInfo
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM tblDrugEntries
                                    WHERE DrugName = $drugName";

            command.Parameters.AddWithValue("$drugName", drugInfo.Name);

            var reader = command.ExecuteReader();

            //read a list of DateTimes from the table
            List<DrugEntry> entries = new List<DrugEntry>();
            while (reader.Read())
                entries.Add(new DrugEntry()
                {
                    DrugName = drugInfo.Name,
                    Time = DateTime.Parse((string)reader["Time"]),
                    Count = Convert.ToInt32(reader["Count"])
                });

            //sort the list
            entries = entries.OrderBy(x => x.Time).ToList();

            return entries;
        }

        /// <summary>
        /// Removes a given DrugEntry from the database
        /// </summary>
        /// <param name="drugEntry">DrugEntry to remove</param>
        public static void RemoveDrugEntry(DrugEntry drugEntry)
        {
            //creates and opens the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create a command, set the text and set all parameters to given DrugEntry
            var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM tblDrugEntries
                                    WHERE DrugName LIKE $drugName
                                      AND Time LIKE $time";

            command.Parameters.AddWithValue("$drugName", drugEntry.DrugName);
            command.Parameters.AddWithValue("$time", drugEntry.Time.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            //write to database
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Adds a given DosageInfo to the database
        /// </summary>
        /// <param name="info">DosageInfo to add</param>
        public static void AddDosageInfo(DosageInfo info)
        {
            //creates and opens the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create a command, set the text and set all parameters to given DrugEntry
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO tblDosageInfo
                                    VALUES($drugName, $drug, $dosage)";

            command.Parameters.AddWithValue("$drugName", info.DrugName);
            command.Parameters.AddWithValue("$drug", info.Drug);
            command.Parameters.AddWithValue("$dosage", info.Dosage);

            //write to database
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Returns all DosageInfos assosciated with a given DrugInfo from the database
        /// </summary>
        /// <param name="drugInfo">DrugInfo to find assosciated DosageInfos for</param>
        /// <returns>A list of DosageInfos</returns>
        public static List<DosageInfo> GetDosageInfos(DrugInfo drugInfo)
        {
            //creates and opens the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create a command, set the text and set all parameters to given DrugInfo
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM tblDosageInfo
                                    WHERE DrugName = $drugName";

            command.Parameters.AddWithValue("$drugName", drugInfo.Name);

            var reader = command.ExecuteReader();

            //read a list of DateTimes from the table
            List<DosageInfo> dosages = new List<DosageInfo>();
            while (reader.Read())
            {
                dosages.Add(new DosageInfo()
                {
                    DrugName = drugInfo.Name,
                    Drug = (string)reader["Drug"],
                    Dosage = new Dosage(Convert.ToInt32(reader["Dosage"])).Micrograms
                });
            }


            return dosages;
        }
    }
}
