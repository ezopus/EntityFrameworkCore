using Microsoft.Data.SqlClient;


//check connectionstrings.com to read up on different connection strings and parameters
SqlConnection connection = new SqlConnection("Server=.;Database=SoftUni;Trusted_Connection=True;Trust Server Certificate=True");

connection.Open();

using (connection)
{
    //initialize command which sends sql query to db
    SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", connection);


    //scalar commands, return only first value of first row
    //int employeesCount = (int)cmd.ExecuteScalar();
    //Console.WriteLine($"Count of employees: {employeesCount}");


    //data reader returns table
    SqlDataReader reader = cmd.ExecuteReader();

    //must be paired with using to open and close reader connection
    using (reader)
    {
        //Read() returns false when it has reached last value of returned table and is called
        while (reader.Read())
        {
            string firstName = (string)reader[1];
            string lastName = (string)reader[2];
            decimal salary = (decimal)reader["Salary"];

            Console.WriteLine($"{firstName} {lastName} - {salary}");
        }

    }
}