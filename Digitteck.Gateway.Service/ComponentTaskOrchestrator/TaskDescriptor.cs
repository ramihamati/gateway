using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service.ComponentTaskOrchestrator
{
    public class TaskDescriptor
    {
        public string Name;
        public string[] After;
        public Action TaskAction;

        public TaskDescriptor(string name, Action provider)
        {
            this.Name = name;
            this.TaskAction = provider;
            this.After = new string[] { };
        }

        public void WaitFor(params TaskDescriptor[] names)
        {
            this.After = names.Select(x => x.Name).ToArray();
        }
    }
}
