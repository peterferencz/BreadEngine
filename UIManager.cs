using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace BreadEngine {
    public abstract class UIManager {
        
        // !IMPORTANT!
        // Due to file reading from top to bottom
        // the matrix is loaded from top to bottom
        // so top is actually y-1


        /// <returns>weather to pass trough the event</returns>
        public delegate bool KeyCallBack();


        private static Dictionary<ConsoleKey,KeyCallBack> universalKeyCodes = new Dictionary<ConsoleKey, KeyCallBack>();
        private static ArrayList matrix;
        private static Dictionary<char, ArrayList> identifiers;
        private static Dictionary<char, Panel> panels = new Dictionary<char, Panel>();
        private static ArrayList navigation;
        private static int selectedIndex = 0;

        public static void addUniversalKeyBind(ConsoleKey key, KeyCallBack callback){
            universalKeyCodes.Add(key, callback);
        }

        public static Panel GetPanel(char identifier){
            return panels[identifier];
        }

        public static Component GetComponent(string uid){
            if(string.IsNullOrEmpty(uid)) { return null; }
            foreach (KeyValuePair<char,Panel> panel in panels) {
                Component _toReturn = panel.Value.GetComponent(uid);
                if (_toReturn != null) {
                    return _toReturn;
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the active working layout
        /// </summary>
        /// <param name="data">The data read by LayoutReader</param>
        public static void setLayout(LayoutData data){
            matrix = data.matrix;
            identifiers = data.identifiers;
            navigation = data.naviagation;
            panels.Clear();
            selectedIndex = 0;

            //Filling up panels
            foreach (KeyValuePair<char, ArrayList> identifier in identifiers) {
                panels.Add(identifier.Key, new Panel(identifier.Value, matrix.Count));
            }
        }

        public static void StartLoop() {
            int matrixHeight = matrix.Count;            
            int matrixWidth = ((char[])matrix[0]).Length;
            int screenWidth = FastConsole.Width;
            int screenHeight = FastConsole.Height;

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
            panels[(char)navigation[0]].selectedIndex = (int)panels[(char) navigation[0]].interactableComponents[0];
            while (true) {
                FastConsole.Clear();
                foreach (KeyValuePair<char, Panel> panel in panels) {
                    panel.Value.Draw();
                }
                FastConsole.Flush();

                ConsoleKeyInfo keyInfo = FastConsole.ReadKey();
                ConsoleKey key = keyInfo.Key;

                //call universal keybinds
                if (universalKeyCodes.ContainsKey(key)) {
                    bool passTrough = universalKeyCodes[key].Invoke();
                    if (!passTrough) {
                        continue; //INFO if passtrough is true, more events can be rased by the same keybind
                    }
                }

                //Sending onkey down to components
                switch (panels[(char)navigation[selectedIndex]].OnKey(keyInfo)) {
                    case PanelNavigationAction.Stay:
                        //Do nothing
                        break;
                    case PanelNavigationAction.NextPanel:
                        do {
                            if(++selectedIndex >= navigation.Count) {
                                selectedIndex = 0;
                            }
                        } while(panels[(char) navigation[selectedIndex]].interactableComponents.Count == 0);
                        panels[(char)navigation[selectedIndex]].selectedIndex = (int)(panels[(char) navigation[selectedIndex]].interactableComponents[0]);
                        break;
                    case PanelNavigationAction.PreviousPanel:
                        do {
                            if(--selectedIndex <= -1) {
                                selectedIndex = navigation.Count - 1;
                            }
                        } while(panels[(char) navigation[selectedIndex]].interactableComponents.Count == 0);
                        panels[(char)navigation[selectedIndex]].selectedIndex = (int)panels[(char)navigation[selectedIndex]].interactableComponents[ panels[(char) navigation[selectedIndex]].interactableComponents.Count - 1 ];
                        break;
                    default:
                        break;
                }
            }
        }


        // This method returns the sector
        // corresponding in the matrix
        // defined in the layout
        // static char getSectorFromMatrix(int x, int y) {
        //     return ((char[])matrix[y])[x];
        // }

        /// This method returns the character representing
        /// the given panel inside the layout file given the
        /// screen x and y coordinates
        // static char GetSectorFromPixel(int x, int y) {
        //     int matrixHeight = matrix.Count;            
        //     int matrixWidth = ((char[])matrix[0]).Length;
        //     int screenWidth = FastConsole.Width;
        //     int screenHeight = FastConsole.Height;

        //     int sectionWidth = screenWidth / matrixWidth;
        //     int sectionHeight = screenHeight / matrixHeight;

        //     int xSection = Math.Clamp(x / sectionWidth, 0, matrixWidth-1);
        //     int ySection = Math.Clamp(y / sectionHeight, 0, matrixHeight-1);

        //     if(x < 0 || x > screenWidth-1 || y < 0 || y > screenHeight-1){
        //         return '\0';
        //     }

        //     return ((char[])(matrix[ySection]))[xSection];
        // }

    }
}