using Microsoft.Data.SqlClient;
using System.Text;

using SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDB;Trusted_Connection=True;Trust Server Certificate=True");

connection.Open();

int villainId = int.Parse(Console.ReadLine());

string result = await GetVillainName(villainId, connection);

Console.WriteLine(result);

static async Task<string> GetVillainName(int villainId, SqlConnection connection)
{
    StringBuilder sb = new StringBuilder();

    SqlCommand getVillainNameCmd = new SqlCommand("SELECT Name FROM Villains WHERE Id = @Id", connection);
    getVillainNameCmd.Parameters.AddWithValue("@Id", villainId);

    var villainNameObj = await getVillainNameCmd.ExecuteScalarAsync();

    if (villainNameObj == null)
    {
        return $"No villain with ID {villainId} exists in the database.";
    }

    sb.AppendLine("Villain: " + (string)villainNameObj);

    SqlCommand getMinionsCmd = new SqlCommand(@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                                              m.Name, 
                                                              m.Age
                                                         FROM MinionsVillains AS mv
                                                         JOIN Minions As m ON mv.MinionId = m.Id
                                                        WHERE mv.VillainId = @Id
                                                     ORDER BY m.Name", connection);
    getMinionsCmd.Parameters.AddWithValue("@Id", villainId);

    var reader = await getMinionsCmd.ExecuteReaderAsync();

    if (!reader.HasRows)
    {
        sb.AppendLine("(no minions)");

        return sb.ToString().Trim();
    }

    while (reader.Read())
    {
        long row = (long)reader["RowNum"];
        string minionName = (string)reader["Name"];
        int minionAge = (int)reader["Age"];

        sb.AppendLine($"{row}. {minionName} {minionAge}");
    }

    return sb.ToString().Trim();
}


