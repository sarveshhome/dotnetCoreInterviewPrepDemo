using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnetCoreInterviewPrepDemo.Question.IntermediateQ
{
    public class LargestElement12
    {
        public static int FindLargestElement()
        {
            int[] numbers = { 12, 45, 7, 23, 99, 56, 3, 81 };
            
            #region method 1
            //return numbers.Max();
            #endregion

            #region method 2
            int largest = numbers[0]; 
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] > largest) 
                {
                    largest = numbers[i];
                }
            }
            return largest;
            #endregion

            #region method 3

            // int number = numbers.OrderByDescending(n => n).FirstOrDefault();

            // return number;

            #endregion
        }
    }
}
