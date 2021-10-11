namespace BreadEngine {
    class Program {
        static void Main(string[] args) {
            FastConsole.Clear();
            FastConsole.Flush();

            ReadReturn ret = FileReader.Read(".Layout");
            UIManager.StartLoop(ret);
            
            
            FastConsole.Write("Program Stopped execution");
            FastConsole.Flush();
            FastConsole.ReadKey();
        }
    }
}