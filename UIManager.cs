using System;
using System.Collections;
using System.Collections.Generic;

namespace tui_generator
{

    public class UIManager
    {
        
        // !IMPORTANT!
        // Due to file reading from top to bottom
        // the matrix is loaded from top to bottom
        // so top is actually y-1

        private static ArrayList matrix;
        private static Dictionary<char,ArrayList> identifiers;

        public static void Loop(ReadReturn data){
            matrix = data.matrix;
            identifiers = data.identifiers;
            Console.Clear();
            Console.CursorVisible = false;

            Dictionary<char,int> identifierLine = new Dictionary<char, int>();
            foreach (KeyValuePair<char,ArrayList> identifier in identifiers) {
                identifierLine.Add(identifier.Key, 0);
            }
            
            int matrixHeight = matrix.Count;            
            int matrixWidth = ((char[])matrix[0]).Length;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;

            int sectionWidth = screenWidth / matrixWidth;
            int sectionHeight = screenHeight / matrixHeight;

            Console.WriteLine($"Width: {matrixWidth} Height: {matrixHeight}");
            Console.Clear();
            Console.SetCursorPosition(0,0);

            for (int y = 0; y < screenHeight; y++)
            {
                for (int x = 0; x < screenWidth; x++)
                {
                    bool l,r,t,b;
                    char current = GetSectorFromPixel(x,y);
                    l = (GetSectorFromPixel(x-1,y) != current);
                    r = (GetSectorFromPixel(x+1,y) != current);
                    b = (GetSectorFromPixel(x,y+1) != current);
                    t = (GetSectorFromPixel(x,y-1) != current);

                    char toWrite = ' ';
                    if(!t && !b && (l || r)){
                        toWrite = '│';
                    }else if(!l && !r && (t || b)){
                        toWrite = '─';
                    } else if(!b && r && !l && t){
                        toWrite = '┐';
                    } else if(!b && l && !r && t){
                        toWrite = '┌';
                    } else if(!t && !r && l && b){
                        toWrite = '└';
                    } else if(!t && !l && r && b){
                        toWrite = '┘';
                    } else{
                        //Emty space
                        //inside of panel 'current'
                        
                        
                        /*toWrite = ((string)(identifiers[current][identifierLine[current]]))[0];
                        identifierLine[current] = identifierLine[current] + 1;*/
                        
                        //toWrite = GetSectorFromPixel(x,y);
                    }

                    Console.Write(toWrite);
                }
            }
            Console.SetCursorPosition(0,0);
            Console.Read();
        }


        /// This method returns the character representing
        /// the given panel inside the layout file given the
        /// screen x and y coordinates
        static char GetSectorFromPixel(int x, int y){
            int matrixHeight = matrix.Count;            
            int matrixWidth = ((char[])matrix[0]).Length;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;

            int sectionWidth = screenWidth / matrixWidth;
            int sectionHeight = screenHeight / matrixHeight;

            int xSection = Math.Clamp(x / sectionWidth, 0, matrixWidth-1);
            int ySection = Math.Clamp(y / sectionHeight, 0, matrixHeight-1);

            if(x < 0 || x > screenWidth-1 || y < 0 || y > screenHeight-1){
                return '\0';
            }

            return ((char[])(matrix[ySection]))[xSection];
        }

    }
}