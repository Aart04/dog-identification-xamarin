﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DogIdentification
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Bootstrapper.Init(this);

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
