using Microsoft.Data.SqlClient;

namespace _7._Print_All_Minion_Names
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

            SqlCommand command = new SqlCommand(@"SELECT Name FROM Minions", connection);

            Queue<string> minionsQueue = new Queue<string>();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                minionsQueue.Enqueue(reader["Name"].ToString());
            }

            Stack<string> minionsStack = new Stack<string>(minionsQueue);            

            List<string> result = new List<string>();
            int count = minionsQueue.Count;

            while (result.Count != count)
            {
                result.Add(minionsQueue.Dequeue());

                if (result.Count == count) break;
                
                result.Add(minionsStack.Pop());
            }

            Console.WriteLine(string.Join(Environment.NewLine, result));
        }
    }
}