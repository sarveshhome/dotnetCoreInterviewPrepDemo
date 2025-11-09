namespace dotnetCoreInterviewPrepDemo.Question.IntermediateQ;
// interview question 31 oct 2025
//move all zeros to the end of the array
public class order_of_non_zero_elements
{
    public static void ordernonzero()
    {
        int[] arr = {1, 0, 3, 0, 4, 5};
        int nonZeroIndex = 0; // pointer for next non-zero placement

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != 0)
            {
                // swap arr[i] with arr[nonZeroIndex]
                int temp = arr[nonZeroIndex];
                arr[nonZeroIndex] = arr[i];
                arr[i] = temp;
                nonZeroIndex++;
            }
        }

        Console.WriteLine(string.Join(",", arr));
    }
}