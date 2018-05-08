using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        List<string> bibiList = new List<string>() { "btc", "ltc", "eth", "etc", "bch" };

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

        private void BuissnesServiceImpl_UpdateCurrencyBodyAction(string arg1, int arg2)
        {
            this.Dispatcher.Invoke(() =>
            {
                var analysisCurrency = _mainModel.AnalysisCurrencies.FirstOrDefault(x => x.Name == arg1);
                if (analysisCurrency != null)
                {
                    _mainModel.AnalysisCurrencies.Remove(analysisCurrency);
                }
                else
                {
                    analysisCurrency = new AnalysisCurrency();
                }
                analysisCurrency.Name = arg1;
                analysisCurrency.AnalysisNumber = arg2;
                _mainModel.AnalysisCurrencies.Add(analysisCurrency);
            });
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
                foreach (var bibi in bibiList)
                {
                    rootobjects.Add(Rootobject.CreateNew(bibi));
                }
                wb.send(JsonConvert.SerializeObject(rootobjects));
                _mainModel.UpdateState(2);
            });


        }
    }
}
