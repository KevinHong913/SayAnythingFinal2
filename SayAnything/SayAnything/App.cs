using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SayAnything
{
    public class App : Application
    {
        public SignalRClient SignalRClient = new SignalRClient("http://localhost:8080");

        public App()
        {
            //show an error if the connection doesn't succeed for some reason
            SignalRClient.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                    MainPage.DisplayAlert("Error", "An error occurred when trying to connect to SignalR: " + task.Exception.InnerExceptions[0].Message, "OK");
            });

            //try to reconnect every 10 seconds, just in case
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                if (!SignalRClient.IsConnectedOrConnecting)
                    SignalRClient.Start();
                return true;
            });


            MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
