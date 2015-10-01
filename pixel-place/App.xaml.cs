using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using pixel_place.Windsor;

namespace pixel_place
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IWindsorContainer _container;
        private Window _mainWindow;

        public App()
        {
            _container = new WindsorContainer();
            _container.Install(new WindsorInstaller());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _mainWindow = _container.Resolve<MainWindow>();
            _mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mainWindow.Hide();
            _container.Release(_mainWindow);
            _container.Dispose();

            base.OnExit(e);
        }
    }
}
