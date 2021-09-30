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
                        char toWrite = ((string)objects[0])[0];
                        
                        bool l,r,t,b;
                        l = isOurCell(cell.matrixX-1, cell.matrixY);
                        r = isOurCell(cell.matrixX+1, cell.matrixY);
                        t = isOurCell(cell.matrixX, cell.matrixY-1);
                        b = isOurCell(cell.matrixX, cell.matrixY+1);

                        if(x == 0 || x == cell.Width-1 || y == 0 || y == cell.Height-1){
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
                                toWrite = ' ';
                            }
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(toWrite);
                            Console.ForegroundColor = ConsoleColor.White;
                        } else{
                            Console.Write( ' ' );
                        }

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