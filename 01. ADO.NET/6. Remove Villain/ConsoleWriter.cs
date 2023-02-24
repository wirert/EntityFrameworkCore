using _6._Remove_Villain.Contracts;

namespace _6._Remove_Villain
{
    public class ConsoleWriter : IWriter
    {
        public void Write(string text) => Console.Write(text);

        public void WriteLine(string text) => Console.WriteLine(text);
    }
}
