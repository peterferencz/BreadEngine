using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BreadEngine {

    public struct ConsoleChar {
        public ConsoleColor ForeGroundColor;
        public ConsoleColor BackGroundColor;
        public char character;


        // public ConsoleChar(){
        //     ForeGroundColor = ConsoleColor.White;
        //     BackGroundColor = ConsoleColor.Black;
        //     character = ' ';
        // }
        public ConsoleChar(char character, ConsoleColor ForeGroundColor,ConsoleColor BackGroundColor){
            this.ForeGroundColor = ForeGroundColor;
            this.BackGroundColor = BackGroundColor;
            this.character = character;
        }

        public override bool Equals(object obj) {
            if (obj.GetType() == typeof(ConsoleChar)) {
                ConsoleChar c = (ConsoleChar)obj;
                return (c.ForeGroundColor == ForeGroundColor) && (c.BackGroundColor == BackGroundColor) && (c.character == character);
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }

    public class FastConsole {

        public static ConsoleChar backgroundChar = new ConsoleChar(' ', ConsoleColor.White, ConsoleColor.Black);

        static ConsoleChar[] currentBuffer;
        static ConsoleChar[] buffer;
        
        private static int offset = 0;
        //private static readonly BufferedStream outputStream;
        //private static StreamWriter streamWriter;

        public static int Width{
            get{ return Console.WindowWidth; }
        }
        public static int Height{
            get{ return Console.WindowHeight; }
        }

        static FastConsole(){
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.CursorVisible = false;

            //outputStream = new BufferedStream(Console.OpenStandardOutput(), Width * Height);
            // streamWriter = new StreamWriter(Console.OpenStandardOutput(), Encoding.Unicode, Width * Height * 4);
            // streamWriter.AutoFlush = false;
            
            currentBuffer = Enumerable.Repeat(new ConsoleChar(), Width * Height).ToArray();
            Clear();
        }

        public static void SetCursor(int x, int y){
            offset = y * Width + x;
            //Console.SetCursorPosition(x,y);
        }


        public static void Write(char c, ConsoleColor ForeGroundColor = ConsoleColor.White, ConsoleColor BackGroundColor = ConsoleColor.Black){
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

            buffer[offset++] = new ConsoleChar(c, ForeGroundColor, BackGroundColor);
            

            //Write(""+c);
            //Console.Write(c);
        }

        public static void Write(string s, ConsoleColor ForeGroundColor = ConsoleColor.White, ConsoleColor BackGroundColor = ConsoleColor.Black){
            for (int i = 0; i < s.Length; i++) {
                buffer[offset++] = new ConsoleChar(s[i], ForeGroundColor, BackGroundColor);
            }

            
            
            // offset += s.Length;
            // var rgb = new byte[s.Length << 1];
            // Encoding.Unicode.GetBytes(s, 0, s.Length, rgb, 0);

            // lock (outputStream) {  // (optional, can omit if appropriate)
            //     Console.WriteLine($"offset: {offset}; length: ");
            //     outputStream.Write(rgb, offset, rgb.Length);
            //     offset += rgb.Length;
            // }
            //Console.Write(s);
        }

        public static void Clear(){
            //Console.Clear();
            buffer = Enumerable.Repeat(backgroundChar, Width * Height).ToArray();
            offset = 0;
        }

        public static ConsoleKeyInfo ReadKey(){
            return Console.ReadKey(true);
        }

        public static void Flush() {
            Console.SetCursorPosition(0,0);
            for (int i = 0; i < buffer.Length; i++) {
                if(!currentBuffer[i].Equals(buffer[i])){
                    Console.SetCursorPosition(i % Width, i / Width);
                    Console.ForegroundColor = buffer[i].ForeGroundColor;
                    Console.BackgroundColor = buffer[i].BackGroundColor;
                    Console.Write(buffer[i].character);
                }
                
            }
            
            currentBuffer = buffer;
            

            //Console.Clear();
            //Console.Write('x');
            
            // byte[] bytes = Encoding.Unicode.GetBytes(buffer, 0, buffer.Length);
            

        }
    }
}