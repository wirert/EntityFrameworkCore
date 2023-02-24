using Microsoft.Data.SqlClient;
using System.Text;

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

        SqlCommand query = new SqlCommand(@"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                              FROM Villains AS v 
                                              JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                          GROUP BY v.Id, v.Name 
                                            HAVING COUNT(mv.VillainId) > 3 
                                          ORDER BY COUNT(mv.VillainId)", sqlConnection);
        using SqlDataReader reader =await query.ExecuteReaderAsync();

        StringBuilder sb = new StringBuilder();

        while (reader.Read())
        {
            string? villianName = reader["Name"].ToString();
            int minionsCount = (int)reader["MinionsCount"];

            sb.AppendLine($"{villianName} – {minionsCount}");
        }

        Console.WriteLine(sb.ToString().TrimEnd());
    }
}