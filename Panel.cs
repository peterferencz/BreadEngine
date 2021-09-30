using System;
using System.Collections;
using System.Collections.Generic;

namespace tui_generator
{
    public class Panel
    {
        public ArrayList cells = new ArrayList();

        private ArrayList objects = new ArrayList();
        public ArrayList cellsVerticalMatrix = new ArrayList();

        public Panel(ArrayList objects){
            this.objects = objects;
        }

        public void Draw(){
            int ScreenWidth = Console.WindowWidth;
            int ScreenHeight = Console.WindowHeight;
            foreach (Cell cell in cells) {
                
                
                for (int x = 0; x < cell.Width; x++) {
                    for (int y = 0; y < cell.Height; y++) {
                        Console.SetCursorPosition((cell.Width * cell.matrixX) + x, (cell.Height * cell.matrixY) + y);
                        Console.Write( ((string)objects[0])[0] );
                    }
                }
            }
            foreach (ArrayList arr in cellsVerticalMatrix){
                string s = "[ ";
                foreach (Cell c in arr) {
                    s += $"({c.matrixX},{c.matrixY}), ";
                }
                s += "]";
                Console.WriteLine(s);
            }
            {
                
            }
        }

        // [][][][]
        // X [][][]
        // X X X []
        // [][][][]
    }

    public struct Cell{
        public int Width, Height, matrixX, matrixY;
    }
}