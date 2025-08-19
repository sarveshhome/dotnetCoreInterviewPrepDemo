using System.Threading.Tasks;

public class ParallelTestService()
{
    private readonly int N = 100;
    public Task ParallelTestMethod(){

        Parallel.For(0,N, Method2);

        // Parallel.For(0,N, delegate(int i)
        // {
        //     Console.WriteLine(i);
        // });

        

        //Parallel.For(0, N, i => Console.WriteLine(i));

        // wait task for 1 min
        // Parallel.For(0, N, i => {
        //     Thread.Sleep(7000);
        // });

        return Task.CompletedTask;
    }

    static void Method2(int i){
        Console.WriteLine(i);
    }

}