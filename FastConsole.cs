using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace BreadEngine {
    public class FastConsole {
        static char[] buffer;
        private static int offset = 0;

        public static int Width{
            get{ return Console.WindowWidth; }
        }
        public static int Height{
            get{ return Console.WindowHeight; }
        }

        static FastConsole(){
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.CursorVisible = false;

            Clear();
        }

        public static void SetCursor(int x, int y){
            // SetCursor((y * Height) + x);
            Console.SetCursorPosition(x,y);
        }
        
        private static void SetCursor(int offset){
            FastConsole.offset = offset;
        }


        public static void Write(char c){
            // Write(c.ToString());
            // Console.ForegroundColor = ConsoleColor.Blue;
            // Console.Write(c);
            // Console.BackgroundColor = ConsoleColor.Red;
            // Console.SetCursorPosition(offset % Width, offset / Width);
            // x: offset % Width;  y: offset / Width
            // Console.ResetColor();

            //0,0 0,1 0,2 0,3   0 1 2 3
            //1,0 1,1 1,2 1,3   4 5 6 7
            //2,0 2,1 2,2 2,3   8 9 11 12

            // buffer[offset++] = c;
            Flush();
            Console.Write(c);
            //
        }

        public static void Write(string s){
            // for (int i = 0; i < s.Length; i++) {
            //     buffer[offset++] = s[i];
            // }
            Console.Write(s);
        }

        public static void Clear(){
            buffer = Enumerable.Repeat('.', Width * Height).ToArray();
            // offset = 0;
            Console.Clear();
        }

        public static ConsoleKeyInfo ReadKey(){
            return Console.ReadKey();
        }

        public static void Flush() {
            // Console.SetCursorPosition(0,0);
            // Console.Clear();
            // Console.Write(new String(buffer));
        }
    }
}