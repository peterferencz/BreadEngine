using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace tui_generator {
    public class UIManager {
        
        // !IMPORTANT!
        // Due to file reading from top to bottom
        // the matrix is loaded from top to bottom
        // so top is actually y-1

        private static ArrayList matrix;
        private static Dictionary<char, ArrayList> identifiers;
        private static Dictionary<char, Panel> panels = new Dictionary<char, Panel>();
        private static ArrayList navigation;
        private static int selectedIndex = 0;

        public static void StartLoop(ReadReturn data) {
            matrix = data.matrix;
            identifiers = data.identifiers;
            navigation = data.naviagation;

            foreach (KeyValuePair<char, ArrayList> identifier in identifiers) {
                panels.Add(identifier.Key, new Panel(identifier.Value, matrix.Count));
            }

            
            
            int matrixHeight = matrix.Count;            
            int matrixWidth = ((char[])matrix[0]).Length;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;

            int sectionWidth = screenWidth / matrixWidth;
            int sectionHeight = screenHeight / matrixHeight;

            int paddingY = screenHeight - matrixHeight * sectionHeight;

            //Filling up each panel with it's corresponding cells
            for (int x = 0; x < matrixWidth; x++) {
                for (int y = 0; y < matrixHeight; y++) {
                    panels[  ((char[])matrix[y])[x]  ].cells.Add(new Cell() {
                        matrixX = x,
                        matrixY = y,
                        Width = sectionWidth,
                        Height = sectionHeight
                    });
                }
            }

            //Set the first element
            panels[(char)navigation[0]].selectedIndex = 0;
            while (true) {
                Console.Clear();
                foreach (KeyValuePair<char, Panel> panel in panels) {
                    panel.Value.Draw();
                }

                Console.SetCursorPosition(0,0);
                ConsoleKey key = Console.ReadKey().Key;
                if(panels[(char)navigation[selectedIndex]].OnKey(key)){
                    if(++selectedIndex >= navigation.Count){
                        selectedIndex = 0;
                    }
                    panels[(char)navigation[selectedIndex]].selectedIndex = 0;
                }
            }
        }


        // This method returns the sector
        // corresponding in the matrix
        // defined in the layout
        static char getSectorFromMatrix(int x, int y) {
            return ((char[])matrix[y])[x];
        }

        /// This method returns the character representing
        /// the given panel inside the layout file given the
        /// screen x and y coordinates
        static char GetSectorFromPixel(int x, int y) {
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