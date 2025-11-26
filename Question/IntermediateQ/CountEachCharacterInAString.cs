public class Count_Each_Character_in_a_String
{
    public static void CountEachCharacterInAStrings()
    {
        string str = "Hello World";
        Dictionary<char, int> charCount = new Dictionary<char, int>();

        foreach (char c in str)
        {
            if (charCount.ContainsKey(c))
                charCount[c]++;
            else
                charCount[c] = 1;
        }

        // Print output
        foreach (var item in charCount)
        {
            Console.WriteLine($"{item.Key} : {item.Value}");
        }

        System.Console.WriteLine(string.Join(", ", charCount.Select(x => $"{x.Key} : {x.Value}")));

        str.GroupBy(c => c).ToList().ForEach(g => Console.WriteLine($"{g.Key} : {g.Count()}"));
    }
}
