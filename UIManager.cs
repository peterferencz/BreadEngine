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
        private static Dictionary<char,Panel> panels = new Dictionary<char, Panel>();

        public static void Loop(ReadReturn data){
            matrix = data.matrix;
            identifiers = data.identifiers;
            Console.Clear();
            Console.SetCursorPosition(0,0);

            foreach (KeyValuePair<char,ArrayList> identifier in identifiers) {
                Console.WriteLine(identifier.Key);
                if(!(identifier.Value.Count > 0)){
                    identifiers[identifier.Key].Add("spacer"); // Don't allow empty arraylists
                }
                panels.Add(identifier.Key, new Panel(identifier.Value));
            }

            
            
            int matrixHeight = matrix.Count;            
            int matrixWidth = ((char[])matrix[0]).Length;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;

            int sectionWidth = screenWidth / matrixWidth;
            int sectionHeight = screenHeight / matrixHeight;

            Console.WriteLine($"Width: {matrixWidth} Height: {matrixHeight}");

            for (int x = 0; x < matrixWidth; x++) {
                for (int y = 0; y < matrixHeight; y++) {
                    panels[  ((char[])matrix[y])[x]  ].cells.Add(new Cell(){
                        matrixX = x,
                        matrixY = y,
                        Width = sectionWidth,
                        Height = sectionHeight
                    });
                }
            }

            Console.Clear();
            foreach (KeyValuePair<char, Panel> panel in panels)
            {
                panel.Value.Draw();
                // foreach (Cell cell in panel.Value.cells)
                // {
                //     Console.WriteLine($"Cell {cell.matrixX},{cell.matrixY} is of dimensions {cell.Width}:{cell.Height} and section {((char[])matrix[cell.matrixY])[cell.matrixX]}");
                // }
            }
            
            // for (int y = 0; y < screenHeight; y++)
            // {
            //     char prevSection = '\0';
            //     int xIndex = 0;
            //     for (int x = 0; x < screenWidth; x++)
            //     {
            //         bool l,r,t,b;
            //         char current = GetSectorFromPixel(x,y);
            //         l = (GetSectorFromPixel(x-1,y) != current);
            //         r = (GetSectorFromPixel(x+1,y) != current);
            //         b = (GetSectorFromPixel(x,y+1) != current);
            //         t = (GetSectorFromPixel(x,y-1) != current);

            //         char toWrite = ' ';
            //         if(!t && !b && (l || r)){
            //             toWrite = '│';
            //         }else if(!l && !r && (t || b)){
            //             toWrite = '─';
            //         } else if(!b && r && !l && t){
            //             toWrite = '┐';
            //         } else if(!b && l && !r && t){
            //             toWrite = '┌';
            //         } else if(!t && !r && l && b){
            //             toWrite = '└';
            //         } else if(!t && !l && r && b){
            //             toWrite = '┘';
            //         } else{
            //             //Emty space
            //             //inside of panel 'current'
            //             if(prevSection != current){
            //                 //Start of new Section
            //                 prevSection = current;
            //                 xIndex = 0;
            //             }
                        
                        
            //             string s = (string)(identifiers[current] [identifierLine[current]]);
            //             string currentObject = (string)identifiers[current][identifierLine[current]];
            //             toWrite = s[xIndex];
            //             //identifierLine[current] = identifierLine[current] + 1;
                        
            //             if(xIndex < s.Length-1){
            //                 xIndex++;
            //             }else{
            //                 xIndex = 0;
            //                 if(identifierLine[current] < identifiers[current].Count-1){
            //                     identifierLine[current] = identifierLine[current] + 1;
            //                 }else{
            //                     toWrite = ' ';
            //                 }
            //             }

            //             //toWrite = GetSectorFromPixel(x,y);
            //         }

            //         Console.Write(toWrite);
            //     }
            // }
            Console.SetCursorPosition(0,0);
            Console.Read();
        }


        // This method returns the sector
        // corresponding in the matrix
        // defined in the layout
        static char getSectorFromMatrix(int x, int y){
            return ((char[])matrix[y])[x];
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