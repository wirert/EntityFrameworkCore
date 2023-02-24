using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6._Remove_Villain
{
    internal static class SqlStrings
    {
        internal const string ConnectionString = @"Server=.;
                                                 Database=MinionsDB;
                                                 TrustServerCertificate=True;
                                                 User Id=sa;
                                                 Password=AsDf23SQLServer";

        internal const string DeleteVilliansMinionsByVillianId = @"DELETE FROM MinionsVillains 
                                                                       WHERE VillainId = @villainId";

        internal const string DeleteVillianById = @"DELETE FROM Villains
                                                        WHERE Id = @villainId";

        internal const string GetVillianNameById = @"SELECT Name 
                                                     FROM Villains 
                                                    WHERE Id = @villainId";
    }
}
