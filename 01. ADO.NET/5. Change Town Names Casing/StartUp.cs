using Microsoft.Data.SqlClient;

namespace _5._Change_Town_Names_Casing
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            Reader reader = new Reader();
            Writer writer = new Writer();

            string countryName = reader.ReadInput();

            await using SqlConnection sqlConnection = new SqlConnection(reader.ConnectionString());

            sqlConnection.Open();

            List<string> towns = new List<string>();
            SqlCommand townNameToUpperCmd = new SqlCommand(@"UPDATE Towns
                                                                   SET Name = UPPER(Name)
                                                                 WHERE CountryCode = (SELECT c.Id 
                                                                                        FROM Countries AS c 
                                                                                       WHERE c.Name = @countryName)", sqlConnection);
            townNameToUpperCmd.Parameters.AddWithValue("@countryName", countryName);

            await townNameToUpperCmd.ExecuteNonQueryAsync();

            SqlCommand findTownsInCountryCmd = new SqlCommand(@"SELECT t.Name 
                                                                  FROM Towns as t
                                                                  JOIN Countries AS c ON c.Id = t.CountryCode
                                                                 WHERE c.Name = @countryName", sqlConnection);
            findTownsInCountryCmd.Parameters.AddWithValue("@countryName", countryName);

            using SqlDataReader townReader = await findTownsInCountryCmd.ExecuteReaderAsync();

            while (townReader.Read())
            {
                towns.Add(townReader["Name"].ToString());
            }

            if (!towns.Any())
            {
                writer.WriteLine("No town names were affected.");
            }
            else
            {
                writer.WriteLine($"{towns.Count} town names were affected.");
                writer.WriteLine("[" + string.Join(", ", towns) + "]");
            }
        }
    }
}