using DuckDB.NET.Data;

namespace Njord.AisStream.Tests
{
    public class MessagesDataClass
    {
        public static IEnumerable<object[]> GetMessages(string connectionString, string tableName, string category)
        {
            using var con = new DuckDBConnection(connectionString);
            using var cmd = con.CreateCommand();
            cmd.CommandText = $"SELECT Id, Value, Valid FROM {tableName} WHERE Category=$category";
            cmd.Parameters.Add(new DuckDBParameter("category", category));
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new object[] { reader.GetInt64(0), reader.GetString(1), reader.GetBoolean(2) };
            }
        }
    }
}
