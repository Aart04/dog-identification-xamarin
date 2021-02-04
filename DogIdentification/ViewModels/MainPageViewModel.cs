using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.ComponentModel;
using Autofac;
using System.IO;
using System.Linq;

namespace DogIdentification.ViewModels
{
    class MainPageViewModel: INotifyPropertyChanged
    {
        public Command TakePhotoCommand { get; }
        public Command PickPhotoCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        private IClassifier offlineInceptionV3Model;

        public MainPageViewModel()
        {
            using (var scope = Bootstrapper.Container.BeginLifetimeScope())
            {
                offlineInceptionV3Model = scope.Resolve<IClassifier>();
            }


            TakePhotoCommand = new Command(async () =>
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                
                var stream = await photo.OpenReadAsync();
                
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                stream = await photo.OpenReadAsync();

                if (stream != null) 
                {   
                    Photo = ImageSource.FromStream(() => { return stream; });
                    Console.WriteLine("Photo was taken");
                }
                else
                {
                    Console.WriteLine("Photo wasn't taken");
                }

                byte[] bytesArray = memoryStream.ToArray();

                offlineInceptionV3Model.ClassificationCompleted += Classifier_ClassificationCompleted;

                await offlineInceptionV3Model.Classify(bytesArray);
            });

            PickPhotoCommand = new Command(async () =>
            {
                var photo = await MediaPicker.PickPhotoAsync();

                var stream = await photo.OpenReadAsync();

                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                stream = await photo.OpenReadAsync();

                if (stream != null)
                {
                    Photo = ImageSource.FromStream(() => { return stream; });
                    Console.WriteLine("Photo was picked");
                }
                else
                {
                    Console.WriteLine("Photo wasn't picked");
                }

                byte[] bytesArray = memoryStream.ToArray();

                offlineInceptionV3Model.ClassificationCompleted += Classifier_ClassificationCompleted;

                await offlineInceptionV3Model.Classify(bytesArray);
            });
        }

        private void Classifier_ClassificationCompleted(object sender, ClassificationEventArgs e)
        {
            var sortedList = e.Predictions.OrderByDescending(x => x.Probability);

            var top_prediction = sortedList.First();

            String results = "This is: " + top_prediction.TagName + " \n Accuracy: " + top_prediction.Probability*100;

            ResultInfo = results;

            ((IClassifier)sender).ClassificationCompleted -= Classifier_ClassificationCompleted;
        }
        

        ImageSource photo;
        public ImageSource Photo 
        { 
            get => photo;
            set 
            {
                photo = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Photo)));

                TakePhotoCommand.ChangeCanExecute();
            } 
        }

        String resultInfo;
        public String ResultInfo
        {
            get => resultInfo;
            set
            {
                resultInfo = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(ResultInfo)));
            }
        }
    }   
}
