using Microsoft.Data.SqlClient;
using System.Text;

SqlConnection connection = new SqlConnection(@"Server=.;Database=MinionsDB;Trusted_Connection=True;Trust Server Certificate=True");

string[] minionInfo = Console.ReadLine().Split(": ");
string[] villainInfo = Console.ReadLine().Split(": ");

connection.Open();
SqlTransaction transaction = connection.BeginTransaction();

try
{
    StringBuilder sb = new StringBuilder();

    int? townId = await GetTownIdAsync(minionInfo[1], villainInfo[1], connection, transaction);

    //Console.WriteLine(result);
    await transaction.CommitAsync();
}
catch (Exception e)
{
    await transaction.RollbackAsync();
}

static async Task<int> GetTownIdAsync(string tokens, string villainName, SqlConnection connection, SqlTransaction transaction)
{
    StringBuilder sb = new StringBuilder();
    string[] minionTokens = tokens.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    string minionName = minionTokens[0];
    int minionAge = int.Parse(minionTokens[1]);
    string minionTown = minionTokens[2];
    int minionTownId = 0;


    SqlCommand getMinionTownCmd = new SqlCommand(@"SELECT Id FROM Towns WHERE Name = @townName", connection, transaction);
    getMinionTownCmd.Parameters.AddWithValue("@townName", minionTown);

    var getTownObj = await getMinionTownCmd.ExecuteScalarAsync();

    if (getTownObj == null)
    {
        SqlCommand addNewTownCmd = new SqlCommand(@"INSERT INTO Towns (Name) VALUES (@townName)", connection, transaction);
        addNewTownCmd.Parameters.AddWithValue("@townName", minionTown);

        await addNewTownCmd.ExecuteNonQueryAsync();

        sb.AppendLine($"Town {minionTown} was added to the database.");
    }
    else
    {
        minionTownId = (int)getTownObj;
    }

    return minionTownId;

    SqlCommand getVillainId = new SqlCommand(@"SELECT Id FROM Villains WHERE Name = @Name", connection, transaction);
    getVillainId.Parameters.AddWithValue("@Name", villainName);

    var getVillainObj = await getVillainId.ExecuteScalarAsync();

    if (getVillainObj == null)
    {
        SqlCommand addNewVillainCmd =
            new SqlCommand(@"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)", connection, transaction);
        addNewVillainCmd.Parameters.AddWithValue("@villainName", villainName);

        await addNewVillainCmd.ExecuteNonQueryAsync();

        sb.AppendLine($"Villain {villainName} was added to the database.");
    }

    SqlCommand addNewMinionCmd =
        new SqlCommand(@"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)", connection, transaction);
    addNewMinionCmd.Parameters.AddWithValue("@name", minionName);
    addNewMinionCmd.Parameters.AddWithValue("@age", minionAge);
    addNewMinionCmd.Parameters.AddWithValue("@townId", minionTownId);

    await addNewMinionCmd.ExecuteNonQueryAsync();

    sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");


    SqlCommand getMinionId = new SqlCommand(@"SELECT Id FROM Minions WHERE Name = @Name", connection, transaction);
    getMinionId.Parameters.AddWithValue("@Name", minionName);

    int minionId = (int)await getMinionId.ExecuteScalarAsync();
    int villainId = (int)await getVillainId.ExecuteScalarAsync();

    SqlCommand addMinionToVillain =
        new SqlCommand(@"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)", connection, transaction);
    addMinionToVillain.Parameters.AddWithValue("@minionId", minionId);
    addMinionToVillain.Parameters.AddWithValue("@villainId", villainId);

    await addMinionToVillain.ExecuteNonQueryAsync();

    return sb.ToString().Trim();
}