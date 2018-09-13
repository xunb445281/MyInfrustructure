using Standard.Infrastructure.Caches;
using Standard.Infrastructure.Helpers;
using Standard.Infrastructure.Logs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Standard.Infrastructure.TaskQueues
{
    public class TaskQueue<T>
    {
        private readonly static TaskQueue<T> obj = new TaskQueue<T>();

        private bool workThreadState;

        private Thread workThread;

        private ConcurrentQueue<TaskItem<T>> pool = new ConcurrentQueue<TaskItem<T>>();

        public static TaskQueue<T> Current
        {
            get
            {
                return obj;
            }
        }

        private TaskQueue()
        {
            this.CreateWorkThread();
        }

        private void CreateWorkThread()
        {
            if (!this.workThreadState)
            {
                this.workThreadState = true;
                this.workThread = new Thread(new ThreadStart(WorkTask))
                {
                    IsBackground = true
                };
                this.workThread.Start();
            }
        }

        public void Enqueue(TaskItem<T> item)
        {
            this.pool.Enqueue(item);
        }

        public void Enqueue(Func<T,Task> action, T args)
        {
            TaskItem<T> taskItem = new TaskItem<T>()
            {
                TaskAction = action,
                Args = args
            };
            this.pool.Enqueue(taskItem);
        }

        private async void StartTaskAsync(TaskItem<T> item)
        {
            if (item == null)
                return;

            Cache.GetCache().Add<string>(string.Format("takeaway-platform-{0}", item.Id), JsonHelper.ToJson(item));
            await item.TaskAction(item.Args);
            Cache.GetCache().Remove(string.Format("takeaway-platform-{0}", item.Id));
        }

        public void  WorkTask()
        {
            TaskItem<T> taskItem;
            while (this.workThreadState)
            {
                if (this.pool.Count > 0)
                {
                    if (this.pool.TryDequeue(out taskItem))
                    {
                        try
                        {
                            StartTaskAsync(taskItem);
                        }
                        catch (Exception ex)
                        {
                            Log.GetLog().Error(string.Format("任务失败，Id：{0},原因:{1}", taskItem.Id, ex.Message));
                        }
                    }
                }
                Thread.Sleep(10);
            }
            this.workThreadState = false;
        }
    }
}
