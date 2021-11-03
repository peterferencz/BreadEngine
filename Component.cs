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
        
        public string textRepresentation = "Default component";

        public ConsoleColor foreground = ConsoleColor.White;
        public ConsoleColor background = ConsoleColor.Black;

        public virtual char[] Draw(int width){
            char[] toReturn = new char[textRepresentation.Length];
            for (int i = 0; i < textRepresentation.Length; i++) {
                toReturn[i] = textRepresentation[i];
            }
            return toReturn;
        }

        public virtual ComponentNavigationAction OnKey(ConsoleKey key){
            if(key == ConsoleKey.DownArrow || key == ConsoleKey.Tab){
                return ComponentNavigationAction.NextComponent;
            }else if(key == ConsoleKey.UpArrow){
                return ComponentNavigationAction.PreviousComponent;
            } else {
                return ComponentNavigationAction.Stay;
            }
        }
    }

    public class Text : Component {
        public Text(string text) {
            textRepresentation = text;
            foreground = ConsoleColor.White;
            background = ConsoleColor.Black;
        }

        
    }

    public class Title : Component {

        public Title(string text) {
            textRepresentation = text;
            foreground = ConsoleColor.White;
            background = ConsoleColor.Black;
        }
        
        public override char[] Draw(int width){
            char[] toReturn = new char[width];
            for (int i = 0; i < width; i++) {
                if(i == 0){
                    toReturn[i] = '─';
                }else{
                    if(i <= textRepresentation.Length){
                        toReturn[i] = textRepresentation[i-1];
                    } else {
                        toReturn[i] = '─';
                    }
                }
            }
            return toReturn;
        }
    }

    public class Spacer : Component{
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
        public Button(string text) {
            textRepresentation = text;
            foreground = ConsoleColor.Green;
        }
    }


    //TODO a loader that writes it's value next to it
    public class Slider : Component {
        public int percent = 90;

        public Slider(){
            textRepresentation = "LoadBarWithText";
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

        public override ComponentNavigationAction OnKey(ConsoleKey key){
            if(key == ConsoleKey.LeftArrow) {
                percent -= (0 < percent)? 1: 0;
                return ComponentNavigationAction.Stay;
            } else if(key == ConsoleKey.RightArrow) {
                percent += (percent < 100)? 1: 0;
                return ComponentNavigationAction.Stay;
            } else{
                return base.OnKey(key);
            }
        }
    }

    public class LoadBar : Component {

        public int percent = 90;

        public LoadBar(){
            textRepresentation = "LoadBar";
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

        public override ComponentNavigationAction OnKey(ConsoleKey key){
            if(key == ConsoleKey.DownArrow || key == ConsoleKey.Tab){
                return ComponentNavigationAction.NextComponent;
            }else if(key == ConsoleKey.UpArrow){
                return ComponentNavigationAction.PreviousComponent;
            } else {
                text += ((char)key);
                return ComponentNavigationAction.Stay;
            }
        }
    }
}