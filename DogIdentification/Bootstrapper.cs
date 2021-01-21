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

        public static void Init(Application app)
        {
            var builder = new ContainerBuilder();

            PlatformSpecific.Init(builder);


            var container = builder.Build();
            DependencyResolver.ResolveUsing(type => container.IsRegistered(type) ? container.Resolve(type) : null);
        }
    }

    public interface IBootstrapper
    {
        void Init(ContainerBuilder builder);
    }
}
