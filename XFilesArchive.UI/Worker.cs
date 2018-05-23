using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XFilesArchive.UI
{
    class Worker
    {
        public event EventHandler<WorkerEventArgs> ProgressChanged;
        
        protected virtual void OnProgressChanged(int Progress)
        {
            if (ProgressChanged != null) ProgressChanged(this, new WorkerEventArgs() { Progress = Progress });
        }
        public Worker()
        {

        }

        public Task<int> Work(IProgress<int> progress, CancellationToken token,Func<int> func)
        {

            return Task<int>.Run(() =>
            {
                ((IProgress<int>)progress).Report(50);
                if (token.IsCancellationRequested)
                {
                    return 0;
                }
                int re = func();
                    ((IProgress<int>)progress).Report(100);
                    if (token.IsCancellationRequested)
                    {
                        return 0;
                    }
             
                return re;
            });

        }


    }
    class WorkerEventArgs : EventArgs { public int Progress { get; set; } }
}
