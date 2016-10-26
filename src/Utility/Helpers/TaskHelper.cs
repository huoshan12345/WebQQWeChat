using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utility.Helpers
{
    public static class TaskHelper
    {
        public static Task<TResult[]> Repeat<TResult>(Func<TResult> action, int times)
        {
            var tasks = new Task<TResult>[times];
            for (var i = 0; i < times; i++)
            {
                tasks[i] = Task.Run(action);
            }
            return Task.WhenAll(tasks);
        }

        public static Task Repeat(Action action, int times)
        {
            var tasks = new Task[times];
            for (var i = 0; i < times; i++)
            {
                tasks[i] = Task.Run(action);
            }
            return Task.WhenAll(tasks);
        }

        public static Task<TResult[]> Repeat<TResult>(Func<Task<TResult>> action, int times)
        {
            var tasks = new Task<TResult>[times];
            for (var i = 0; i < times; i++)
            {
                tasks[i] = action();
            }
            return Task.WhenAll(tasks);
        }

        public static Task Repeat(Func<Task> action, int times)
        {
            var tasks = new Task[times];
            for (var i = 0; i < times; i++)
            {
                tasks[i] = action();
            }
            return Task.WhenAll(tasks);
        }
        
        public static async Task<TResult> Try<TResult>(Func<Task<TResult>> action, int times, 
            Action<AggregateException> handleException = null, TResult defaultResult = default(TResult))
        {
            var exs = new List<Exception>();
            for (var i = 0; i < times; ++i)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    exs.Add(ex);
                    await Task.Delay(1000 * (i + 1) * (i + 1));
                }
            }
            handleException?.Invoke(new AggregateException(exs));
            return defaultResult;
        }

        public static async Task Try(Func<Task> action, int times, Action<AggregateException> handleException = null)
        {
            var exs = new List<Exception>();
            for (var i = 0; i < times; ++i)
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    exs.Add(ex);
                    await Task.Delay(1000 * (i + 1) * (i + 1));
                }
            }
            handleException?.Invoke(new AggregateException(exs));
        }
    }
}
