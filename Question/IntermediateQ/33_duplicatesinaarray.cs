
using  System;

namespace dotnetCoreInterviewPrepDemo.Question.IntermediateQ
{
    public class DuplicatesInArray33
    {
        public static void FindDuplicates()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 3, 2, 6, 7, 8, 1 };

            #region Method 1: Using HashSet
            var seen = new HashSet<int>();
            var duplicates = new HashSet<int>();

            foreach (var number in numbers)
            {
                if (!seen.Add(number))
                {
                    duplicates.Add(number);
                }
            }

            Console.WriteLine("Duplicates using HashSet: " + string.Join(", ", duplicates));
            #endregion

            #region Method 2: Using LINQ
            var duplicateNumbers = numbers.GroupBy(n => n)
                                          .Where(g => g.Count() > 1)
                                          .Select(g => g.Key);

            Console.WriteLine("Duplicates using LINQ: " + string.Join(", ", duplicateNumbers));
            #endregion

            #region Method 3: Using Sorting
            Array.Sort(numbers);
            var sortedDuplicates = new List<int>();

            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] == numbers[i - 1] && (sortedDuplicates.Count == 0 || sortedDuplicates.Last() != numbers[i]))
                {
                    sortedDuplicates.Add(numbers[i]);
                }
            }

            Console.WriteLine("Duplicates using Sorting: " + string.Join(", ", sortedDuplicates));
            #endregion
        }
    }
}