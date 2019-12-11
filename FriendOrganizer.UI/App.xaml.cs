using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using FriendOrganizer.UI.Startup;

namespace FriendOrganizer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();

            var container = bootstrapper.Bootstrap();

            var maniWindow = container.Resolve<MainWindow>();
            maniWindow.Show();
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                "Unexpected error occured. Please inform the admin." + 
                Environment.NewLine + e.Exception.Message,
                "Unexpected error");

            e.Handled = true;
        }
    }
}
