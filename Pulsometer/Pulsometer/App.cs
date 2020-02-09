using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Tizen.Wearable.CircularUI.Forms;

namespace Pulsometer
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = new MainPage();
            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            MessagingCenter.Send<Application>(this, "sleep");
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
