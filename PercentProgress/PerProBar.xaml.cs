using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PercentProgress
{
    /// <summary>
    /// PerProBar.xaml 的交互逻辑
    /// </summary>
    public partial class PerProBar : UserControl, INotifyPropertyChanged
    {
        ResourceDictionary dictionary;
        public PerProBar()
        {
            InitializeComponent();
            this.DataContext = this;
            dictionary = new ResourceDictionary { Source = new Uri("/PercentProgress;component/BaseStyle.xaml", UriKind.Relative) };
        }

        private double _barValue = 0;

        public double BarValue
        {
            get
            {
                return _barValue;
            }

            set
            {
                if (value == 0)
                    return;
                else if (value > 100)
                    _barValue = 100;
                else
                    _barValue = value;
            }
        }

        private int _foregroundColor;

        public int ForegroundColor
        {
            get
            {
                return _foregroundColor;
            }

            set
            {
                _foregroundColor = value;
                LinearGradientBrush lgb = dictionary["ForegroundColor" + value] as LinearGradientBrush;
                if (lgb != null)
                    proBar.Foreground = txt.Foreground = percent.Foreground = lgb;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _valueText = "0";

        public string ValueText
        {
            get
            {
                return _valueText;
            }

            set
            {
                _valueText = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ValueText"));
                }
            }
        }

        private void ProgressBarGetValue()
        {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += Bgw_DoWork;
            bgw.RunWorkerAsync();
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < BarValue; i++)
            {
                System.Threading.Thread.Sleep(50);
                proBar.Dispatcher.Invoke(new Action(
                    delegate
                    {
                        if (proBar.Value <= BarValue)
                        {
                            proBar.Value++;
                        }
                    }));
                ValueText = i + "";
            }
            ValueText = BarValue + "";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressBarGetValue();
        }
    }
}
