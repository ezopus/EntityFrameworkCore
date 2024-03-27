using Microsoft.Data.SqlClient;

using SqlConnection connection = new SqlConnection(@"Server=.;Database=MinionsDB;Trusted_Connection=True;Trust Server Certificate=True");

connection.Open();

int minionId = int.Parse(Console.ReadLine());

SqlCommand updateMinionAgeCmd = new SqlCommand(@"EXEC usp_GetOlder @Id", connection);
updateMinionAgeCmd.Parameters.AddWithValue("@Id", minionId);

await updateMinionAgeCmd.ExecuteNonQueryAsync();

SqlCommand getMinionByIdCmd = new SqlCommand(@"SELECT Name, Age FROM Minions WHERE Id = @Id", connection);
getMinionByIdCmd.Parameters.AddWithValue("@Id", minionId);

var reader = await getMinionByIdCmd.ExecuteReaderAsync();

while (reader.Read())
{
    string name = (string)reader["Name"];
    int age = (int)reader["Age"];

    Console.WriteLine($"{name} - {age} years old");
}

