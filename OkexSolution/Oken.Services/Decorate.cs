using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oken.ViewModel;

namespace Oken.Services
{
    public class Decorate
    {
        public AnalysisCurrency Analysis(AnalysisCurrency analysisCurrency, MainModel mainModel, Currencie currencie)
        {
            analysisCurrency.AnalysisNumbers = new List<decimal>();
            analysisCurrency.AnalysisNumbers.Add(1 / currencie.Sell_Eth * currencie.Buy_Usdt / mainModel.eth_usdt_Sell);
            analysisCurrency.AnalysisNumbers.Add(1 / currencie.Sell_Eth * currencie.Buy_Usdt / mainModel.eth_usdt_Sell);
            analysisCurrency.AnalysisNumbers.Add(1 / currencie.Sell_Eth * currencie.Buy_Usdt * mainModel.eth_usdt_Sell);
            analysisCurrency.AnalysisNumbers.Add(1 / currencie.Sell_Eth * currencie.Buy_Usdt * mainModel.eth_usdt_Sell);
            analysisCurrency.AnalysisNumbers.Add(1 / currencie.Sell_Eth * currencie.Buy_Usdt * mainModel.eth_usdt_Sell);
            analysisCurrency.AnalysisNumbers.Add(1 / currencie.Sell_Eth * currencie.Buy_Usdt / mainModel.eth_usdt_Sell);
            return analysisCurrency;
        }
    }
}
