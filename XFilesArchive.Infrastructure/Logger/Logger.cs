using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Infrastructure
{
    public class Logger : ILogger
    {
        System.Collections.Specialized.StringCollection _log ;
        public Logger()
        {
            _log = new System.Collections.Specialized.StringCollection();
        }
        public void Add(string message)
        {
            _log.Add(message);
        }
        public string GetLog() {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _log)
            {
                sb.Append(item);
            }
            return sb.ToString();
        }
    }
}
