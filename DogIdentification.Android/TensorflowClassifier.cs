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
            var modelByteBuffer = LoadModel("dog-identification"); 
            var interpreter = new Xamarin.TensorFlow.Lite.Interpreter(modelByteBuffer);

            var inputTensor = interpreter.GetInputTensor(0);

            var shape = inputTensor.Shape();
            
        }
            
        private ByteBuffer LoadModel(string path)
        {
            var assetDescriptor = Application.Context.Assets.OpenFd(path);
            var stream = new FileInputStream(assetDescriptor.FileDescriptor);

            ByteBuffer byteBuffer = ByteBuffer.Allocate(stream.Available());
            var channel = stream.Channel.Read(byteBuffer);
            
            return byteBuffer; 
        }
    }
}