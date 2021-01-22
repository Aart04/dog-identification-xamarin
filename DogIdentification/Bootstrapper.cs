using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DogIdentification
{
    public static class Bootstrapper
    {
        public static IBootstrapper PlatformSpecific;
        public static IContainer Container;

        public static void Init(Application app)
        {
            var builder = new ContainerBuilder();

            PlatformSpecific.Init(builder);


            Container = builder.Build();
            DependencyResolver.ResolveUsing(type => Container.IsRegistered(type) ? Container.Resolve(type) : null);
        }
    }

    public interface IBootstrapper
    {
        void Init(ContainerBuilder builder);
    }
}
