using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Server.Persistence
{
    /// <summary>
    /// A class to allow writing to and reading from a database
    /// </summary>
    public static partial class Database
    {
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
                                    VALUES($guid, $drug, $dosage)";

            command.Parameters.AddWithValue("$guid", info.Guid);
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
                                    WHERE Guid = $guid";

            command.Parameters.AddWithValue("$guid", drugInfo.Guid);

            var reader = command.ExecuteReader();

            //read a list of DateTimes from the table
            List<DosageInfo> dosages = new List<DosageInfo>();
            while (reader.Read())
            {
                dosages.Add(new DosageInfo()
                {
                    Guid = drugInfo.Guid,
                    Drug = (string)reader["Drug"],
                    Dosage = new Dosage(Convert.ToInt32(reader["Dosage"])).Micrograms
                });
            }

            return dosages;
        }
    }
}
