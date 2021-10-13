using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service.ComponentTaskOrchestrator
{
    public class TaskOrchestrator
    {
        private static readonly object mlock = new object();
        private readonly List<string> executedTasks;
        private int lastExecutionCount = -1;

        private TaskRunnerContext Context { get; }

        internal TaskOrchestrator()
        {
            executedTasks = new List<string>();
            Context = new TaskRunnerContext();
        }

        public static TaskOrchestrator New()
        {
            return new TaskOrchestrator();
        }

        public TaskDescriptor GetTaskDescriptorByName(string name)
        {
            return this.Context.TaskDescriptors.Find(x => x.Name == name);
        }

        public void Add(params TaskDescriptor[] taskDescriptors)
        {
            Context.Add(taskDescriptors);
        }

        public void Run()
        {
            Context.Validate();

            Run(Context.TaskDescriptors.ToArray());
        }

        private void Run(TaskDescriptor[] leftOvers)
        {
            List<TaskDescriptor> canRun = new List<TaskDescriptor>();

            lock (mlock)
            {
                //check if we are running the same tasks
                if (lastExecutionCount > -1 && leftOvers.Length == lastExecutionCount)
                {
                    throw new Exception($"The task runner is stuck executing the same tasks");
                }

                lastExecutionCount = leftOvers.Length;

                canRun = new List<TaskDescriptor>();

                foreach (var t in leftOvers)
                {
                    if (CanRun(t))
                    {
                        canRun.Add(t);
                    }
                }
            }

            RunThese(canRun.ToArray());

            lock (mlock)
            {
                executedTasks.AddRange(canRun.Select(x => x.Name));

                List<TaskDescriptor> nextLeftOvers = new List<TaskDescriptor>();

                foreach (var t in leftOvers)
                {
                    if (!canRun.Any(x => x.Name == t.Name))
                    {
                        nextLeftOvers.Add(t);
                    }
                }

                if (nextLeftOvers.Count > 0)
                {
                    Run(nextLeftOvers.ToArray());
                }
            }

        }

        private bool CanRun(TaskDescriptor t)
        {
            if (executedTasks.Any(x => x == t.Name))
            {
                return false;
            }

            foreach (var wait in t.After)
            {
                if (!executedTasks.Contains(wait))
                {
                    return false;
                }
            }

            return true;
        }

        private void RunThese(TaskDescriptor[] taskDescriptors)
        {
            List<Task> tasks = new List<Task>();

            foreach (var t in taskDescriptors)
            {
                tasks.Add(new Task(t.TaskAction));
            }

            foreach (var t in tasks)
            {
                t.Start();
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
