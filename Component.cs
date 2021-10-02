using System;

namespace tui_generator
{
    public class Component {
        
        public string textRepresentation{get;set;}

        public ConsoleColor foreground{get;set;}
        public ConsoleColor background{get;set;}

        public virtual char Draw(int x, int width){
            return '-';
        }
    }

    public class Text : Component {
        public Text(string text){
            textRepresentation = text;
            foreground = ConsoleColor.White;
            background = ConsoleColor.Black;
        }
    }

    public class Spacer : Text{
        public Spacer(): base(text:""){
            
        }
    }

    public class Button : Component {
        public Button(string text){
            textRepresentation = text;
            foreground = ConsoleColor.Black;
            background = ConsoleColor.White;
        }
    }

    public class LoadBar : Component {
        public LoadBar(){
            textRepresentation = "[======>           ]";
            foreground = ConsoleColor.Black;
            background = ConsoleColor.White;
        }
    }
}