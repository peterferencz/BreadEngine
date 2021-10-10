using System;
using System.Collections;

namespace tui_generator
{
    public class Panel
    {
        public ArrayList cells = new ArrayList();
        private ArrayList components = new ArrayList();
        private ArrayList cellsVerticalMatrix = new ArrayList();
        public ConsoleColor borderColor = ConsoleColor.White;

        public int selectedIndex = -1;

        private int panelCount;
        public Panel(ArrayList components, int panelCount){
            this.components = components;
            this.panelCount = panelCount;

            //To prevent being empty and throwing errors
            if(components.Count == 0){
                components.Add(new Spacer());
            }
        }

        

        bool firstDrawCall = true;
        public void Draw(){
            if(firstDrawCall){
                //Fill the cellsverticalmatrix with empty arrays
                for (int i = 0; i < /*Count of rows, need outsideinfo*/panelCount; i++) {
                    cellsVerticalMatrix.Add(new ArrayList());
                }
                //Add cells to cellsverticalmatrix based on their
                //matrixY position
                for (int i = 0; i < cells.Count; i++) {
                    Cell cell = (Cell)cells[i];
                    ((ArrayList)cellsVerticalMatrix[cell.matrixY]).Add(cell);
                }
                //remove the elements from the arraylist
                //with the help of another arrylist
                ArrayList newArray = new ArrayList();
                for (int i = 0; i < cellsVerticalMatrix.Count; i++) {
                    if (((ArrayList)cellsVerticalMatrix[i]).Count != 0) {
                        newArray.Add(cellsVerticalMatrix[i]);
                    }
                }
                cellsVerticalMatrix = newArray;

                firstDrawCall = false;
            }

            int ScreenWidth = Console.WindowWidth;
            int ScreenHeight = Console.WindowHeight;

            //Handling of the title and removing it
            //from the regular components arraylist
            bool hasTitle = false;
            string titlestring = "";
            Component title = null;
            int titleIndex = 0;
            foreach (Component component in components) {
                if(typeof(Title) == component.GetType()){
                    hasTitle = true;
                    title = component;
                    components.Remove(component);
                    break;
                }
            }
            

            int objectIndex = 0;
            int textIndex = 0;
            for (int rowCount = 0; rowCount < cellsVerticalMatrix.Count; rowCount++) {
                ArrayList currentRow = (ArrayList)cellsVerticalMatrix[rowCount];
                Cell lastCell = (Cell)currentRow[currentRow.Count-1];
                for (int y = 0; y < ((Cell)currentRow[0]).Height; y++) {
                    for (int cellIndex = 0; cellIndex < currentRow.Count; cellIndex++) {
                        Cell cell = (Cell)currentRow[cellIndex];
                        bool l,r,t,b;
                        l = isOurCell(cell.matrixX-1, cell.matrixY);
                        r = isOurCell(cell.matrixX+1, cell.matrixY);
                        t = isOurCell(cell.matrixX, cell.matrixY-1);
                        b = isOurCell(cell.matrixX, cell.matrixY+1);
                        for (int x = 0; x < cell.Width; x++) {
                            char toWrite = '*';

                            Console.ForegroundColor = borderColor;
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
                            } else if(y == 0 && !t) {
                                if(hasTitle){
                                    //Drawing titles
                                    if(x == 1) {
                                        //First char of title
                                        titlestring = new String(title.Draw(cell.Width * currentRow.Count - 2));
                                    }
                                    if(titleIndex >= titlestring.Length){
                                        toWrite = '─';
                                    }else{
                                        toWrite = titlestring[titleIndex];
                                        titleIndex++;
                                    }
                                }else{
                                    toWrite = '─';
                                }
                            } else if(y == cell.Height-1 && !b) {
                                toWrite = '─';
                            } else {
                                Console.ResetColor();
                                if(objectIndex >= components.Count) {
                                    toWrite = ' ';
                                } else {
                                    Component component = (Component)components[objectIndex];
                                    string objString = new String(component.Draw(cell.Width * currentRow.Count - 2));
                                    Console.ForegroundColor = component.foreground;
                                    if(selectedIndex == objectIndex){
                                        Console.BackgroundColor = ConsoleColor.Blue;
                                    }else{
                                        Console.BackgroundColor = component.background;
                                    }
                                    
                                    #region Drawing of string onto the screen
                                    if(textIndex >= objString.Length){
                                        //If textIndex out of range
                                        if(cell.Equals(lastCell) && x == cell.Width-2) {
                                            //Last char of line
                                            if (textIndex < objString.Length) {
                                                //Haven't finished string
                                            } else {
                                                //Finished string
                                                textIndex = 0;
                                                objectIndex++;
                                            }
                                            toWrite = ' ';
                                        } else {
                                            toWrite = ' ';
                                        }
                                    } else {
                                        //Textindex is not out of range
                                        if(textIndex == objString.Length-1 && cell.Equals(lastCell) && x == cell.Width-2) {
                                                toWrite = objString[textIndex];
                                                textIndex = 0;
                                                objectIndex++;
                                        } else {
                                            toWrite = objString[textIndex];
                                            textIndex++;
                                        }
                                    }
                                    #endregion
                                }
                            }
                            
                            Console.SetCursorPosition((cell.Width * cell.matrixX) + x, (cell.Height * cell.matrixY) + y);
                            Console.Write(toWrite);
                            Console.ResetColor();
                        }
                    }
                }
            }
        }

        public bool OnKey(ConsoleKey key){
            if(((Component)components[selectedIndex]).OnKey(key)){
                //Go to next cmponent
                if(++selectedIndex >= components.Count){
                    //Run out of components
                    selectedIndex = -1;
                    return true;
                }
            }
            return false;
        }

        // This function returns weather we own a
        // cell given it's coordinates in the matrix
        public bool isOurCell(int x, int y) {
            foreach (Cell cell in cells) {
                if(cell.matrixX == x && cell.matrixY == y) {
                    return true;
                }
            }
            return false;
        }
    }

    public struct Cell{
        public int Width, Height, matrixX, matrixY;

        public override bool Equals(object obj) {
            if(obj.GetType() == this.GetType()) {
                Cell other = (Cell)obj;
                return (matrixX == other.matrixX) && (matrixY == other.matrixY);
            } else {
                return false;
            }
        }

        //Put this here so that the warning goes away
        public override int GetHashCode() {
            return HashCode.Combine(Width, Height, matrixX, matrixY);
        }
    }
}