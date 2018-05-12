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
            if (mainModel.eth_usdt_Sell != 0 && currencie.Sell_Eth != 0)
            {
                analysisCurrency.AnalysisNumber1 = (1 * currencie.Buy_Usdt / currencie.Sell_Eth /
                                                     mainModel.eth_usdt_Sell);
            }
            if (mainModel.eth_btc_Sell != 0 && currencie.Sell_Eth != 0)
            {
                analysisCurrency.AnalysisNumber2 = (
                    1 * currencie.Buy_Btc / currencie.Sell_Eth / mainModel.eth_btc_Sell);
            }

            if (currencie.Sell_Usdt != 0 && mainModel.eth_usdt_Buy != 0)
            {
                analysisCurrency.AnalysisNumber3 = (currencie.Buy_Eth * mainModel.eth_usdt_Buy / currencie.Sell_Usdt);
            }


            if (currencie.Sell_Usdt != 0 && mainModel.btc_usdt_Buy != 0)
            {
                analysisCurrency.AnalysisNumber4 = (1 * currencie.Buy_Btc * mainModel.btc_usdt_Buy / currencie.Sell_Usdt);
            }


            if (mainModel.eth_btc_Buy != 0 && currencie.Sell_Btc != 0)
            {
                analysisCurrency.AnalysisNumber5 = (1 * currencie.Buy_Eth *
                                                     mainModel.eth_btc_Buy / currencie.Sell_Btc);
            }
            if (mainModel.btc_usdt_Sell != 0 && currencie.Sell_Btc != 0)
            {
                analysisCurrency.AnalysisNumber6 = (1 * currencie.Buy_Usdt / currencie.Sell_Btc /
                                                     mainModel.btc_usdt_Sell);
            }

            return analysisCurrency;
        }
    }
}
