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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Post_It.Dialogs
{
    /// <summary>
    /// Interaction logic for WaitDialog.xaml
    /// </summary>
    public partial class WaitDialog : Window, IDisposable
    {
        public Action Worker { get; set; }

        public WaitDialog(double mainWindowWidth, Action worker)
        {
            InitializeComponent();
            Worker = worker ?? throw new ArgumentNullException();
            this.Width = mainWindowWidth - 20;

        }
        public WaitDialog(double mainWindowWidth, Action worker, string message)
        {
            InitializeComponent();
            Worker = worker ?? throw new ArgumentNullException();
            Message.Content = message;
            this.Width = mainWindowWidth - 20;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Task.Factory.StartNew(Worker)
                .ContinueWith(t =>
                {
                   
                    Close(); 

                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
