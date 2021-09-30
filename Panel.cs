using System;
using System.Collections;
using System.Collections.Generic;

namespace tui_generator
{
    public class Panel
    {
        public ArrayList cells = new ArrayList();
        public ArrayList matrix = new ArrayList();

        public void Draw(){
            int ScreenWidth = Console.WindowWidth;
            int ScreenHeight = Console.WindowHeight;

            foreach (Cell cell in cells) {
                for (int x = 0; x < cell.Width; x++) {
                    for (int y = 0; y < cell.Height; y++) {
                        Console.SetCursorPosition((cell.Width * cell.matrixX) + x, (cell.Height * cell.matrixY) + y);
                        Console.Write('?');
                    }
                }
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