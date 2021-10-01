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

        

        bool firstDrawCall = true;
        public void Draw(){
            if(firstDrawCall){
                objects.Sort();
                firstDrawCall = false;
            }

            int ScreenWidth = Console.WindowWidth;
            int ScreenHeight = Console.WindowHeight;
            for (int i = 0; i < cells.Count; i++) {
                Cell cell = (Cell)cells[i];
                bool l,r,t,b;
                l = isOurCell(cell.matrixX-1, cell.matrixY);
                r = isOurCell(cell.matrixX+1, cell.matrixY);
                t = isOurCell(cell.matrixX, cell.matrixY-1);
                b = isOurCell(cell.matrixX, cell.matrixY+1);
                for (int x = 0; x < cell.Width; x++) {
                    for (int y = 0; y < cell.Height; y++) {
                        Console.SetCursorPosition((cell.Width * cell.matrixX) + x, (cell.Height * cell.matrixY) + y);
                        char toWrite = ((string)objects[0])[0];

                        //if(x == 0 || x == cell.Width-1 || y == 0 || y == cell.Height-1){
                            if (!t && !l && x == 0 && y == 0) {
                                toWrite = '┌';
                            } else if(!t && !r && x == cell.Width-1 && y == 0) {
                                toWrite = '┐';
                            } else if(!l && !b && x == 0 && y == cell.Height-1) {
                                toWrite = '└';
                            } else if(!r && !b && x == cell.Width-1 && y == cell.Height-1) {
                                toWrite = '┘';
                            } else if((x == 0 && !l) || (x == cell.Width-1 && !r)) {
                                toWrite = '│';
                            } else if((y == 0 && !t) || (y == cell.Height-1 && !b)) {
                                toWrite = '─';
                            } else{
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                toWrite = i.ToString()[0];
                            }
                            Console.Write(toWrite);
                            Console.ResetColor();
                    }
                }
            }
        }

        // This function returns weather we own a
        // cell given it's coordinates in the matrix
        public bool isOurCell(int x, int y){
            foreach (Cell cell in cells) {
                if(cell.matrixX == x && cell.matrixY == y){
                    return true;
                }
            }
            return false;
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