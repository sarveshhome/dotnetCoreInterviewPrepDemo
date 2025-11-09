namespace dotnetCoreInterviewPrepDemo.Question.IntermediateQ;

public class ExpecptionDivideByZeroException
{
    public static void ExceptionDiv()
    {
        try
        {
            Console.WriteLine("Hello");
            //compile time error
            //var i = 2 / 0;      //error CS0020: Division by constant zero
            
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("A");
        }
        catch (Exception)
        {
            Console.WriteLine("B");
        }
        finally
        {
            Console.WriteLine("C");
        }
        Console.ReadKey();
    }
}
