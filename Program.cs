using System;

namespace tui_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.SetCursorPosition(0,0);
            Console.CursorVisible = false;


            ReadReturn ret = FileReader.Read(".Layout");
            Console.WriteLine("Heh1");
            UIManager.Loop(ret);
            
        }
    }
}
