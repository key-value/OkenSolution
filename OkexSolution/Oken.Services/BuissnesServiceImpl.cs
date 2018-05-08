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

        public static event Action<string, int> UpdateCurrencyBodyAction;


        public void onReceive(string msg)
        {
            var regex = new Regex("ok_sub_futureusd_([^_]*)_ticker_");
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
                    if (match.Groups.Count != 2)
                    {
                        return;
                    }
                    Group matchGroup = match.Groups[1];
                    switch (matchGroup.Value)
                    {
                        case "addChannel":
                            break;
                        default:
                            MemoryDB.dictionary.AddOrUpdate(matchGroup.Value, objContract,
                                (s, contract) => objContract);
                            UpdateCurrencyBodyAction?.BeginInvoke(matchGroup.Value, 1, null, null);
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
