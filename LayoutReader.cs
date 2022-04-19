using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BreadEngine {

    /// <summary>
    /// Created when reading a layout file, contains parsed data
    /// </summary>
    public struct LayoutData {
        public ArrayList matrix;
        public Dictionary<char, ArrayList> identifiers;

        public ArrayList naviagation;
    }


    public abstract class LayoutReader {

        private enum state {
            LAYOUT,
            NAVIGATION,
            PANEL,
            NONE
        }

        /// <summary>
        /// This method reads a file, parses it and returns the data in a special struct
        /// It also throws errors when it finds one
        /// </summary>
        /// <param name="path">absolute path to the file to read from</param>
        /// <returns>Data related to the reading of the file</returns>
        public static LayoutData Read(string path) {
            string[] lines = { "Error", "shouldn't be able to read this" };
            try {
                string[] _lines = File.ReadAllLines(path);
                lines = _lines;
            } catch(Exception e) {
                ThrowError("[FileRead] " + e.Message, 0);
            }

            state currentState = state.NONE;
            object currentStateObject = null;
            int layoutWidth = -1;
            ArrayList matrix = new ArrayList();
            Dictionary<char, ArrayList> identifiers = new Dictionary<char, ArrayList>();
            ArrayList navigation = new ArrayList();

            for(int lineIndex = 0; lineIndex < lines.Length; lineIndex++) {
                string line = lines[lineIndex].Trim();
                if(string.IsNullOrEmpty(line)) {
                    currentState = state.NONE;
                    currentStateObject = null;
                    continue;
                } else if(line.ToLower() == ":layout") {
                    if(currentState != state.NONE) {
                        ThrowError("Cannot start new section when already in another; should be seperated by empty line", lineIndex);
                    }
                    currentState = state.LAYOUT;
                    currentStateObject = null;
                } else if(line.ToLower() == ":nav") {
                    if(currentState != state.NONE) {
                        ThrowError("Cannot start new section when already in another; should be seperated by empty line", lineIndex);
                    }
                    currentState = state.NAVIGATION;
                    currentStateObject = null;
                } else if(line.StartsWith(':')) {
                    if(currentState != state.NONE) {
                        ThrowError("Cannot start new section when already in another; should be seperated by empty line", lineIndex);
                    }
                    currentState = state.PANEL;
                    try {
                        char panelid = char.Parse(line.Substring(1));
                        if(!identifiers.ContainsKey(panelid)) {
                            ThrowError($"Identifier '{panelid}' was not declared in :layout", lineIndex);
                        } else if(identifiers[panelid].Count != 0) {
                            ThrowError("Identifier was already defined", lineIndex);
                        }
                        currentStateObject = panelid;
                    } catch(Exception) {
                        ThrowError("Identifier couldn't be parsed into a character", lineIndex);
                    }
                } else {
                    switch(currentState) {
                        case state.LAYOUT:
                            string[] stringSections = line.Split(' ');
                            char[] idSections = new char[stringSections.Length];
                            for(int i = 0; i < stringSections.Length; i++) {
                                try {
                                    char panelid = char.Parse(stringSections[i]);
                                    if(panelid == ':') {
                                        ThrowError($"Illegal panel identifier", lineIndex);
                                    }
                                    if(!identifiers.ContainsKey(panelid)) {
                                        identifiers.Add(panelid, new ArrayList());
                                    }
                                    idSections[i] = panelid;
                                } catch(Exception) {
                                    ThrowError($"Error parsing to character: '{stringSections[i]}' is not a character", lineIndex);
                                }
                            }
                            if(layoutWidth == -1) {
                                layoutWidth = idSections.Length;
                            } else if(layoutWidth != idSections.Length) {
                                ThrowError($"Layout should be rectangular (constant line widths)", lineIndex); //TODO we could make it place empty cells with no panel
                            }
                            matrix.Add(idSections);
                            break;
                        case state.NAVIGATION:
                            string[] lineSections = line.Split(' ');
                            for(int i = 0; i < lineSections.Length; i++) {
                                if(lineSections[i].Length > 1) {
                                    ThrowError($"Navigationcomponent must be of type char", lineIndex);
                                } else if(!identifiers.ContainsKey(lineSections[i][0])) {
                                    ThrowError($"No identifier resembles char '{lineSections[i][0]}'", lineIndex);
                                } else if(navigation.Contains(lineSections[i][0])) {
                                    ThrowError($"Navigation step '{lineSections[i][0]}' is already in the navigation flow", lineIndex);
                                } else {
                                    navigation.Add(lineSections[i][0]);
                                }
                            }
                            break;
                        case state.PANEL:
                            identifiers[(char) currentStateObject].Add(parseComponent(line, lineIndex));
                            break;
                        default:
                            ThrowError("State was none, but we didn't catch it (My fault sorry)", lineIndex);
                            break;
                    }
                }
            }

            return new LayoutData() {
                matrix = matrix,
                identifiers = identifiers,
                naviagation = navigation
            };
        }

        private static Component parseComponent(string line, int lineCounter) {
            int OpeningBracketIndex = line.IndexOf('(');
            int ClosingBracketIndex = line.LastIndexOf(')');
            int HashIndex = line.IndexOf('#');
            bool hasId = false;


            Component component;
            string uid = null;
            if(OpeningBracketIndex == -1) {
                if(ClosingBracketIndex != -1) {
                    ThrowError($"Found ')', but no '('", lineCounter);
                    return null;
                }

                hasId = HashIndex != -1;
                if(hasId)
                    uid = line.Substring(HashIndex + 1);

                //No parameters
                string componentname = ((hasId) ? line.Substring(0, HashIndex) : line).ToLower().Trim();
                switch(componentname) {
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
                        ThrowError("Unrecognized component", lineCounter);
                        return null;
                }
            } else {
                //With Parameters
                hasId = HashIndex != -1 && HashIndex < OpeningBracketIndex;
                if(hasId)
                    uid = line.Substring(HashIndex + 1, OpeningBracketIndex - HashIndex - 1);
                string componentname = line.ToLower().Trim().Substring(0, (hasId) ? HashIndex : OpeningBracketIndex/* - ((hasId)?HashIndex-1:0)*/);
                switch(componentname) {
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
                        component = new Spacer(getParameter(line, lineCounter));
                        break;
                    default:
                        ThrowError("Unrecognized component", lineCounter);
                        return null;
                }
            }
            if(hasId) {
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
                ThrowError("Missing Opening bracket", lineCount);
                return "";
            } else if(to == -1) {
                ThrowError("Missing Closing bracket", lineCount);
                return "";
            } else {
                string toReturn = line.Substring(from + 1, to - from - 1);
                if(toReturn.Length == 0) {
                    ThrowError("Empty parameter", lineCount);
                }
                return toReturn;
            }
        }


        //Stops execution and prompts to user
        private static void ThrowError(string message, int lineIndex) {
            FastConsole.SetCursor(0, 0);
            FastConsole.Write($"Error on line {lineIndex}: {message}", ConsoleColor.Red);
            FastConsole.Flush();
            Console.SetCursorPosition(0, 1);
            Environment.Exit(1);
        }
    }
}