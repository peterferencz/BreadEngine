using System;
using System.Text;
using System.Linq;

namespace BreadEngine {
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

        public bool OnKey(ConsoleKey key){
            if(key == ConsoleKey.DownArrow || key == ConsoleKey.Tab){
                return true;
            }else{
                return false;
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
}