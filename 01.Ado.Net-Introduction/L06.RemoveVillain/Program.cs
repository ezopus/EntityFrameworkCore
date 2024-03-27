using Microsoft.Data.SqlClient;

using SqlConnection connection = new SqlConnection(@"Server=.;Database=MinionsDB;Trusted_Connection=True;Trust Server Certificate=True");

connection.Open();

int villainId = int.Parse(Console.ReadLine());

SqlCommand getVillainId = new SqlCommand(@"SELECT Name FROM Villains WHERE Id = @villainId", connection);
getVillainId.Parameters.AddWithValue("@villainId", villainId);

var villainObj = await getVillainId.ExecuteScalarAsync();

if (villainObj == null)
{
    Console.WriteLine("No such villain was found.");
}
else
{
    SqlCommand deleteVillainCmd = new SqlCommand(@"DELETE FROM Villains WHERE Id = @villainId", connection);
    deleteVillainCmd.Parameters.AddWithValue("@villainId", villainId);

    SqlCommand releaseMinionsCmd =
        new SqlCommand(@"DELETE FROM MinionsVillains WHERE VillainId = @villainId", connection);
    releaseMinionsCmd.Parameters.AddWithValue("@villainId", villainId);

    int countMinionsReleased = await releaseMinionsCmd.ExecuteNonQueryAsync();

    await deleteVillainCmd.ExecuteNonQueryAsync();
    Console.WriteLine($"{(string)villainObj} was deleted.");
    Console.WriteLine($"{countMinionsReleased} minions were released.");
}