using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

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
    }   
}
