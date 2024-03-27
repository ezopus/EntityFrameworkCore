using Microsoft.Data.SqlClient;
using System.Text;

using SqlConnection connection = new SqlConnection(@"Server=.;Database=MinionsDB;Trusted_Connection=True;Trust Server Certificate=True");
using SqlConnection connection2 = new SqlConnection(@"Server=.;Database=MinionsDB;Trusted_Connection=True;Trust Server Certificate=True");

StringBuilder sb = new StringBuilder();

string countryInput = Console.ReadLine();

connection.Open();

SqlCommand selectCountryIdCmd = new SqlCommand(@" SELECT t.Name 
                                                    FROM Towns as t
                                                    JOIN Countries AS c ON c.Id = t.CountryCode
                                                   WHERE c.Name = @countryName", connection);
selectCountryIdCmd.Parameters.AddWithValue("@countryName", countryInput);

var reader = await selectCountryIdCmd.ExecuteReaderAsync();

connection2.Open();

SqlCommand updateTownsCmd = new SqlCommand(@"UPDATE Towns
                                                SET Name = UPPER(Name)
                                              WHERE CountryCode = (SELECT c.Id 
                                                                     FROM Countries AS c 
                                                                    WHERE c.Name = @countryName)", connection2);
updateTownsCmd.Parameters.AddWithValue("countryName", countryInput);

var townsChangedObj = await updateTownsCmd.ExecuteNonQueryAsync();

if (townsChangedObj == 0)
{
    sb.AppendLine("No town names were affected.");
}
else
{
    sb.AppendLine($"{townsChangedObj} town names were affected.");
    var towns = new List<string>();

    while (reader.Read())
    {
        towns.Add((string)reader["Name"]);
    }

    sb.AppendLine("[" + string.Join(", ", towns) + "]");
}

Console.WriteLine(sb.ToString().Trim());
