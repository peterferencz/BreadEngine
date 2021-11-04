using System;
using System.Text;
using System.Linq;

namespace BreadEngine {

    public enum ComponentNavigationAction{
        Stay,
        NextComponent,
        PreviousComponent,
        NextPanel,
        PreviousPanel
    }

    public class Component {
        public ConsoleColor foreground = ConsoleColor.White;
        public ConsoleColor background = ConsoleColor.Black;

        public virtual char[] Draw(int width){
            return new char[width];
        }

        public virtual ComponentNavigationAction OnKey(ConsoleKeyInfo keyInfo){
            if(keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.Tab){
                return ComponentNavigationAction.NextComponent;
            }else if(keyInfo.Key == ConsoleKey.UpArrow){
                return ComponentNavigationAction.PreviousComponent;
            } else {
                return ComponentNavigationAction.Stay;
            }
        }
    }

    public class Text : Component {
        public string text;

        public Text(string text) {
            this.text = text;
            foreground = ConsoleColor.White;
            background = ConsoleColor.Black;
        }

        public override char[] Draw(int width){
            char[] toReturn = new char[text.Length];
            for (int i = 0; i < text.Length; i++) {
                toReturn[i] = text[i];
            }
            return toReturn;
        }
    }

    public class Title : Component {
        public string text;

        public Title(string text) {
            this.text = text;
            foreground = ConsoleColor.White;
            background = ConsoleColor.Black;
        }
        
        public override char[] Draw(int width){
            char[] toReturn = new char[width];
            for (int i = 0; i < width; i++) {
                if(i == 0){
                    toReturn[i] = '─';
                }else{
                    if(i <= text.Length){
                        toReturn[i] = text[i-1];
                    } else {
                        toReturn[i] = '─';
                    }
                }
            }
            return toReturn;
        }
    }

    public class Spacer : Component{
        public string text;

        string spacer;

        public Spacer(string spaceWith = " ") {
            spacer = spaceWith;
        }

        public override char[] Draw(int width) {
            char[] toReturn = new char[width];
            for (int i = 0; i < width; i++) {
                toReturn[i] = spacer[i%spacer.Length];
            }
            return toReturn;
        }
    }

    public class Button : Component {
        public string text;

        public Button(string text) {
            this.text = text;
            foreground = ConsoleColor.Green;
        }

        public override char[] Draw(int width){
            char[] toReturn = new char[text.Length];
            for (int i = 0; i < text.Length; i++) {
                toReturn[i] = text[i];
            }
            return toReturn;
        }
    }


    //TODO a loader that writes it's value next to it
    public class Slider : Component {
        public string text;
        public int percent = 90;

        public Slider(){
            text = "LoadBarWithText";
            foreground = ConsoleColor.Cyan;
        }

        public override char[] Draw(int width) {
            char[] toReturn = new char[width];
            string percent_str = percent.ToString().PadLeft(3, '0');
            for (int i = 0; i < width; i++) {
                switch (i) {
                    case 0:
                        toReturn[i] = '[';
                        break;
                    case 1:
                        toReturn[i] = percent_str[0];
                        break;
                    case 2:
                        toReturn[i] = percent_str[1];
                        break;
                    case 3:
                        toReturn[i] = percent_str[2];
                        break;
                    case 4:
                        toReturn[i] = ']';
                        break;
                    case 5:
                        toReturn[i] = '[';
                        break;
                    default:
                        if(i == width-1){
                            toReturn[i] = ']';
                        }else if(i-7 < ((width-8) * percent / 100)) { // width-7 100       000000111111111110
                            toReturn[i] = '=';
                        } else {
                            toReturn[i] = ' ';
                        }
                        break;
                }
            }
            //width: 100
            // 
            return toReturn;
        }

        public override ComponentNavigationAction OnKey(ConsoleKeyInfo keyInfo){
            if(keyInfo.Key == ConsoleKey.LeftArrow) {
                percent -= (0 < percent)? 1: 0;
                return ComponentNavigationAction.Stay;
            } else if(keyInfo.Key == ConsoleKey.RightArrow) {
                percent += (percent < 100)? 1: 0;
                return ComponentNavigationAction.Stay;
            } else{
                return base.OnKey(keyInfo);
            }
        }
    }

    public class LoadBar : Component {
        public int percent = 90;

        public LoadBar(){
            foreground = ConsoleColor.Blue;
        }

        public override char[] Draw(int width) {
            char[] toReturn = new char[width];
            for (int i = 0; i < width; i++) {
                if(i == 0) {
                    toReturn[i] = '[';
                } else if(i == width-1){
                    toReturn[i] = ']';
                } else {
                    if(i < (width * percent / 100)) {
                        toReturn[i] = '=';
                    } else {
                        toReturn[i] = ' ';
                    }
                }
            }
            return toReturn;
        }
    }
public class TextBox : Component {
        
        public string text = "";

        public override char[] Draw(int width){
            char[] toReturn = new char[text.Length];
            for (int i = 0; i < text.Length; i++) {
                toReturn[i] = text[i];
            }
            return toReturn;
        }

        public override ComponentNavigationAction OnKey(ConsoleKeyInfo keyInfo){
            if(keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.Tab){
                return ComponentNavigationAction.NextComponent;
            }else if(keyInfo.Key == ConsoleKey.UpArrow){
                return ComponentNavigationAction.PreviousComponent;
            } else {
                switch (keyInfo.Key) {
                    case ConsoleKey.Enter:
                    case ConsoleKey.End:
                    case ConsoleKey.Home:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        break;
                    case ConsoleKey.Backspace:
                        text = text.Substring(0, text.Length-1);
                        break;

                    default:
                        text += keyInfo.KeyChar;
                        break;
                }
                return ComponentNavigationAction.Stay;
            }
        }
    }
}