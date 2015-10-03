using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using pixel_place.Filters;

namespace pixel_place.Windsor
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Register(Component.For<MainWindow>()
                                        .LifestyleSingleton());

            container.Register(Classes.FromThisAssembly()
                .BasedOn<Window>()
                .If(a => a != typeof(MainWindow))
                .LifestyleTransient()
                .WithServiceBase()
                .WithServiceSelf());

            container.Register(Classes.FromThisAssembly()
                .BasedOn<IImageFilter>()
                .WithServiceBase()
                .LifestyleTransient());
        }
    }
}
