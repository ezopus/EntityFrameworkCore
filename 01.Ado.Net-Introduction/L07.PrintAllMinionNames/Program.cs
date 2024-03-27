using Microsoft.Data.SqlClient;

using SqlConnection connection = new SqlConnection(@"Server=.;Database=MinionsDB;Trusted_Connection=True;Trust Server Certificate=True");

connection.Open();

SqlCommand getMinionNamesCmd = new SqlCommand(@"SELECT Name FROM Minions", connection);

var reader = await getMinionNamesCmd.ExecuteReaderAsync();

var names = new List<string>();

while (reader.Read())
{
    names.Add((string)reader["Name"]);
}

var output = new List<string>();
int half = names.Count / 2;
int first = 0;
int second = names.Count - 1;

if (names.Count % 2 == 0)
{
    while (first < half && second >= half)
    {
        output.Add(names[first]);
        output.Add(names[second]);
        first++;
        second--;
    }
}
else
{
    while (first <= half && second > half)
    {
        output.Add(names[first]);
        output.Add(names[second]);
        first++;
        second--;
    }
    output.Add(names[second]);
}

output.ForEach(a => Console.WriteLine(a));