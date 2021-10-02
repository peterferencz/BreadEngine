using System;

namespace tui_generator
{
    public class Component {
        
        public string textRepresentation{get;set;}

        public ConsoleColor foreground{get;set;}
        public ConsoleColor background{get;set;}
    }

    public class Text : Component {
        public Text(string text){
            textRepresentation = text;
            foreground = ConsoleColor.White;
            background = ConsoleColor.Black;
        }
    }

    public class Spacer : Text{
        public Spacer(): base(text:"---"){ //Only for debug purpouses, Should be empty
            
        }
    }

    public class Button : Component {
        public Button(string text){
            textRepresentation = text;
            foreground = ConsoleColor.Black;
            background = ConsoleColor.White;
        }
    }
}