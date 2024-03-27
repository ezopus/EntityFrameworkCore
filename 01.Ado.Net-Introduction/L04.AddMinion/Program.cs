using Microsoft.Data.SqlClient;
using System.Text;

SqlConnection connection = new SqlConnection(@"Server=.;Database=MinionsDB;Trusted_Connection=True;Trust Server Certificate=True");

string[] minionInfo = Console.ReadLine().Split(": ", StringSplitOptions.RemoveEmptyEntries);
string[] villainInfo = Console.ReadLine().Split(": ", StringSplitOptions.RemoveEmptyEntries);

connection.Open();
SqlTransaction transaction = connection.BeginTransaction();

try
{
    StringBuilder sb = new StringBuilder();

    int townId = await GetTownIdAsync(minionInfo[1], connection, transaction, sb);
    int villainId = await GetVillainIdAsync(villainInfo[1], connection, transaction, sb);
    int minionId = await AddNewMinionAndGetMinionIdAsync(minionInfo[1], villainInfo[1], townId, connection, transaction, sb);

    await AddMinionAsServantToVillain(minionId, villainId, connection, transaction);

    await transaction.CommitAsync();

    Console.WriteLine(sb.ToString().Trim());
}
catch (Exception e)
{
    await transaction.RollbackAsync();
}

static async Task<int> GetTownIdAsync(string tokens, SqlConnection connection,
    SqlTransaction transaction, StringBuilder sb)
{
    string minionTown = tokens.Split(" ", StringSplitOptions.RemoveEmptyEntries)[2];
    int minionTownId = 0;

    SqlCommand getMinionTownCmd =
        new SqlCommand(@"SELECT Id FROM Towns WHERE Name = @townName", connection, transaction);
    getMinionTownCmd.Parameters.AddWithValue("@townName", minionTown);

    var getTownObj = await getMinionTownCmd.ExecuteScalarAsync();

    if (getTownObj == null)
    {
        SqlCommand addNewTownCmd =
            new SqlCommand(@"INSERT INTO Towns (Name) VALUES (@townName)", connection, transaction);
        addNewTownCmd.Parameters.AddWithValue("@townName", minionTown);

        await addNewTownCmd.ExecuteNonQueryAsync();

        minionTownId = (int)await getMinionTownCmd.ExecuteScalarAsync();

        sb.AppendLine($"Town {minionTown} was added to the database.");
    }
    else
    {
        minionTownId = (int)getTownObj;
    }

    return minionTownId;
}

static async Task<int> GetVillainIdAsync(string villainName, SqlConnection connection, SqlTransaction transaction,
    StringBuilder sb)
{
    int villainId = 0;

    SqlCommand getVillainId = new SqlCommand(@"SELECT Id FROM Villains WHERE Name = @Name", connection, transaction);
    getVillainId.Parameters.AddWithValue("@Name", villainName);

    var getVillainObj = await getVillainId.ExecuteScalarAsync();

    if (getVillainObj == null)
    {
        SqlCommand addNewVillainCmd =
            new SqlCommand(@"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)", connection, transaction);
        addNewVillainCmd.Parameters.AddWithValue("@villainName", villainName);

        await addNewVillainCmd.ExecuteNonQueryAsync();

        villainId = (int)await getVillainId.ExecuteScalarAsync();

        sb.AppendLine($"Villain {villainName} was added to the database.");
    }
    else
    {
        villainId = (int)getVillainObj;
    }

    return villainId;
}

static async Task<int> AddNewMinionAndGetMinionIdAsync(string minionTokens, string villainName, int minionTownId, SqlConnection connection, SqlTransaction transaction, StringBuilder sb)
{
    string[] tokens = minionTokens.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    string minionName = tokens[0];
    string minionAge = tokens[1];

    SqlCommand addNewMinionCmd =
        new SqlCommand(@"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)", connection, transaction);
    addNewMinionCmd.Parameters.AddWithValue("@name", minionName);
    addNewMinionCmd.Parameters.AddWithValue("@age", minionAge);
    addNewMinionCmd.Parameters.AddWithValue("@townId", minionTownId);

    await addNewMinionCmd.ExecuteNonQueryAsync();

    SqlCommand getMinionId = new SqlCommand(@"SELECT Id FROM Minions WHERE Name = @Name", connection, transaction);
    getMinionId.Parameters.AddWithValue("@Name", minionName);

    int minionId = (int)await getMinionId.ExecuteScalarAsync();

    sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");

    return minionId;
}

static async Task AddMinionAsServantToVillain(int minionId, int villainId, SqlConnection connection, SqlTransaction transaction)
{
    SqlCommand addMinionToVillain =
        new SqlCommand(@"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)", connection, transaction);
    addMinionToVillain.Parameters.AddWithValue("@minionId", minionId);
    addMinionToVillain.Parameters.AddWithValue("@villainId", villainId);

    await addMinionToVillain.ExecuteNonQueryAsync();
}