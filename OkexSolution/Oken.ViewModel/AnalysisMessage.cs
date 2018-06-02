using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oken.ViewModel
{
    public class AnalysisMessage
    {
        public AnalysisMessage(string name, decimal number, DateTime updateTime, string biName)
        {
            Name = name;
            Number = number.ToString("0.000");
            UpdateTime = updateTime.ToString("HH:mm:ss fff");
            BiName = biName;
        }

        public string Name { get; set; }

        public string Number { get; set; }

        public string UpdateTime { get; set; }

        public string BiName { get; set; }

    }
}
