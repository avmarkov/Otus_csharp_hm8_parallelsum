### Домашняя работа № 8. Многопоточный проект
1. Напишите вычисление суммы элементов массива интов:
* Обычное
* Параллельное (для реализации использовать Thread, например List)
* Параллельное с помощью LINQ
2. Замерьте время выполнения для 100 000, 1 000 000 и 10 000 000
3. Укажите в таблице результаты замеров, указав:
* Окружение (характеристики компьютера и ОС)
* Время выполнения последовательного вычисления
* Время выполнения параллельного вычисления
* Время выполнения LINQ

### 1. Напишите вычисление суммы элементов массива интов:
Я сделал класс public class MySum, в котором реализовал разные варианты суммирования элементов массива

### Обычное вычисление суммы элементов массива интов

```cs
public (long, TimeSpan) GetSequentialSum()
{
    var stopWatch = new Stopwatch();
    stopWatch.Start();

    long sum = 0;
    foreach (var item in array)
    {
        sum += item;
    }
    stopWatch.Stop();

    return (sum, stopWatch.Elapsed);
}
```

### Параллельное (для реализации использовать Thread, например List)
Для реализации параллельного вычисления я использовал не напрямую потоки, а Task. 
Я запускаю вычисление суммы массива в отдельной Task. Количество Task - это входной параметр taskCount.
```cs
public (long, TimeSpan) GetWithTaskSum(int taskCount) // taskCount - количество тасок, с помощью которых будет вычисляться сумма
{
    var stopWatch = new Stopwatch();
    stopWatch.Start();
    long sum = 0;
    var taskList = new List<Task<long>>();

    // длина подмассива, для которого будет вычиляться сумма в таске
    int subArrLen = array.Length / taskCount;
    for (int i = 0; i < taskCount; i++)
    {
        int lastInd;

        if (i == taskCount - 1)
        {
            lastInd = array.Length;
        }
        else
        {
            lastInd = (i + 1) * subArrLen;
        }

        // добавляем и задачу в taskList, она будет запущена внутри CalcSumInTaskAsync 
        taskList.Add(CalcSumInTaskAsync(i * subArrLen, lastInd));
    }

    try
    {
        // ожидание выполнения всех тасок из taskList
        Task.WaitAll(taskList.ToArray());                
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
            
    foreach (var item in taskList)
    {
        sum = sum + item.Result;
    }
    stopWatch.Stop();
    return (sum, stopWatch.Elapsed);
}

public async Task<long> CalcSumInTaskAsync(int firstInd, int lastInd)
{
    // запускаем Task, в которой вычисляется сумма
    var t = Task.Run(() =>
    {
        long sum = 0;

        for (int i = firstInd; i <= lastInd - 1; i++)
        {
            sum = sum + array[i];
        }

        return sum;
    });

    return await t;
}
```

### Параллельное с помощью LINQ
```cs
public (long, TimeSpan) GetParallelLinqSum()
{
    var stopWatch = new Stopwatch();
    stopWatch.Start();
    long sum = 0;
    sum = array.AsParallel().Sum();

    stopWatch.Stop();

    return (sum, stopWatch.Elapsed);
}
```

### 2. Замерьте время выполнения для 100 000, 1 000 000 и 10 000 000
<image src="images/result.png" alt="result">