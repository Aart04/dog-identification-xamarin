using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DogIdentification.ViewModels
{
    class MainPageViewModel: INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            TakePhotoCommand = new Command(async () =>
            {
                var stream = await TakePhotoAsync();
                if (stream != null) 
                {   
                    Photo = ImageSource.FromStream(() => { return stream; });
                    Console.WriteLine("Photo was taken");
                }
                else
                {
                    Console.WriteLine("Photo wasn't taken");
                }
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

        async Task<System.IO.Stream> TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                var stream = await photo.OpenReadAsync();
                return stream;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"There was error during making a photo: {ex.Message}");
                return null;
            }
        }
    }   
}
