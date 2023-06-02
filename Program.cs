
using hm8_parallelsum;


SumCalc(100_000);
SumCalc(1_000_000);
SumCalc(10_000_000);

static int[] GetRandomArray(int arrLenght)
{
    int[] arr = new int[arrLenght];
    Random rnd = new Random();

    for (int i = 0; i < arr.Length; i++)
    {
        arr[i] = rnd.Next(-1000, 1000);
    }

    return arr;
}

static async void SumCalc(int arrLenght)
{
    int[] arr = GetRandomArray(arrLenght);
    MySum mySum = new MySum(arr);


    Console.WriteLine("Последовательное суммирование " + arrLenght + " элементов:");
    var ressum = mySum.GetSequentialSum();
    Console.WriteLine("Сумма: {0}, время: {1}", ressum.Item1, ressum.Item2);
    Console.WriteLine();

    Console.WriteLine("Параллельное суммирование с помощью LINQ " + arrLenght + " элементов:");
    ressum = mySum.GetParallelLinqSum();
    Console.WriteLine("Сумма: {0}, время: {1}", ressum.Item1, ressum.Item2);
    Console.WriteLine();

    Console.WriteLine("Параллельное суммирование с помощью Task " + arrLenght + " элементов:");
    ressum = mySum.GetWithTaskSum(11);
    Console.WriteLine("Сумма: {0}, время: {1}", ressum.Item1, ressum.Item2);

    Console.WriteLine();
    Console.WriteLine("----------------------------------------------------------------------");
    Console.WriteLine();
}

