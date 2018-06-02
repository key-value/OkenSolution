using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Oken.Services;
using Oken.ViewModel;
using websocket;

namespace WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        static string url = "wss://real.okex.com:10440/websocket/okexapi";
        //国际站配置为"wss://real.okcoin.com:10440/websocket/okcoinapi"
        List<string> bibiList = new List<string>();

        private MainModel _mainModel = new MainModel();

        WebSocketBase wb;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _mainModel;
            BuissnesServiceImpl.StateChangeAction += BuissnesServiceImpl_StateChangeAction;
            BuissnesServiceImpl.UpdateCurrencyBodyAction += BuissnesServiceImpl_UpdateCurrencyBodyAction;
            Loaded += MainWindow_Loaded;
        }

        private object obj = new object();
        private void BuissnesServiceImpl_UpdateCurrencyBodyAction(string arg1, string arg3, Currencie arg2)
        {
            this.Dispatcher.Invoke(() =>
            {
                var hit = false;
                switch (arg1)
                {
                    case "btc_usdt":
                        hit = true;
                        _mainModel.btc_usdt_Buy = arg2.Buy_Usdt;
                        _mainModel.btc_usdt_Sell = arg2.Sell_Usdt;
                        break;
                    case "eth_btc":
                        hit = true;
                        _mainModel.eth_btc_Buy = arg2.Buy_Btc;
                        _mainModel.eth_btc_Sell = arg2.Sell_Btc;
                        break;
                    case "eth_usdt":
                        hit = true;
                        _mainModel.eth_usdt_Buy = arg2.Buy_Usdt;
                        _mainModel.eth_usdt_Sell = arg2.Sell_Usdt;
                        break;
                }

                if (hit)
                {
                    _mainModel.AnalysisCurrencies.Clear();
                    var newList = MemoryDB.dictionary.ToDictionary(x => x.Key, x => x.Value);
                    foreach (var currency in newList)
                    {
                        FreshTable(currency.Key, currency.Value);
                    }
                }
                else
                {
                    FreshTable(arg3, arg2);
                }

            });

            Task.Factory.StartNew(() =>
            {
                if (DateTime.Now > lastTime.AddSeconds(20))
                {
                    // 然后在程序中调用
                    //uint beep = 0x00000010;
                    //MessageBeep(beep);
                    Console.Beep(500, 100);
                    lastTime = DateTime.Now;
                }
            });

        }

        private static DateTime lastTime = DateTime.Now;


        private void FreshTable(string arg3, Currencie arg2)
        {
            var currencyNum = 0;
            var analysisCurrency = _mainModel.AnalysisCurrencies.FirstOrDefault(x => x.Name == arg3);
            currencyNum = bibiList.IndexOf(arg3);
            if (analysisCurrency != null)
            {
                _mainModel.AnalysisCurrencies.Remove(analysisCurrency);
            }
            else
            {
                analysisCurrency = new AnalysisCurrency();
                analysisCurrency.Name = arg3;
            }
            if (currencyNum > _mainModel.AnalysisCurrencies.Count)
            {
                currencyNum = _mainModel.AnalysisCurrencies.Count;
            }
            var decorate = new Decorate();
            decorate.Analysis(analysisCurrency, this._mainModel, arg2);
            if (analysisCurrency.AnalysisNumber1 < _mainModel.MinNumber &&
                analysisCurrency.AnalysisNumber2 < _mainModel.MinNumber &&
                analysisCurrency.AnalysisNumber3 < _mainModel.MinNumber &&
                analysisCurrency.AnalysisNumber4 < _mainModel.MinNumber &&
                analysisCurrency.AnalysisNumber5 < _mainModel.MinNumber &&
                analysisCurrency.AnalysisNumber6 < _mainModel.MinNumber)
            {
                return;
            }
            _mainModel.AnalysisCurrencies.Insert(currencyNum, analysisCurrency);
            string analysisName = "";
            decimal analysisNumber = 0;
            if (analysisCurrency.AnalysisNumber1 > _mainModel.MinNumber)
            {
                analysisName = "ETH->USDT";
                analysisNumber = analysisCurrency.AnalysisNumber1;
            }
            if (analysisCurrency.AnalysisNumber2 > _mainModel.MinNumber)
            {
                analysisName = "ETH->BTC";
                analysisNumber = analysisCurrency.AnalysisNumber2;
            }
            if (analysisCurrency.AnalysisNumber3 > _mainModel.MinNumber)
            {
                analysisName = "USDT->ETH";
                analysisNumber = analysisCurrency.AnalysisNumber3;
            }
            if (analysisCurrency.AnalysisNumber4 > _mainModel.MinNumber)
            {
                analysisName = "USDT->BTC";
                analysisNumber = analysisCurrency.AnalysisNumber4;
            }
            if (analysisCurrency.AnalysisNumber5 > _mainModel.MinNumber)
            {
                analysisName = "BTC->ETH";
                analysisNumber = analysisCurrency.AnalysisNumber5;
            }
            if (analysisCurrency.AnalysisNumber6 > _mainModel.MinNumber)
            {
                analysisName = "BTC->USDT";
                analysisNumber = analysisCurrency.AnalysisNumber6;
            }
            this._mainModel.CurrencieMessages.Add(new AnalysisMessage(arg3, analysisNumber, DateTime.Now, analysisName));
            if (_mainModel.CurrencieMessages.Count > 200)
            {
                _mainModel.CurrencieMessages.RemoveAt(0);
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WebSocketService wss = new BuissnesServiceImpl();
            wb = new WebSocketBase(url, wss);

        }

        private void BuissnesServiceImpl_StateChangeAction(int obj)
        {
            this.Dispatcher.Invoke(() =>
            {
                _mainModel.UpdateState(3);
            });
        }

        private void SocketClient_OnClick(object sender, RoutedEventArgs e)
        {
            _mainModel.UpdateState(1);
            wb.start();
            Task.Factory.StartNew(() =>
            {
                var rootobjects = new List<Rootobject>();
                rootobjects.Add(Rootobject.CreateNew("eth_usdt"));
                rootobjects.Add(Rootobject.CreateNew("eth_btc"));
                rootobjects.Add(Rootobject.CreateNew("btc_usdt"));
                wb.send(JsonConvert.SerializeObject(rootobjects));
                foreach (var bibi in bibiList)
                {
                    rootobjects = new List<Rootobject>();
                    rootobjects.AddRange(Rootobject.CreateNewList(bibi));
                    wb.send(JsonConvert.SerializeObject(rootobjects));
                }
                _mainModel.UpdateState(2);
            });


        }

        private void InitCurrencie_OnClick(object sender, RoutedEventArgs e)
        {
            _mainModel.Imported = false;
            bibiList = new List<string>();
            new Thread(() =>
            {
                try
                {
                    var webRequest = (HttpWebRequest)WebRequest.Create("https://www.okex.com/v2/spot/markets/tickers");
                    webRequest.Proxy = new WebProxy("127.0.0.1:25378");
                    var webResponse = webRequest.GetResponse() as HttpWebResponse;
                    if (webResponse == null)
                    {
                        this.Dispatcher.Invoke(() => { MessageBox.Show("导入币种失败，请检查网络"); });
                        return;
                    }
                    using (var responseStream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        var result = responseStream.ReadToEnd();
                        responseStream.Close();
                        var currencyRoot = JsonConvert.DeserializeObject<CurrencyRoot>(result);
                        if (currencyRoot.data != null)
                        {
                            this.Dispatcher.Invoke(
                                () =>
                                {
                                    foreach (var symbol in currencyRoot.data.Select(x => x.symbol))
                                    {
                                        var curNameList = symbol.Split('_');
                                        foreach (var s in curNameList)
                                        {
                                            if (!bibiList.Contains(s))
                                            {
                                                bibiList.Add(s);
                                            }
                                        }
                                    }
                                    _mainModel.CurrencieNum = bibiList.Count;
                                    bibiList = bibiList.OrderBy(x => x.ToLower()).ToList();
                                });

                        }

                    }
                }
                catch (Exception exception)
                {
                    this.Dispatcher.Invoke(() => { MessageBox.Show("导入币种失败"); });
                }
                finally
                {
                    _mainModel.Imported = true;
                    _mainModel.UpdateState(_mainModel.State);
                }
            }).Start();
        }

        private void ReadCurrencie_OnClick(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                var excelEdit = new ExcelEdit();
                excelEdit.Open(AppDomain.CurrentDomain.BaseDirectory + "Currencie.xlsx");
                var sheet = excelEdit.GetSheet("default");
                var rowCount = sheet.UsedRange.Rows.Count;
                for (int i = 1; i <= rowCount; i++) //
                {
                    if (sheet.Rows[i] == null)
                    {
                        continue;
                    }
                    var curNameList = sheet.Cells[i, "A"].Value2.ToString().Split('_');
                    foreach (var s in curNameList)
                    {
                        if (!bibiList.Contains(s))
                        {
                            bibiList.Add(s);
                        }
                    }
                }
                _mainModel.CurrencieNum = bibiList.Count;
                bibiList = bibiList.OrderBy(x => x.ToLower()).ToList();

                _mainModel.Imported = true;
                _mainModel.UpdateState(_mainModel.State);
            });
        }
    }
}
