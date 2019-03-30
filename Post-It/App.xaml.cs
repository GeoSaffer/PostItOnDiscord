using System;
using System.Windows;
using System.Windows.Threading;
using Post_It.Utils;

namespace Post_It
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
            Dispatcher.UnhandledException += App_DispatcherUnhandledException;
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;
            // update Logs
            Log.Exception(e.Message);
            // Process unhandled exception
            DumpFile.Create(e);
        }

        private static void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //update Logs
            Log.Exception(e.Exception.Message);
            // Process unhandled exception
            DumpFile.Create(e.Exception);
            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
