using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AthenaCodingChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true) // Loop indefinitely
            {

                Console.WriteLine("Please enter a word:");
                string originalWord = Console.ReadLine();
                double totalMillisecondsAtStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
                char[] originalWordCharArray = originalWord.ToCharArray();
                char[] wordSortedCharArray = originalWord.ToCharArray();
                Array.Sort(wordSortedCharArray);
                int[] numberToFind = new int[originalWord.Length];
                int[] sortedNumberArray = new int[originalWord.Length];
                sortedNumberArray[0] = 1;

                // Begin: convert word to number (the entire purpose of this portion of the code is to convert the inputted word to it's corresponding numeric representation and store that number in numberToFind (since words are easier to work with than letters))
                for (int i = 1; i < wordSortedCharArray.Length; i++)
                {
                    if (wordSortedCharArray[i] != wordSortedCharArray[i - 1])
                    {
                        sortedNumberArray[i] = sortedNumberArray[i - 1] + 1;
                    }
                    else
                    {
                        sortedNumberArray[i] = sortedNumberArray[i - 1];
                    }
                }

                for (int i = 0; i < originalWordCharArray.Length; i++)
                {
                    for (int j = 0; j < wordSortedCharArray.Length; j++)
                    {
                        if (originalWordCharArray[i] == wordSortedCharArray[j])
                        {
                            numberToFind[i] = sortedNumberArray[j];
                            break;
                        }
                    }
                }
                // End: convert word to number


                //for (int i = 0; i < originalWordCharArray.Length; i++)
                //{
                //    Console.WriteLine(numberToFind[i]);
                //}
                //Console.ReadLine();

                double[] answerArray = new double[numberToFind.Length];
                int tempSmallestNum;
                LinkedList<double> answerList = new LinkedList<double>();
                double denominator;
                int prevSmallestNum;

                for (int i = 0; i < numberToFind.Length; i++)
                {
                    prevSmallestNum = 0;
                    while (true)
                    {
                        // find lowest # after # you're on
                        tempSmallestNum = numberToFind[i];
                        for (int j = i + 1; j < numberToFind.Length; j++)
                        {
                            if (numberToFind[j] < numberToFind[i] && numberToFind[j] < tempSmallestNum && numberToFind[j] > prevSmallestNum)
                            {
                                tempSmallestNum = numberToFind[j];
                            }
                        }

                        prevSmallestNum = tempSmallestNum;

                        if (tempSmallestNum >= numberToFind[i])
                        {
                            break;
                        }
                        else // swap letters and find numOccurences of each letter
                        {
                            // First, swap the letters
                            int[] tempNumberToFindArray = new int[numberToFind.Length];
                            Array.Copy(numberToFind, tempNumberToFindArray, numberToFind.Length);
                            //int tempStorage;
                            // find tempSmallestNum and do the swap
                            for (int j = i + 1; j < numberToFind.Length; j++)
                            {
                                if (numberToFind[j] == tempSmallestNum)
                                {
                                    //tempStorage = numberToFind[j];
                                    tempNumberToFindArray[j] = numberToFind[i];
                                    //tempNumberToFindArray[i] = tempStorage;

                                    break;
                                }
                            }

                            int[] duplicateFindingArray = new int[numberToFind.Length - 1 - i];
                            Array.Copy(tempNumberToFindArray, i + 1, duplicateFindingArray, 0, numberToFind.Length - 1 - i);

                            // Then find the Num Occurences of each letter
                            IEnumerable<KeyValuePair<int, int>> dupCounts = FindDuplicates(duplicateFindingArray);

                            denominator = 1;
                            foreach (KeyValuePair<int, int> pair in dupCounts)
                            {
                                denominator = denominator * Factorial(pair.Value);
                            }

                            answerList.AddLast(Factorial(numberToFind.Length - 1 - i) / denominator);

                        }
                    }
                }

                double finalAnswer = 0;

                foreach (var item in answerList)
                {
                    finalAnswer = finalAnswer + item;
                }

                finalAnswer = finalAnswer + 1;

                Console.WriteLine("Rank: " + finalAnswer);
                Console.WriteLine("Milliseconds to Execute: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - totalMillisecondsAtStart).ToString());
                Console.WriteLine();
            }

        }

        public static double Factorial(double num)
        {
            if (num == 1)
            {
                return 1;
            }
            return num * Factorial(num - 1);
        }

        private static IEnumerable<KeyValuePair<int, int>> FindDuplicates(int[] array)
        {
            Dictionary<int, int> intSet = new Dictionary<int, int>();
            foreach (var item in array)
            {
                int count;
                if (intSet.TryGetValue(item, out count))
                {
                    intSet[item] = count + 1;
                }
                else
                {
                    intSet[item] = 1;
                }
            }
            return intSet.Where(p => p.Value > 1);
        }
    }
}
