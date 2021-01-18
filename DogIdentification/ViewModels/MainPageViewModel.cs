using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace DogIdentification.ViewModels
{
    class MainPageViewModel
    {
        public MainPageViewModel()
        {
            TakePhotoCommand = new Command(() =>
            {
                //Taking Photo logic
            });
        }


        public Command TakePhotoCommand { get; }

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
