using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace SpecNextTiler.ViewModel
{
    public class LogViewModel : ViewModelBase
    {
        private string _logText = string.Empty;
        public string LogText
        {
            get => _logText;
            set => SetProperty(ref _logText, value);
        }

        public void AddLogEntry(string entry)
        {
            var log = new StringBuilder(LogText);
            log.AppendLine(entry);
            LogText = log.ToString();
        }
    }
}
