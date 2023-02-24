using Microsoft.Data.SqlClient;
using System.Text;

namespace _8._Increase_Minion_Age
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection(@"Server=.;
                                                           Database=MinionsDB;
                                                           TrustServerCertificate=True;
                                                           User Id=sa;
                                                           Password=AsDf23SQLServer");
            connection.Open();

            int[] ids = Console.ReadLine().Split().Select(int.Parse).ToArray();

            
            for (int i = 0; i < ids.Length; i++)
            {
                SqlCommand changeAgeAndFirstLetterByIdCmd = new SqlCommand(@"UPDATE Minions
                                                                            SET Name = LOWER(LEFT(Name, 1))
                                                                                       + SUBSTRING(Name, 2, LEN(Name)),
                                                                                       Age += 1                    
                                                                          WHERE Id = @Id", connection);
                changeAgeAndFirstLetterByIdCmd.Parameters.AddWithValue("@id", ids[i]);

                await changeAgeAndFirstLetterByIdCmd.ExecuteNonQueryAsync();
            }

            SqlCommand readAllMinionsCmd = new SqlCommand(@"SELECT Name, Age FROM Minions", connection);

            StringBuilder sb = new StringBuilder();

            using SqlDataReader reader =await readAllMinionsCmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                sb.AppendLine($"{reader["Name"]} {reader["Age"]}");
            }

            Console.WriteLine(sb.ToString().TrimEnd());
        }
    }
}