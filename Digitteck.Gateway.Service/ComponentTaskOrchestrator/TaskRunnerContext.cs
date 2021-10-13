using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Digitteck.Gateway.Service.ComponentTaskOrchestrator
{
    public class TaskRunnerContext
    {
        public List<TaskDescriptor> TaskDescriptors { get; }

        public TaskRunnerContext()
        {
            this.TaskDescriptors = new List<TaskDescriptor>();
        }

        public void Add(params TaskDescriptor[] taskDescriptor)
        {
            this.TaskDescriptors.AddRange(taskDescriptor);
        }

        internal void Validate()
        {
            CheckForDuplicateNames();
            CheckForCircularDependency();
            CheckForSelfWaiting();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckForDuplicateNames()
        {
            foreach (TaskDescriptor descriptor in TaskDescriptors)
            {
                if (TaskDescriptors.Count(x => x.Name == descriptor.Name) != 1)
                {
                    throw new Exception($"Multiple tasks with the same name {descriptor.Name} detected");
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckForSelfWaiting()
        {
            foreach (TaskDescriptor descriptor in TaskDescriptors)
            {
                if (descriptor.After.Contains(descriptor.Name))
                {
                    throw new Exception($"TaskDescriptor {descriptor.Name} waits for itself");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckForCircularDependency()
        {
            StringBuilder chain = new StringBuilder();
            const string chainLink = "->";

            foreach (TaskDescriptor descriptor in TaskDescriptors)
            {
                chain.Clear();
                chain.Append(descriptor.Name).Append(chainLink);

                foreach (var linked in Navigate(descriptor))
                {
                    chain.Append(linked.Name).Append(chainLink);

                    if (descriptor.Name == linked.Name)
                    {
                        if (chain.Length > chainLink.Length)
                        {
                            chain.Remove(chain.Length - chainLink.Length, chainLink.Length);
                        }

                        throw new Exception($"Circular dependency: {chain}");
                    }
                }
            }
        }

        private IEnumerable<TaskDescriptor> Navigate(TaskDescriptor taskDescriptor)
        {
            foreach (string taskNameBefore in taskDescriptor.After)
            {
                TaskDescriptor taskBefore = TaskDescriptors.Find(x => x.Name == taskNameBefore);

                yield return taskBefore;

                foreach (var subitem in Navigate(taskBefore))
                {
                    yield return subitem;
                }
            }
        }
    }
}
