﻿using System;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using TestStack.BDDfy.Processors;

namespace TestStack.BDDfy.Scanners.StepScanners
{
    public class StepActionFactory
    {
        public static Action<object> GetStepAction(MethodInfo method, object[] inputs)
        {
            return o => Run(()=>method.Invoke(o, inputs));
        }

        public static Action<object> GetStepAction<TScenario>(Action<TScenario> action)
            where TScenario : class
        {
            return o => Run(() =>
            {
                action((TScenario) o);
                return null;
            });
        }

        private static void Run(Func<object> func)
        {
            var oldSyncContext = SynchronizationContext.Current;
            try
            {
                var asyncSyncContext = new AsyncTestSyncContext();
                SetSynchronizationContext(asyncSyncContext);
                var result = func();
                var task = result as Task;
                if (task != null)
                {
                    try
                    {
                        task.Wait();
                    }
                    catch (AggregateException ae)
                    {
                        var innerException = ae.InnerException;
                        ExceptionProcessor.PreserveStackTrace(innerException);
                        throw innerException;
                    }
                }
                else
                {
                    var ex = asyncSyncContext.WaitForCompletion();
                    if (ex != null)
                    {
                        ExceptionProcessor.PreserveStackTrace(ex);
                        throw ex;
                    }
                }
            }
            finally
            {
                SetSynchronizationContext(oldSyncContext);
            }
        }

        [SecuritySafeCritical]
        static void SetSynchronizationContext(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
        }
    }
}