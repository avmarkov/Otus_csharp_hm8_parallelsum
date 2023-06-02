using System.Diagnostics;

namespace hm8_parallelsum
{
    public class MySum
    {
        private int[] array;
        public MySum(int[] array)
        {
            this.array = array;
        }

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

        public (long, TimeSpan) GetParallelLinqSum()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            long sum = 0;
            sum = array.AsParallel().Sum();

            stopWatch.Stop();

            return (sum, stopWatch.Elapsed);
        }

        public (long, TimeSpan) GetWithTaskSum(int taskCount) // taskCount - количество тасок, с помощью которых будет вычисляться сумма
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            long sum = 0;
            var taskList = new List<Task<long>>();

            // длина массива, для которого будет вычиляться сумма в таске
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

                taskList.Add(CalcSumInTaskAsync(i * subArrLen, lastInd));
            }

            try
            {
                Task.WaitAll(taskList.ToArray());
                //var res = await t;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //var res = await t;
            foreach (var item in taskList)
            {
                sum = sum + item.Result;
            }
            stopWatch.Stop();
            return (sum, stopWatch.Elapsed);
        }

        public async Task<long> CalcSumInTaskAsync(int firstInd, int lastInd)
        {
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
    }
}
