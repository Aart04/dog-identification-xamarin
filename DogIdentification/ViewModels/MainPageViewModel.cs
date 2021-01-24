using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.ComponentModel;
using Autofac;
using System.IO;

namespace DogIdentification.ViewModels
{
    class MainPageViewModel: INotifyPropertyChanged
    {
        private IClassifier offlineInceptionV3Model;

        public MainPageViewModel()
        {
            using (var scope = Bootstrapper.Container.BeginLifetimeScope())
            {
                offlineInceptionV3Model = scope.Resolve<IClassifier>();
            }


            TakePhotoCommand = new Command(async () =>
            {
                var photo = new Photo();
                var stream = await photo.TakePhotoAsync();
                if (stream != null) 
                {   
                    Photo = ImageSource.FromStream(() => { return stream; });
                    Console.WriteLine("Photo was taken");
                }
                else
                {
                    Console.WriteLine("Photo wasn't taken");
                }
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                

                byte[] bytesArray = memoryStream.ToArray();

                await offlineInceptionV3Model.Classify(bytesArray);

            });
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

            
        public Command TakePhotoCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
    }   
}
