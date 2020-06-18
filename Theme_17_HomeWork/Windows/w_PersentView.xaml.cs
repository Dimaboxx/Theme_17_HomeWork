using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using BankNameSpase;
using LogCenterNameSpace;

namespace Theme_17_HomeWork
{
    /// <summary>
    /// Interaction logic for w_PersentView.xaml
    /// </summary>
    public partial class w_PersentView : Window
    {
        public ObservableCollection<PercentsView> percentsViews;
        public w_PersentView()
        {
            //lst_Persents.ItemsSource = percentsViews;
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.wnd_PersentView = null;
        }
    }
}
