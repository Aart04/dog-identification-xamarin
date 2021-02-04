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
using Android.Graphics;

namespace DogIdentification.Droid
{
    class TensorflowClassifier : IClassifier
    {
        const int FloatSize = 4;
        const int PixelSize = 3;
        const float TensorflowInputScale = 255.0f;

        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        public async Task Classify(byte[] bytes)
        {
            var modelByteBuffer = LoadModel("dog-identification.tflite"); 
            var interpreter = new Xamarin.TensorFlow.Lite.Interpreter(modelByteBuffer);

            var inputTensor = interpreter.GetInputTensor(0);

            var shape = inputTensor.Shape();

            var width = shape[1];
            var height = shape[2];

            List<String> labels = new List<String>();

            var labelsStream = Application.Context.Assets.Open("labels.txt");
            
            using (StreamReader sr = new StreamReader(labelsStream))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    labels.Add(line);
                }
            }

            var input = GetPhotoAsByteBuffer(bytes, width, height);


            var outputLocations = new float[1][] { new float[labels.Count] };
            var outputs = Java.Lang.Object.FromArray(outputLocations);

            interpreter.Run(input, outputs);

            var classificationResults = outputs.ToArray<float[]>();

            System.Console.WriteLine("I got results: ");
            for (int i = 0; i < classificationResults[0].Length; i++)
            {
                System.Console.WriteLine($"{labels[i]} : {classificationResults[0][i]}");
            }
        }
            
        private MappedByteBuffer LoadModel(string path)
        {
            var assetDescriptor = Application.Context.Assets.OpenFd(path);
            var stream = new FileInputStream(assetDescriptor.FileDescriptor);
            var mappedByteBuffer = stream.Channel.Map(FileChannel.MapMode.ReadOnly, assetDescriptor.StartOffset, assetDescriptor.DeclaredLength);
            return mappedByteBuffer; 
        }

        private ByteBuffer GetPhotoAsByteBuffer(byte[] image, int width, int height)
        {

            var bitmap = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            
            var resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);
            
            var modelInputSize = FloatSize * height * width * PixelSize;
            var byteBuffer = ByteBuffer.AllocateDirect(modelInputSize);
            byteBuffer.Order(ByteOrder.NativeOrder());

            var pixels = new int[width * height];
            resizedBitmap.GetPixels(pixels, 0, resizedBitmap.Width, 0, 0, resizedBitmap.Width, resizedBitmap.Height);

            var pixel = 0;

            //Loop through each pixels to create a Java.Nio.ByteBuffer
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var pixelVal = pixels[pixel++];

                    byteBuffer.PutFloat(((pixelVal >> 16) & 0xFF)/ TensorflowInputScale);
                    byteBuffer.PutFloat(((pixelVal >> 8) & 0xFF)/ TensorflowInputScale);
                    byteBuffer.PutFloat(((pixelVal) & 0xFF)/ TensorflowInputScale);
                }
            }

            bitmap.Recycle();

            return byteBuffer;
        }


    }
}