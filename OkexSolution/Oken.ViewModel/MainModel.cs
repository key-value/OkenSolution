using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Oken.ViewModel
{
    public class MainModel : ObservableObject
    {
        private int _state;
        private ObservableCollection<AnalysisCurrency> _analysisCurrencies = new ObservableCollection<AnalysisCurrency>();
        private string _stateName;

        public int State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public string StateName
        {
            get { return _stateName; }
            set { SetProperty(ref _stateName, value); }
        }

        public ObservableCollection<AnalysisCurrency> AnalysisCurrencies
        {
            get { return _analysisCurrencies; }
            set { _analysisCurrencies = value; }
        }

        public void UpdateState(int state)
        {
            State = state;
            switch (state)
            {
                case 0:
                    StateName = "停止"; break;
                case 1:
                    StateName = "启动中"; break;
                case 2:
                    StateName = "正在连接"; break;
                case 3:
                    StateName = "已连接"; break;
                case 4:
                    StateName = "连接中断"; break;
                case 5:
                    StateName = "连接中断...正在重连"; break;
                case 6:
                    StateName = "重连失败"; break;
                default: break;
            }
        }
    }
}
