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
                                    VALUES($drugName, $timeBetweenDose, $info)";

            command.Parameters.AddWithValue("$drugName", info.Name);
            command.Parameters.AddWithValue("$timeBetweenDose", info.TimeBetweenDoses);
            command.Parameters.AddWithValue("$info", info.Info);

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
                    Info = reader["Info"] == DBNull.Value ? null : (string)reader["Info"]
                };

                decimal? value;
                if (reader["TimeBetweenDoses"] == DBNull.Value)
                    value = null;
                else
                    value = Convert.ToDecimal(reader["TimeBetweenDoses"]);

                drug.TimeBetweenDoses = value;

                //find all DrugEntries associated with the DrugInfo
                drug.Entries = GetDrugEntries(drug);

                info.Add(drug);
            }

            return info;
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
                                    VALUES($drugName, $time)";

            command.Parameters.AddWithValue("$drugName", entry.DrugName);
            command.Parameters.AddWithValue("$time", entry.Time.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            //write to database
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets a list of all times associated with a given DrugInfo
        /// </summary>
        /// <returns>A list of DateTimes</returns>
        public static List<DateTime> GetDrugEntries(DrugInfo drugInfo)
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
            List<DateTime> entries = new List<DateTime>();
            while (reader.Read())
                entries.Add(DateTime.Parse((string)reader["Time"]));

            //sort the list
            entries.Sort();

            return entries;
        }

        public static void RemoveDrugEntry(DrugEntry drugEntry)
        {
            //creates and opens the connection
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            //create a command, set the text and set all parameters to given DrugEntry
            var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM tblDrugEntries WHERE DrugName LIKE $drugName AND Time LIKE $time";

            command.Parameters.AddWithValue("$drugName", drugEntry.DrugName);
            command.Parameters.AddWithValue("$time", drugEntry.Time.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            //write to database
            command.ExecuteNonQuery();
        }
    }
}
