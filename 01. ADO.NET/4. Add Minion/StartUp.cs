using Microsoft.Data.SqlClient;

namespace _4._Add_Minion
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            string[] minionInput = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string villianName = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Last();

            string minionName = minionInput[1];
            int minionAge = int.Parse(minionInput[2]);
            string minionTown = minionInput[3];

            await using SqlConnection sqlConnection = new SqlConnection(@"Server=.;
                                                Database=MinionsDB;
                                                TrustServerCertificate=True;
                                                User Id=sa;
                                                Password=AsDf23SQLServer");

            sqlConnection.Open();

            SqlTransaction transaction = sqlConnection.BeginTransaction();

            try
            {
                int townId = await FindTownIdAndAddNewTownIfNotExistInDbAsync(minionTown, sqlConnection, transaction);

                int villianId = await FindVillianIdAndAddNewIfNotExistAsync(villianName, sqlConnection, transaction);

                int minionId = await AddNewMinoinAndReturnIdAsync(minionName, minionAge, townId, sqlConnection, transaction);

                await AddMinionToVilianAsync(minionId, villianId, sqlConnection, transaction);

                Console.WriteLine($"Successfully added {minionName} to be minion of {villianName}.");

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                Console.WriteLine("Transaction Failed");
            }
        }

        private static async Task<int> FindTownIdAndAddNewTownIfNotExistInDbAsync(string minionTown, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            SqlCommand findMinionTownIdByName = new SqlCommand(@"SELECT Id FROM Towns WHERE Name = @townName", sqlConnection, transaction);
            findMinionTownIdByName.Parameters.AddWithValue("@townName", minionTown);

            int? townId = (int?)await findMinionTownIdByName.ExecuteScalarAsync();

            if (!townId.HasValue)
            {
                SqlCommand addNewTownToDb = new SqlCommand(@"INSERT INTO Towns (Name) VALUES (@townName)", sqlConnection, transaction);
                addNewTownToDb.Parameters.AddWithValue("@townName", minionTown);

                await addNewTownToDb.ExecuteNonQueryAsync();

                townId = (int)await findMinionTownIdByName.ExecuteScalarAsync();

                Console.WriteLine($"Town {minionTown} was added to the database.");
            }

            return (int)townId;
        }

        private static async Task<int> FindVillianIdAndAddNewIfNotExistAsync(string villianName, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            SqlCommand findVillianIdByNameCmd = new SqlCommand(@"SELECT Id 
                                                                   FROM Villains 
                                                                  WHERE Name = @Name", sqlConnection, transaction);
            findVillianIdByNameCmd.Parameters.AddWithValue("@Name", villianName);

            int? villianId = (int?)await findVillianIdByNameCmd.ExecuteScalarAsync();

            if (villianId == null)
            {
                SqlCommand addNewVillianToDb = new SqlCommand(@"INSERT INTO Villains (Name, EvilnessFactorId)  
                                                                     VALUES (@villainName, 4)", sqlConnection, transaction);
                addNewVillianToDb.Parameters.AddWithValue("@villainName", villianName);

                await addNewVillianToDb.ExecuteNonQueryAsync();

                villianId = (int)await findVillianIdByNameCmd.ExecuteScalarAsync();

                Console.WriteLine($"Villain {villianName} was added to the database.");
            }

            return (int)villianId;
        }

        private static async Task<int> AddNewMinoinAndReturnIdAsync(string minionName, int age, int townId, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            SqlCommand addNewMinionCmd = new SqlCommand(@"INSERT INTO Minions (Name, Age, TownId) 
                                                               VALUES (@name, @age, @townId)", sqlConnection, transaction);
            addNewMinionCmd.Parameters.AddWithValue("@name", minionName);
            addNewMinionCmd.Parameters.AddWithValue("@age", age);
            addNewMinionCmd.Parameters.AddWithValue("@townId", townId);

            await addNewMinionCmd.ExecuteNonQueryAsync();

            SqlCommand findMinionIdByNameCmd = new SqlCommand(@"SELECT Id 
                                                                  FROM Minions 
                                                                 WHERE Name = @Name AND Age = @Age AND TownId = @TownId", sqlConnection, transaction);
            findMinionIdByNameCmd.Parameters.AddWithValue("@Name", minionName);
            findMinionIdByNameCmd.Parameters.AddWithValue("@Age", age);
            findMinionIdByNameCmd.Parameters.AddWithValue("@TownId", townId);

            int minionId = (int)await findMinionIdByNameCmd.ExecuteScalarAsync();

            return (int)minionId;
        }

        private static async Task AddMinionToVilianAsync(int minionId, int villianId, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            SqlCommand addMinionVillianCmd = new SqlCommand(@"INSERT INTO MinionsVillains (MinionId, VillainId) 
                                                                   VALUES (@minionId, @villainId)", sqlConnection, transaction);
            addMinionVillianCmd.Parameters.AddWithValue("@minionId", minionId);
            addMinionVillianCmd.Parameters.AddWithValue("@villainId", villianId);

            await addMinionVillianCmd.ExecuteNonQueryAsync();
        }
    }
}