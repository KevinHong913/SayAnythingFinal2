using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SayAnything
{
    public class WaitingBet : ContentPage
    {
        string userName;
        App app = Application.Current as App;

        public WaitingBet(string name)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = Color.White;

            userName = name;
            app.SignalRClient.OnBetSubmitted += async(userList, isFinished) =>
            {
                if (isFinished)
                {
                    await Navigation.PushAsync(new RoundResult(userName));
                }
            };

            Label WBMainLabel = new Label
            {
                FontSize = 20,
                Text = "Please wait for other players to place their bets...",
                TextColor = Color.Blue
            };

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    WBMainLabel
                }
            };
        }
    }
}
