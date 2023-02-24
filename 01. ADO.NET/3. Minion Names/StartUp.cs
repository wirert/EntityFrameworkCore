using Microsoft.Data.SqlClient;
using System.Text;

namespace _3._Minion_Names
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            await using SqlConnection sqlConnection = new SqlConnection(@"Server=.;
                                                Database=MinionsDB;
                                                TrustServerCertificate=True;
                                                User Id=sa;
                                                Password=AsDf23SQLServer");

            sqlConnection.Open();

            SqlCommand sqlVillianNameQuery = new SqlCommand(@"SELECT Name FROM Villains WHERE Id = @Id", sqlConnection);

            int villianIdInput = int.Parse(Console.ReadLine());

            sqlVillianNameQuery.Parameters.AddWithValue("@Id", villianIdInput);

            string? villianName =(string?) await sqlVillianNameQuery.ExecuteScalarAsync();

            if (villianName == null)
            {
                Console.WriteLine($"No villain with ID {villianIdInput} exists in the database.");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Villain: {villianName}");

            SqlCommand readMinionsQuery = new SqlCommand(@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) 
                                                               AS RowNum,
                                                                  m.Name, 
                                                                  m.Age
                                                             FROM MinionsVillains AS mv
                                                             JOIN Minions As m ON mv.MinionId = m.Id
                                                            WHERE mv.VillainId = @Id
                                                         ORDER BY m.Name", sqlConnection);

            readMinionsQuery.Parameters.AddWithValue("@Id", villianIdInput);

            using SqlDataReader minionsReader = await readMinionsQuery.ExecuteReaderAsync();

            if (!minionsReader.HasRows)
            {
                sb.AppendLine("(no minions)");
            }
            else
            {                
                while (minionsReader.Read())
                {
                    long number = (long)minionsReader["RowNum"];
                    string minionName = (string)minionsReader["Name"];
                    int minionAge = (int)minionsReader["Age"];

                    sb.AppendLine($"{number}. {minionName} {minionAge}");
                }
            }

            Console.WriteLine(sb.ToString().TrimEnd());
        }
    }
}