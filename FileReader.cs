using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace tui_generator
{
    public struct ReadReturn{
        public ArrayList matrix;
        public Dictionary<char,ArrayList> identifiers;
    }

    public class FileReader
    {
        public static ReadReturn Read(string path){
            try {
            using (var sr = new StreamReader(path)) {
                bool inLayout = false;
                int layoutWidth = -1;
                char currentIdentifier = '\0';
                ArrayList matrix = new ArrayList();
                Dictionary<char,ArrayList> identifiers = new Dictionary<char, ArrayList>();

                int lineCounter = 0;
                while (sr.Peek() >= 0)
                {
                    lineCounter++;
                    string line = sr.ReadLine().Trim().ToLower();
                    if(String.IsNullOrWhiteSpace(line)){
                        inLayout = false;
                        currentIdentifier = '\0';
                        continue;
                    }

                    if(line.StartsWith(":layout")){
                        inLayout = true;
                        continue;
                    }else if(line.StartsWith(":")){
                        inLayout = false;
                        try
                        {
                            char selector = Char.Parse(line.Substring(1));
                            if(!identifiers.ContainsKey(selector)){
                                ThrowError($"Identifier '{selector}' was not declared in layout (line {lineCounter})");
                            }
                            currentIdentifier = selector;
                        }
                        catch (Exception)
                        {
                            ThrowError($"Identifier couldn't be parsed into a character (line {lineCounter})");
                        }
                        
                    }else{
                        if(inLayout){
                            string[] sections = line.Split(' ');
                            char[] _sections = new char[sections.Length];
                            for (int i = 0; i < sections.Length; i++)
                            {
                                try{
                                    _sections[i] = Char.Parse(sections[i]);
                                    if(_sections[i] == ':'){
                                        ThrowError($"Illegal layout identifier. Identifier cannot be ':' (line {lineCounter})");
                                    }
                                    if(!identifiers.ContainsKey(_sections[i])){
                                        identifiers.Add(_sections[i], new ArrayList());
                                    }
                                }catch(Exception){
                                    ThrowError($"Error parsing layout: '{sections[i]}' is not a character (line {lineCounter})");
                                }
                            }
                            if(layoutWidth == -1){
                                layoutWidth = _sections.Length;
                            }else if(layoutWidth != _sections.Length){
                                ThrowError($"Layout length is not consistent troughouth lines (line {lineCounter})");
                            }
                            matrix.Add(_sections);
                        }else if(currentIdentifier != '\0'){
                            identifiers[currentIdentifier].Add(line);
                        }else{
                            ThrowError($"Invalid line '{line}' (line {lineCounter})");
                        }
                    }
                    
                    Console.WriteLine(line);
                }

                //TODO check if all regions aren't cut off
                // 1 0 0
                // 0 1 0  Like this
                // 0 1 0

                Console.WriteLine("Elements of matrix:");
                foreach (char[] item in matrix)
                {
                    foreach (char item_ in item)
                    {
                        Console.WriteLine(item_);
                    }
                }

                Console.WriteLine("Identifiers:");
                foreach (KeyValuePair<char,ArrayList> item in identifiers)
                {
                    Console.WriteLine($"{item.Key} --> {item.Value}");
                    foreach (string obj in item.Value)
                    {
                        Console.WriteLine(" " + obj);
                    }
                }

                return new ReadReturn(){
                    matrix = matrix,
                    identifiers = identifiers
                };
            }
            } catch (IOException e) {
                ThrowError($"Error reading file: {e.Message}");
                return new ReadReturn();
            }
        }

        public static void ThrowError(string message){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("An error occured: ");
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(1);
        }
    }
}