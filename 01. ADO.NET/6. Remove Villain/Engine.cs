using _6._Remove_Villain.Contracts;
using Microsoft.Data.SqlClient;

namespace _6._Remove_Villain
{
    internal class Engine
    {
        private readonly IReader reader;
        private readonly IWriter writer;

        internal Engine()
        {
            reader = new ConsoleReader();
            writer = new ConsoleWriter();
        }

        public async Task RunAsync()
        {
            int villianId = int.Parse(reader.ReadLine());

            using SqlConnection connection = new SqlConnection(SqlStrings.ConnectionString);

            connection.Open();

            SqlCommand getVillianNameCmd = new SqlCommand(SqlStrings.GetVillianNameById, connection);
            getVillianNameCmd.Parameters.AddWithValue("@villainId", villianId);

            string? villianName = (string?)await getVillianNameCmd.ExecuteScalarAsync();

            if (villianName == null)
            {
                writer.WriteLine("No such villain was found.");
                return;
            }

            SqlCommand releaseMinionsCmd = new SqlCommand(SqlStrings.DeleteVilliansMinionsByVillianId, connection);
            releaseMinionsCmd.Parameters.AddWithValue("@villainId", villianId);

            int releasedMinionsCount = await releaseMinionsCmd.ExecuteNonQueryAsync(); 

            SqlCommand deleteVillianCmd = new SqlCommand(SqlStrings.DeleteVillianById, connection);
            deleteVillianCmd.Parameters.AddWithValue("@villainId", villianId);

            await deleteVillianCmd.ExecuteNonQueryAsync();

            writer.WriteLine($"{villianName} was deleted.");
            writer.WriteLine($"{releasedMinionsCount} minions were released.");
        }
    }
}
