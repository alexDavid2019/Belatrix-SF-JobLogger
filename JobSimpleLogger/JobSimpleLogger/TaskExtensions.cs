using System;
using System.Threading;
using System.Threading.Tasks;


namespace JobSimpleLogger
{
    internal static class TaskExtensions
    {
        public static Task<TResult> Then<TResult>(this Task task, Func<Task, TResult> continuationFunction, CancellationToken token)
        {
            return task
                .ContinueWith(t =>
                {
                    var tcs = new TaskCompletionSource<TResult>();

                    if (t.IsCanceled)
                        tcs.SetCanceled();
                    else if (t.Exception != null) // t.IsFaulted is true
                        tcs.SetException(t.Exception.GetBaseException());
                    else
                        tcs.SetResult(continuationFunction(t));

                    return tcs.Task;
                }, token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Current)
                .Unwrap();
        }

        public static Task CanceledTask(this TaskCompletionSource<object> tcs)
        {
            tcs.SetCanceled();
            return tcs.Task;
        }

        public static Task SucceededTask(this TaskCompletionSource<object> tcs)
        {
            tcs.SetResult(null);
            return tcs.Task;
        }

        public static Task FailedTask(this TaskCompletionSource<object> tcs, Exception exception)
        {
            tcs.SetException(exception);
            return tcs.Task;
        }
    }
}
