using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogCenterNameSpace
{
    public class LogCenter
    {
        public ObservableCollection<LogRecord> records;

        public LogCenter()
        {
            records = new ObservableCollection<LogRecord>();
            records.Add(new LogRecord("Журнал создан"));
        } 
        /// <summary>
        ///add message to log
        /// </summary>
        /// <param name="MSg"></param>
        public void AddMessage(string MSg)
        {
            records.Add(new LogRecord(MSg));
        }
    }
    
    /// <summary>
    /// базовая запись лога
    /// </summary>
    public class LogRecord
    {
        public DateTime Time { get; }
        public string Message { get; }

        public LogRecord() { }

        public LogRecord(string Msg)
        {
            this.Time = DateTime.Now;
            this.Message = Msg;
        }
        
    }
}
