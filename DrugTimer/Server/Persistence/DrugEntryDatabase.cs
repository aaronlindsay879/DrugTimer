using DrugTimer.Shared;
using DrugTimer.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace DrugTimer.Server.Persistence
{
    /// <summary>
    /// A class to allow writing to and reading from a database
    /// </summary>
    public static partial class Database
    {
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
                                    VALUES($drugGuid, $entryGuid, $time, $count, $notes)";

            command.Parameters.AddWithValue("$drugGuid", entry.DrugGuid);
            command.Parameters.AddWithValue("$entryGuid", entry.EntryGuid);
            command.Parameters.AddWithValue("$time", entry.Time.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("$count", entry.Count);
            command.Parameters.AddWithValue("$notes", entry.Notes);

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
                                    WHERE DrugGuid = $drugGuid
                                    ORDER BY Time DESC";
            

            command.Parameters.AddWithValue("$drugGuid", drugInfo.Guid);

            var reader = command.ExecuteReader();

            //create a list of commands to run once main loop is done
            List<DrugEntry> updateEntries = new List<DrugEntry>();

            //read a list of DateTimes from the table
            List<DrugEntry> entries = new List<DrugEntry>();
            while (reader.Read())
            {
                DrugEntry entry = new DrugEntry()
                {
                    DrugGuid = drugInfo.Guid,
                    EntryGuid = (string)reader["EntryGuid"],
                    Time = DateTime.Parse((string)reader["Time"]),
                    Count = Convert.ToDecimal(reader["Count"]),
                    Notes = reader["Notes"].HandleNull<string>()
                };

                entries.Add(entry);
            }

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
                                    WHERE EntryGuid LIKE $entryGuid";

            command.Parameters.AddWithValue("$drugGuid", drugEntry.DrugGuid);
            command.Parameters.AddWithValue("$entryGuid", drugEntry.EntryGuid);

            //write to database
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates a DrugEntry, using GUID as comparator
        /// </summary>
        /// <param name="drugEntry">DrugEntry to update</param>
        public static void UpdateDrugEntry(DrugEntry drugEntry)
        {
            //creates and opens the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create a command, set the text and set all parameters to given DrugInfo
            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE tblDrugEntries
                                       SET Time = $time,
                                           Count = $count,
                                           Notes = $notes
                                     WHERE EntryGuid LIKE $entryGuid";

            command.Parameters.AddWithValue("$time", drugEntry.Time);
            command.Parameters.AddWithValue("$count", drugEntry.Count);
            command.Parameters.AddWithValue("$notes", drugEntry.Notes);
            command.Parameters.AddWithValue("$entryGuid", drugEntry.EntryGuid);

            //write to database
            command.ExecuteNonQuery();
        }
    }
}
