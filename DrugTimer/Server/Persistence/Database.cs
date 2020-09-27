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

        public static void SetConnInfo(string info)
        {
            _connectionInfo = info;
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
                info.Add(new DrugInfo()
                {
                    Name = (string)reader["DrugName"],
                    TimeBetweenDoses = Convert.ToDecimal(reader["TimeBetweenDoses"])
                });
            }

            return info;
        }
    }
}
