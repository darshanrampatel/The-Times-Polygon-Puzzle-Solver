using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using edu.stanford.nlp.tagger.maxent;

namespace Polygon_Puzzle_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            var allLetters = "APISTNC"; // excluding central letter
            var centralLetter = "Y";           
            allLetters += centralLetter; // add back central letter
            var minLetters = 4;
            var maxLetters = allLetters.Length + centralLetter.Length;
            var words = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "3of6game.txt")); // from http://wordlist.aspell.net/12dicts/
            var results = words
                .Select(w => w.ToUpperInvariant())
                // TODO excluding capitalised words
                // TODO conjugated words                
                // TODO comparatives and superlatives                
                .Where(w => !w.EndsWith("LY")) // exclude adverbs ending in LY
                .Where(w => w.Contains(centralLetter)) // must contain central letter
                .Where(w => w.Length <= maxLetters) // must be within required length
                .Where(w => w.Length >= minLetters) // must be within required length
                .Where(w =>
                {
                    var list = allLetters.ToList();
                    return w.All(ch => list.Remove(ch));
                }) // only contains allowed letters                 
                .OrderBy(w => w);
            var foundWords = new List<string>();
            var tagger = new MaxentTagger(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "english-left3words-distsim.tagger"));
            Console.Clear();
            Console.WriteLine("The Times' Polygon Puzzle Solver");
            foreach (var word in results)
            {
                var taggedString = tagger.tagString(word);
                if (!taggedString.Trim().Equals($"{word}_NNS")) // remove plurals
                {
                    Console.WriteLine(word);
                    foundWords.Add(word);
                }
            }
            Console.WriteLine($"Total words: {foundWords.Count}");

            Console.ReadLine();
        }
    }
}
