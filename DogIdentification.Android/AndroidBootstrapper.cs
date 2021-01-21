using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DogIdentification.Droid
{
    class AndroidBootstrapper : IBootstrapper
    {
        public void Init(ContainerBuilder builder)
        {
            builder.RegisterType<TensorflowClassifier>().Keyed<IClassifier>("OfflineInceptionModel");
        }
    }
}