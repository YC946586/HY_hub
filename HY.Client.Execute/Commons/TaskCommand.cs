using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons
{
    /// <summary>
    /// 线程通用类
    /// </summary>
    public class TaskCommand
    {
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        ManualResetEvent resetEvent = new ManualResetEvent(true);
        Thread thread = null;
        /// <summary>
        /// 开始任务
        /// </summary>
        public void StartData()
        {
            tokenSource = new CancellationTokenSource();
            resetEvent = new ManualResetEvent(true);

            List<int> Ids = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                Ids.Add(i);
            }
            thread = new Thread(new ThreadStart(() => StartTask(Ids)));
            thread.Start();
        }
        /// <summary>
        /// 暂停任务
        /// </summary>
        public void OutData()
        {
            //task暂停
            resetEvent.Reset();
        }
        /// <summary>
        /// 继续任务
        /// </summary>
        public void ContinueData()
        {
            //task继续
            resetEvent.Set();
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        public void Cancel()
        {
            //释放对象
            resetEvent.Dispose();
            foreach (var CurrentTask in ParallelTasks)
            {
                if (CurrentTask != null)
                {
                    if (CurrentTask.Status == TaskStatus.Running) { }
                    {
                        //终止task线程
                        tokenSource.Cancel();
                    }
                }
            }
            thread.Abort();
        }
        /// <summary>
        /// 执行数据
        /// </summary>
        /// <param name="Index"></param>
        public void Execute(int Index)
        {
            //阻止当前线程
            resetEvent.WaitOne();

            Console.WriteLine("当前第" + Index + "个线程");

            Thread.Sleep(1000);

        }
        //队列对象
        private Queue<MeterAsyncQueue> AsyncQueues { get; set; }

        /// <summary>
        /// 并发任务数
        /// </summary>
        private int ParallelTaskCount { get; set; }


        /// <summary>
        /// 并行任务集合
        /// </summary>
        private List<Task> ParallelTasks { get; set; }
        //控制线程并行数量
        public void StartTask(List<int> Ids)
        {
            IsInitTask = true;
            ParallelTasks = new List<Task>();
            AsyncQueues = new Queue<MeterAsyncQueue>();
            //获取并发数
            ParallelTaskCount = 5;

            //初始化异步队列
            InitAsyncQueue(Ids);
            //开始执行队列任务
            HandlingTask();

            Task.WaitAll(new Task[] { Task.WhenAll(ParallelTasks.ToArray()) });
        }
        /// <summary>
        /// 初始化异步队列
        /// </summary>
        private void InitAsyncQueue(List<int> Ids)
        {
            foreach (var item in Ids)
            {
                MeterInfo info = new MeterInfo();
                info.Id = item;
                AsyncQueues.Enqueue(new MeterAsyncQueue()
                {
                    MeterInfoTask = info
                });
            }
        }
        /// <summary>
        /// 是否首次执行任务
        /// </summary>
        private bool IsInitTask { get; set; }
        //锁
        private readonly object _objLock = new object();

        /// <summary>
        /// 开始执行队列任务
        /// </summary>
        private void HandlingTask()
        {
            lock (_objLock)
            {
                if (AsyncQueues.Count <= 0)
                {
                    return;
                }

                var loopCount = GetAvailableTaskCount();
                //并发处理队列
                for (int i = 0; i < loopCount; i++)
                {
                    HandlingQueue();
                }
                IsInitTask = false;
            }
        }
        /// <summary>
        /// 获取队列锁
        /// </summary>
        private readonly object _queueLock = new object();

        /// <summary>
        /// 处理队列
        /// </summary>
        private void HandlingQueue()
        {
            CancellationToken token = tokenSource.Token;
            lock (_queueLock)
            {
                if (AsyncQueues.Count > 0)
                {
                    var asyncQueue = AsyncQueues.Dequeue();

                    if (asyncQueue == null) return;
                    var task = Task.Factory.StartNew(() =>
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }
                        //阻止当前线程
                        resetEvent.WaitOne();
                        //执行任务
                        Execute(asyncQueue.MeterInfoTask.Id);

                    }, token).ContinueWith(t =>
                    {
                        HandlingTask();
                    }, TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
                    ParallelTasks.Add(task);
                }
            }
        }
        /// <summary>
        /// 获取当前有效并行的任务数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private int GetAvailableTaskCount()
        {
            if (IsInitTask)
                return ParallelTaskCount;
            return 1;
        }
    }

     /// 并发对象
      /// </summary>
   public class MeterAsyncQueue
     {
       public MeterAsyncQueue()
         {
           MeterInfoTask = new MeterInfo();
        }
 
        public MeterInfo MeterInfoTask { get; set; }
     }
    public class MeterInfo
    {
        public MeterInfo()
         {
 
        }
         public int Id { get; set; }

     }
}
