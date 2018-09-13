using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Standard.Infrastructure.TaskQueues
{
    public class TaskItem<T>
    {
        [NonSerialized]
        public Func<T,Task> TaskAction;

        public string ActionName
        {
            get
            {
                return this.TaskAction.GetType().Name;
            }
        }

        public T Args
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public TaskItem()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
