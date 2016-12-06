using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SayAnything
{
    public class WaitingAnswer : ContentPage
    {
        string userName;
        App app = Application.Current as App;

        public WaitingAnswer(string name)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = Color.White;
            userName = name;

            //wait for server to signal to go
            app.SignalRClient.OnAnswerSubmitted += async(userList, isFinished) =>
            {
                if (isFinished)
                {
                    await Navigation.PushAsync(new BettingPage(userName));
                }
            };

            Label WAMainLabel = new Label
            {
                FontSize = 20,
                Text = "Please wait for other players to respond...",
                TextColor = Color.Blue
            };

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    WAMainLabel
                }
            };

            

        }
    }
}
