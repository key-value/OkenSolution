using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Oken.Services;

namespace websocket
{
    public class BuissnesServiceImpl : WebSocketService
    {


        public static event Action<int> StateChangeAction;

        public static event Action<string, string, Currencie> UpdateCurrencyBodyAction;


        public void onReceive(string msg)
        {
            var regex = new Regex(@"ok_sub_spot_(?<input>[^_]*)_(?<output>[^_]*)_ticker");
            Console.WriteLine(msg);
            if (!regex.IsMatch(msg))
            {
                if (msg == "{\"event\":\"pong\"}")
                {
                    StateChangeAction?.BeginInvoke(1, null, null);
                }
                return;
            }
            try
            {
                var contracts = JsonConvert.DeserializeObject<List<Contract>>(msg);
                if (contracts == null)
                {
                    return;
                }
                foreach (var objContract in contracts)
                {
                    var match = regex.Match(objContract.channel);

                    Group input = match.Groups["input"];
                    Group output = match.Groups["output"];
                    if (string.IsNullOrEmpty(input.Value) || string.IsNullOrEmpty(output.Value))
                    {
                        continue;
                    }
                    var matchput = $"{input.Value}_{output.Value}";
                    switch (matchput)
                    {
                        case "addChannel":
                            break;
                        default:

                            var currencieName = input.Value;
                            var currencie = new Currencie();
                            MemoryDB.dictionary.GetOrAdd(currencieName, currencie);
                            switch (output.Value.ToLower())
                            {
                                case "eth":
                                    currencie.Buy_Eth = objContract.data.Buy();
                                    currencie.Sell_Eth = objContract.data.Sell();
                                    break;
                                case "btc":
                                    currencie.Buy_Btc = objContract.data.Buy();
                                    currencie.Sell_Btc = objContract.data.Sell();
                                    break;
                                case "usdt":
                                    currencie.Buy_Usdt = objContract.data.Buy();
                                    currencie.Sell_Usdt = objContract.data.Sell();
                                    break;
                                default: return;
                            }
                            UpdateCurrencyBodyAction?.BeginInvoke(matchput, input.Value.ToLower(), currencie, null, null);
                            break;

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
