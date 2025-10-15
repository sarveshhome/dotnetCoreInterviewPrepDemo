//Check if a string is a palindrome.
using System;

namespace dotnetCoreInterviewPrepDemo.Question.IntermediateQ
{
    public class StringPalindrome
    {
        public static void CheckPalindrome()
        {
        
            // madam, racecar, level, radar, refer
            string input = "madam";
            string reversed = "";
            for (int i = input.Length - 1; i >= 0; i--)
            {
                reversed += input[i];
            }
            if (input == reversed)
            {
                Console.WriteLine("The string is a palindrome.");
            }
            else
            {
                Console.WriteLine("The string is not a palindrome.");
            }
        }
    }
}
