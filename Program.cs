using System;

namespace tui_generator {
    class Program {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.SetCursorPosition(0,0);
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.CursorVisible = false;

            ReadReturn ret = FileReader.Read(".Layout");
            UIManager.StartLoop(ret);
            
            
            Console.WriteLine("Program Stopped execution");
            Console.Read();
            Console.Clear();
        }
    }
}