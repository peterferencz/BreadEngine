namespace BreadEngine {
    class Program {
        static void Main(string[] args) {
            FastConsole.Clear();
            FastConsole.Flush();

            ReadReturn ret = FileReader.Read(".Layout");
            UIManager.StartLoop(ret);
            
            FastConsole.SetCursor(0,0);
            FastConsole.Write("Program Stopped execution");
            FastConsole.Flush();
            FastConsole.ReadKey();
        }
    }
}