using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Server.Persistence
{
    public static class Database
    {
        private static string _connectionInfo;
        public static bool StateHasChanged;

        public static void SetConnInfo(string info)
        {
            _connectionInfo = info;
            StateHasChanged = false;
        }

        public static void AddDrugInfo(DrugInfo info)
        {
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO tblDrugInfo
                                    VALUES($drugName, $timeBetweenDose)";

            command.Parameters.AddWithValue("$drugName", info.Name);
            command.Parameters.AddWithValue("$timeBetweenDose", info.TimeBetweenDoses);

            command.ExecuteNonQuery();
        }

        public static List<DrugInfo> GetDrugInfo()
        {
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM tblDrugInfo";

            var reader = command.ExecuteReader();

            List<DrugInfo> info = new List<DrugInfo>();
            while (reader.Read())
            {
                var drug = new DrugInfo()
                {
                    Name = (string)reader["DrugName"],
                    TimeBetweenDoses = Convert.ToDecimal(reader["TimeBetweenDoses"])
                };

                drug.Entries = GetDrugEntries(drug);

                info.Add(drug);
            }

            return info;
        }

        public static void AddDrugEntry(DrugEntry entry)
        {

            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO tblDrugEntries
                                    VALUES($drugName, $time)";

            command.Parameters.AddWithValue("$drugName", entry.DrugName);
            command.Parameters.AddWithValue("$time", entry.Time.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            command.ExecuteNonQuery();
        }

        public static List<DateTime> GetDrugEntries(DrugInfo drugInfo)
        {
            using var connection = new SQLiteConnection(_connectionInfo);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM tblDrugEntries
                                    WHERE DrugName = $drugName";

            command.Parameters.AddWithValue("$drugName", drugInfo.Name);

            var reader = command.ExecuteReader();

            List<DateTime> entries = new List<DateTime>();
            while (reader.Read())
                entries.Add(DateTime.Parse((string)reader["Time"]));

            entries.Sort();

            return entries;
        }
    }
}
