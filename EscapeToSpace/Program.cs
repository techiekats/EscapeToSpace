using System.Collections.Generic;
using System.IO;
using System;

namespace EscapeToSpace
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> commands;
            SentenceParser parser = new SentenceParser();
            //read from file
            if (args.Length == 1)
            {
                if (!File.Exists(args[0]))
                {
                    Console.WriteLine(string.Format("file not found at path:{0}", args[0]));
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine($"Reading file contents of {args[0]}...");
                commands = GetInputCommandsFromFile(args[0]);
                foreach (string s in commands)
                {
                    var parsedSentence = parser.Parse(s);
                    if (parsedSentence.Type == Enums.SentenceTypes.Query)
                    {
                        int result = parser.Evaluate(parsedSentence);
                        string message = result == int.MinValue ? "Sorry, didnt get that" : result.ToString();
                        Console.WriteLine($"{ parsedSentence.Original} => {result.ToString()}");
                    }
                    if (parsedSentence.Type == Enums.SentenceTypes.Invalid)
                    {
                        string message = "Invalid. please check the query";
                        Console.WriteLine($"{ parsedSentence.Original} => {message}");
                    }
                }
            }
        }
        /// <summary>
        /// Parses file to yield a list of commands
        /// </summary>
        /// <param name="path">valid file path</param>
        /// <returns></returns>
        private static IEnumerable<string> GetInputCommandsFromFile(string path)
        {
            using (var fs = File.OpenText(path))
            {
                string line;
                while ((line = fs.ReadLine()) != null)
                {
                    yield return line;
                }
            }
            yield break;
        }
    }
}
