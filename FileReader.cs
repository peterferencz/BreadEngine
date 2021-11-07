using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BreadEngine {
    public struct ReadReturn {
        public ArrayList matrix;
        public Dictionary<char,ArrayList> identifiers;

        public ArrayList naviagation;
    }


    public class FileReader {

        /// This method reads a file, parses it
        /// and returns the data in a special
        /// struct. It also throws errors when
        /// finding them
        public static ReadReturn Read(string path) {
            try {
            using (var sr = new StreamReader(path)) {
                bool inLayout = false;
                bool inNavigation = false;
                int layoutWidth = -1;
                char currentIdentifier = '\0';
                bool previousLineIsEmpty = false;
                ArrayList matrix = new ArrayList();
                Dictionary<char,ArrayList> identifiers = new Dictionary<char, ArrayList>();
                ArrayList navigation = new ArrayList();

                int lineCounter = 0;
                while (sr.Peek() >= 0) {
                    lineCounter++;
                    string line = sr.ReadLine().Trim().ToLower(); //TODO don't make all lowercase
                    if(String.IsNullOrWhiteSpace(line)) {
                        inLayout = false;
                        inNavigation = false;
                        currentIdentifier = '\0';
                        previousLineIsEmpty = true;
                        continue;
                    }

                    if(line == ":layout") {
                        if(inNavigation){
                            ThrowError($"Start of a new identifier should always be seperated with an empty line (line {lineCounter})");
                        }
                        inNavigation = false;
                        inLayout = true;
                        continue;
                    } else if(line == ":nav") {
                        if(inLayout){
                            ThrowError($"Start of a new identifier should always be seperated with an empty line (line {lineCounter})");
                        }
                        inLayout = false;
                        inNavigation = true;
                        continue;
                    } else if(line.StartsWith(":")) {
                        if(inLayout || inNavigation || !previousLineIsEmpty){
                            ThrowError($"Start of a new identifier should always be seperated with an empty line (line {lineCounter})");
                        }
                        inLayout = false; //TODO error handling: multiple sections after
                        inNavigation = false; // each other can cause wired effect
                        try {
                            char selector = Char.Parse(line.Substring(1));
                            if(!identifiers.ContainsKey(selector)) {
                                ThrowError($"Identifier '{selector}' was not declared in layout (line {lineCounter})");
                            } else if(identifiers[selector].Count != 0){
                                ThrowError($"Identifier is defined on multiple occasions (line {lineCounter})");
                            }
                            currentIdentifier = selector;
                        } catch (Exception) {
                            ThrowError($"Identifier couldn't be parsed into a character (line {lineCounter})");
                        }
                        
                    } else {
                        if(inLayout) {
                            string[] sections = line.Split(' ');
                            char[] _sections = new char[sections.Length];
                            for (int i = 0; i < sections.Length; i++) {
                                try {
                                    _sections[i] = Char.Parse(sections[i]);
                                    if(_sections[i] == ':') {
                                        ThrowError($"Illegal layout identifier. Identifier cannot be ':' (line {lineCounter})");
                                    }
                                    if(!identifiers.ContainsKey(_sections[i])) {
                                        identifiers.Add(_sections[i], new ArrayList());
                                    }
                                }catch(Exception) {
                                    ThrowError($"Error parsing layout: '{sections[i]}' is not a character (line {lineCounter})");
                                }
                            }
                            if(layoutWidth == -1) {
                                layoutWidth = _sections.Length;
                            } else if(layoutWidth != _sections.Length) {
                                ThrowError($"Layout length is not consistent troughouth lines (line {lineCounter})");
                            }
                            matrix.Add(_sections);
                        } else if(currentIdentifier != '\0') {
                            //Parsing of components
                            identifiers[currentIdentifier].Add(parseComponent(line, lineCounter));
                            
                        } else if(inNavigation){
                            string[] lineSections = line.Split(' ');
                            for (int i = 0; i < lineSections.Length; i++) {
                                if(lineSections[i].Length > 1){
                                    ThrowError($"Navigationcomponent must be of type char (line {lineCounter})");
                                } else if(!identifiers.ContainsKey(lineSections[i][0])){
                                    ThrowError($"No identifier resembles char '{lineSections[i][0]}'");
                                } else {
                                    navigation.Add(lineSections[i][0]);
                                }
                            }
                        } else {
                            ThrowError($"Cannot understand line {lineCounter} ('{line}')");
                        }
                    }
                    previousLineIsEmpty = false;
                }

                //TODO check if all regions aren't cut off
                // 1 0 0
                // 0 1 0  Like this
                // 0 1 0

                return new ReadReturn() {
                    matrix = matrix,
                    identifiers = identifiers,
                    naviagation = navigation
                };
            }
            } catch (IOException e) {
                ThrowError($"Error reading file: {e.Message}");
                return new ReadReturn();
            }
        }


        private static Component parseComponent(string line, int lineCounter){
            int OpeningBracketIndex = line.IndexOf('(');
            int ClosingBracketIndex = line.LastIndexOf(')');
            int HashIndex = line.IndexOf('#');
            bool hasId = false;


            Component component;
            string uid = null;
            if(OpeningBracketIndex == -1) {
                if (ClosingBracketIndex != -1) {
                    ThrowError($"Found ')', but no '(' (line {lineCounter})");
                    return null;
                }

                hasId = HashIndex != -1;
                if (hasId)
                    uid = line.Substring(HashIndex+1);
                
                //No parameters
                string componentname = ((hasId) ?  line.Substring(0, HashIndex) : line).ToLower().Trim();
                switch (componentname) {
                    case "text":
                        component = new Text(getParameter(line, lineCounter));
                        break;
                    case "button":
                        component = new Button(getParameter(line, lineCounter));
                        break;
                    case "title":
                        //TODO check for multiple titles
                        component = new Title(getParameter(line, lineCounter));
                        break;
                    case "loader":
                        component = new LoadBar();
                        break;
                    case "slider":
                        component = new Slider();
                        break;
                    case "textbox":
                        component = new TextBox();
                        break;
                    case "spacer":
                        component = new Spacer();
                        break;
                    default:
                        ThrowError($"Unrecognized component on line {lineCounter}");
                        return null;
                }
            }else{
                //With Parameters
                hasId = HashIndex != -1 && HashIndex < OpeningBracketIndex;
                if (hasId)
                    uid = line.Substring(HashIndex+1, OpeningBracketIndex - HashIndex-1);
                string componentname = line.ToLower().Trim().Substring(0, OpeningBracketIndex - ((hasId)?HashIndex-1:0));
                switch (componentname) {
                    case "text":
                        component = new Text(getParameter(line, lineCounter));
                        break;
                    case "button":
                        component = new Button(getParameter(line, lineCounter));
                        break;
                    case "title":
                        //TODO check for multiple titles
                        component = new Title(getParameter(line, lineCounter));
                        break;
                    case "spacer":
                        component = new Spacer(getParameter(line,lineCounter));
                        break;
                    default:
                        ThrowError($"Unrecognized component on line {lineCounter}");
                        return null;
                }
            }

            //Adding id
            if (hasId) {
                component.uid = uid;
            }
            return component;
        }

        //Extracts the string from inside of
        // the caracters '(' and ')'
        private static string getParameter(string line, int lineCount) {
            int from = line.IndexOf('(');
            int to = line.LastIndexOf(')');
            if(from == -1) {
                ThrowError($"Missing Opening bracket on line {lineCount}");
                return "";
            } else if(to == -1) {
                ThrowError($"Missing Closing bracket on line {lineCount}");
                return "";
            } else {
                string toReturn = line.Substring(from+1, to - from-1);
                if(toReturn.Length == 0) {
                    ThrowError($"Empty parameter on line {lineCount}");
                }
                return toReturn;
            }
        }


        //Stops execution and prompts to user
        private static void ThrowError(string message) {
            FastConsole.Write("An error occured: ", ConsoleColor.Red);
            FastConsole.Write(message, ConsoleColor.Red);
            FastConsole.Flush();
            Console.SetCursorPosition(0,1);
            Environment.Exit(1);
        }
    }
}