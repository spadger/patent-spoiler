using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PatentSpoiler.App.Initialisation
{
    public interface IInitialisationManager
    {
        bool IsInitialised { get; }
        void RegisterTask(string taskName);
        void TaskIsComplete(string taskName);
        IEnumerable<string> PendingTasks { get; } 
        IEnumerable<string> CompletedTasks { get; } 
    }

    public class InitialisationManager : IInitialisationManager
    {
        private long inProgressCount;
        private IDictionary<string, bool> workItems = new Dictionary<string, bool>();

        public bool IsInitialised
        {
            get
            {
                return inProgressCount == 0;
            }
        }

        public void RegisterTask(string taskName)
        {
            workItems.Add(taskName, false);
            Interlocked.Increment(ref inProgressCount);
        }

        public void TaskIsComplete(string taskName)
        {
            workItems[taskName] = true;
            Interlocked.Decrement(ref inProgressCount);
        }

        public IEnumerable<string> PendingTasks 
        {
            get { return workItems.Where(x => x.Value == false).Select(x => x.Key); }
        }

        public IEnumerable<string> CompletedTasks
        {
            get { return workItems.Where(x => x.Value).Select(x => x.Key); }
        }
    }
}