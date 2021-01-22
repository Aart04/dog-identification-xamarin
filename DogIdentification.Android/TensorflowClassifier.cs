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
using Android.Content.Res;
using Java.IO;
using Java.Nio;
using Java.Nio.Channels;
using System.IO;

namespace DogIdentification.Droid
{
    class TensorflowClassifier : IClassifier
    {
        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        public async Task Classify(byte[] bytes)
        {
            var modelByteBuffer = LoadModel("dog-identification.tflite"); 
            var interpreter = new Xamarin.TensorFlow.Lite.Interpreter(modelByteBuffer);

            var inputTensor = interpreter.GetInputTensor(0);

            var shape = inputTensor.Shape();

            var width = shape[1];
            var height = shape[2];
        }
            
        private MappedByteBuffer LoadModel(string path)
        {
            var assetDescriptor = Application.Context.Assets.OpenFd(path);
            var stream = new FileInputStream(assetDescriptor.FileDescriptor);
            var mappedByteBuffer = stream.Channel.Map(FileChannel.MapMode.ReadOnly, assetDescriptor.StartOffset, assetDescriptor.DeclaredLength);
            return mappedByteBuffer; 
        }


    }
}