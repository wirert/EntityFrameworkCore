namespace _5._Change_Town_Names_Casing
{
    internal class Reader
    {        
        public string ReadInput() => Console.ReadLine();

        public string ConnectionString() => @"Server=.;
                                              Database=MinionsDB;
                                              TrustServerCertificate=True;
                                              User Id=sa;
                                              Password=AsDf23SQLServer";


    }
}
