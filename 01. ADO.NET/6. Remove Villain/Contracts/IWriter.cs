using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6._Remove_Villain.Contracts
{
    public interface IWriter
    {
        void Write(string value);

        void WriteLine(string value);
    }
}
