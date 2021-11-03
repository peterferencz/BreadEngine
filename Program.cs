using System;

namespace BreadEngine {
    class Program {
        static void Main(string[] args) {
            FastConsole.Clear();

            ReadReturn ret = FileReader.Read(".Layout");
            UIManager.addUniversalKeyBind(ConsoleKey.Escape,() => {
                Console.ResetColor();
                Console.Clear();
                Environment.Exit(1);
                return false;
            });

            UIManager.StartLoop(ret);
            
            FastConsole.SetCursor(0,0);
            FastConsole.Write("Program Stopped execution");
            FastConsole.Flush();
            FastConsole.ReadKey();
        }
    }
}