using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DogIdentification
{
    class Photo
    {
        public async Task<System.IO.Stream> TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                var stream = await photo.OpenReadAsync();
                return stream;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was error during making a photo: {ex.Message}");
                return null;
            }
        }
    }
}
