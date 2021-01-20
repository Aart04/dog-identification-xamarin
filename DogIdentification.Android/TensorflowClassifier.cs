using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogIdentification.Droid
{
    class TensorflowClassifier : IClassifier
    {
        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        public Task Classify(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}