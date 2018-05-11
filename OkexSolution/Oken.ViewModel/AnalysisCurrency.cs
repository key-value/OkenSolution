using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oken.ViewModel
{
    public class AnalysisCurrency
    {
        public string Name { get; set; }

        /// <summary>
        /// 分析数值
        /// </summary>
        public decimal AnalysisNumber { get; set; }

        /// <summary>
        /// 分析数值
        /// </summary>
        public List<decimal> AnalysisNumbers { get; set; }
    }
}
