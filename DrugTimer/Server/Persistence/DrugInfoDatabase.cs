using DrugTimer.Shared;
using DrugTimer.Shared.Extensions;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DrugTimer.Server.Persistence
{
    /// <summary>
    /// A class to allow writing to and reading from a database
    /// </summary>
    public static partial class Database
    {

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
                                    VALUES($guid, $drugName, $info, $user, $timeBetweenDose, $expectedDoses, $numberLeft, $discordWebHook, $discordWebHookEnabled, $notificationsEnabled)";

            command.Parameters.AddWithValue("$guid", info.Guid);
            command.Parameters.AddWithValue("$drugName", info.Name);
            command.Parameters.AddWithValue("$info", info.Info);
            command.Parameters.AddWithValue("user", info.User);
            command.Parameters.AddWithValue("$timeBetweenDose", info.TimeBetweenDoses);
            command.Parameters.AddWithValue("$expectedDoses", info.ExpectedDoses);
            command.Parameters.AddWithValue("$numberLeft", info.NumberLeft);
            command.Parameters.AddWithValue("$discordWebHook", info.DrugSettings.DiscordWebHook);
            command.Parameters.AddWithValue("$discordWebHookEnabled", info.DrugSettings.DiscordWebHookEnabled);
            command.Parameters.AddWithValue("$notificationsEnabled", info.DrugSettings.NotificationsEnabled);

            //add all associated dosages
            foreach (DosageInfo dosageInfo in info.Dosages)
                AddDosageInfo(dosageInfo);

            //write to database
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets a list of all DrugInfos stored within the database
        /// </summary>
        /// <returns>A list of DrugInfo</returns>
        public static List<DrugInfo> GetDrugInfo(string guid = null)
        {
            //create and open the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create the command, set the text and create a reader from that command
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM tblDrugInfo";

            if (guid is not null)
            {
                command.CommandText += "\nWHERE Guid = $guid";
                command.Parameters.AddWithValue("$guid", guid);
            }

            var reader = command.ExecuteReader();

            //read a list of DrugInfos from the table
            List<DrugInfo> info = new List<DrugInfo>();
            while (reader.Read())
            {
                var drug = new DrugInfo
                {
                    Guid = (string) reader["Guid"],
                    Name = (string) reader["DrugName"],
                    User = (string) reader["User"],
                    Info = reader["Info"].HandleNull<string>(),
                    TimeBetweenDoses = reader["TimeBetweenDoses"].HandleNull<decimal?>(),
                    ExpectedDoses = reader["ExpectedDoses"].HandleNull<int?>(),
                    NumberLeft = reader["NumberLeft"].HandleNull<decimal>(),
                    DrugSettings =
                    {
                        NotificationsEnabled = reader["NotificationsEnabled"].HandleNull<bool>(),
                        DiscordWebHook = reader["DiscordWebHook"].HandleNull<string>(),
                        DiscordWebHookEnabled = reader["DiscordWebHookEnabled"].HandleNull<bool>()
                    }
                };


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
                                    WHERE Guid LIKE $guid";

            command.Parameters.AddWithValue("$guid", drugInfo.Guid);

            //write to database
            command.ExecuteNonQuery();

            //then remove all drug entries with same name
            command.CommandText = @"DELETE FROM tblDrugEntries
                                    WHERE DrugGuid LIKE $guid";

            command.Parameters.AddWithValue("$guid", drugInfo.Guid);
            command.ExecuteNonQuery();

            //then remove all dosage info with same name
            command.CommandText = @"DELETE FROM tblDosageInfo
                                    WHERE Guid LIKE $guid";

            command.Parameters.AddWithValue("$guid", drugInfo.Guid);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates a given DrugInfo, uses drug guid as comparator
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
                                       SET NumberLeft = $numberLeft,
                                           DiscordWebHook = $webHook,
                                           DiscordWebHookEnabled = $webHookEnabled,
                                           NotificationsEnabled = $notifications
                                     WHERE Guid LIKE $guid";

            command.Parameters.AddWithValue("$numberLeft", drugInfo.NumberLeft);
            command.Parameters.AddWithValue("$webHook", drugInfo.DrugSettings.DiscordWebHook);
            command.Parameters.AddWithValue("$webHookEnabled", drugInfo.DrugSettings.DiscordWebHookEnabled);
            command.Parameters.AddWithValue("$notifications", drugInfo.DrugSettings.NotificationsEnabled);
            command.Parameters.AddWithValue("$guid", drugInfo.Guid);

            //write to database
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the number of doses left for a given guid
        /// </summary>
        /// <param name="guid">Guid of drug to update</param>
        /// <param name="amount">Amount of doses left</param>
        /// <param name="set">Whether to set the value instead of updating</param>
        public static void UpdateNumberLeft(string guid, decimal amount, bool set = true)
        {
            //creates and opens the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create a command, set the text and set all parameters to given info
            var command = connection.CreateCommand();
            command.CommandText = $@"UPDATE tblDrugInfo
                                    SET NumberLeft = {(set ? "$numberLeft" : "NumberLeft - $numberLeft")}
                                    WHERE Guid LIKE $guid";
                
            command.Parameters.AddWithValue("$numberLeft", amount);
            command.Parameters.AddWithValue("$guid", guid);

            //write to database
            command.ExecuteNonQuery();
        }
    }
}
