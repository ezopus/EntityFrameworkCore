using Microsoft.Data.SqlClient;

using SqlConnection connection = new SqlConnection(@"Server=.;Database=MinionsDB;Trusted_Connection=True;Trust Server Certificate=True");

connection.Open();

int[] tokens = Console.ReadLine()
                        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();

foreach (int token in tokens)
{
    SqlCommand updateMinionsCmd = new SqlCommand(@" UPDATE Minions SET Name = LOWER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1 WHERE Id = @Id", connection);
    updateMinionsCmd.Parameters.AddWithValue("@Id", token);

    await updateMinionsCmd.ExecuteNonQueryAsync();
}

SqlCommand getMinionsAndAgesCmd = new SqlCommand(@"SELECT Name, Age FROM Minions", connection);

var reader = await getMinionsAndAgesCmd.ExecuteReaderAsync();

while (reader.Read())
{
    string name = (string)reader["Name"];
    int age = (int)reader["Age"];
    Console.WriteLine($"{name} {age}");
}


