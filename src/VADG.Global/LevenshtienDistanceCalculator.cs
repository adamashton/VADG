using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VADG.Global
{
    public static class LevenshteinDistanceCalculator
    {
        private static int EditDistance(string s, string t)
        {
            var theTable = new int[s.Length + 1, t.Length + 1];


            for (int i = 0; i <= s.Length; i++)
                theTable[i, 0] = i;
            
            for (int j = 0; j <= t.Length; j++)
                theTable[0, j] = j;

            for (int j = 1; j <= t.Length; j++)
            {
                //for (int u = 0; u <= s.Length; u++)
                //{
                //    for (int v = 0; v <= t.Length; v++)
                //    {
                //        Console.Write(theTable[u, v] + " ");
                //    }
                //    Console.WriteLine();
                //}
                //Console.WriteLine();



                for (int i = 1; i <= s.Length; i++)
                {
                    if (s[i - 1] == t[j - 1])
                        theTable[i, j] = theTable[i - 1, j - 1]; // bring cost forward if match
                    else
                    {
                        int deletionCost = theTable[i - 1, j] + 1;
                        int insertionCost = theTable[i, j - 1] + 1;
                        int substitutionCost = theTable[i - 1, j - 1] + 1;

                        theTable[i, j] = Math.Min(Math.Min(deletionCost, insertionCost), substitutionCost);
                    }

                }
            }

            return theTable[s.Length, t.Length];
        }

        /// <summary>
        /// Provides a suggestion of a word that the target may be close to from the dictionary.
        /// Only works for words greater than or equal to 4 characters
        /// Returns null if the target equals any word in the dictionary
        /// Returns null if the target is too far away from a word in the dictionary
        /// </summary>
        /// <param name="target">User typed word</param>
        /// <param name="dictionary">Dictionary of words we are checking against</param>
        /// <returns></returns>
        public static String suggest(string target, List<string> dictionary)
        {
            if (target.Length < 4)
                return null;
            
            int min = Int32.MaxValue;
            String suggestion = null;

            foreach (String word in dictionary)
            {
                if (target.Equals(word))
                    return null;

                int distance = EditDistance(target, word);
                if (distance < min)
                {
                    min = distance;
                    suggestion = word;
                }
            }
            
            int distanceThreshold = -1;
            if (target.Length <= 8)
                distanceThreshold = (target.Length / 2) - 1;
            else
                distanceThreshold = 4;

            if (min <= distanceThreshold)
                return suggestion;
            else
                return null;
        }
    }
}
