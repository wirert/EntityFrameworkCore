using Microsoft.Data.SqlClient;

namespace _9._Increase_Age_Stored_Procedure
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection(@"Server=.;
                                                           Database=MinionsDB;
                                                           TrustServerCertificate=True;
                                                           User Id=sa;
                                                           Password=AsDf23SQLServer");
            connection.Open();

            SqlCommand createProcCmd = new SqlCommand(@"CREATE OR ALTER PROC usp_GetOlder @id INT
                                                        AS
                                                        UPDATE Minions
                                                           SET Age += 1
                                                         WHERE Id = @id", connection);
            await createProcCmd.ExecuteNonQueryAsync();

            int id = int.Parse(Console.ReadLine());

            SqlCommand execProcCmd = new SqlCommand("EXEC usp_GetOlder @id", connection);
            execProcCmd.Parameters.AddWithValue("@id", id);

            await execProcCmd.ExecuteNonQueryAsync();

            SqlCommand readMinionByIdCmd = new SqlCommand(@"SELECT Name, Age FROM Minions WHERE Id = @Id", connection);
            readMinionByIdCmd.Parameters.AddWithValue("@id", id);

            using SqlDataReader reader   = await readMinionByIdCmd.ExecuteReaderAsync();

            reader.Read();
            Console.WriteLine($"{reader["Name"]} - {reader["Age"]} years old");
        }
    }
}