using Microsoft.Data.SqlClient;


//check connectionstrings.com to read up on different connection strings and parameters
SqlConnection connection = new SqlConnection("Server=.;Database=SoftUni;Trusted_Connection=True;Trust Server Certificate=True");

connection.Open();

using (connection)
{
    //initialize command which sends sql query to db
    SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", connection);


    //scalar commands, return only first value of first row
    int employeesCount = (int)cmd.ExecuteScalar();
    Console.WriteLine($"Count of employees: {employeesCount}");

}
