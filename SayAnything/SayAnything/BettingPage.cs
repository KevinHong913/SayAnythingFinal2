using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace SayAnything
{
    public class BettingPage : ContentPage
    {
        App app = Application.Current as App;
        string userName;
        ObservableCollection<UserData> gameData;
        UserData selected = new UserData("init");

        public BettingPage(string name)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = Color.White;
            userName = name;

            List<UserData> playerList = app.SignalRClient.GetPlayerList();
            //get gameData from server
            gameData = new ObservableCollection<UserData>(playerList);

            Label BettingLabel1 = new Label
            {
                TextColor = Color.Blue,
                FontSize = 20,

                Text = "Please select your favoriate answer: \n"
            };

            ListView answers = new ListView();
            answers.ItemsSource = gameData;
            answers.VerticalOptions = LayoutOptions.FillAndExpand;
            answers.SeparatorVisibility = SeparatorVisibility.Default;
            answers.ItemTemplate = new DataTemplate(() =>
            {
                Label players = new Label
                {
                    TextColor = Color.Blue,
                    //BackgroundColor = Color.Yellow,
                    FontSize = 18
                };
                players.SetBinding(Label.TextProperty, "UserName");

                Label playerAnswer = new Label
                {
                    TextColor = Color.Blue,
                    //BackgroundColor = Color.Yellow,
                    FontSize = 12
                };
                playerAnswer.SetBinding(Label.TextProperty, "Answer");

                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Children =
                            {
                                players,
                                playerAnswer
                            }
                    }
                };
            });
            answers.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
            {
                selected = (UserData)sender;
            };

            Button confirmBet = new Button
            {
                Text = "Confirm Bet",
                FontSize = 20,
                BackgroundColor = Color.Silver,
                TextColor = Color.Red,
                BorderRadius = 15
            };

            confirmBet.Clicked += async(object sender, EventArgs e) =>
              {
                  if (selected.Username != "init")
                  {
                      int betNum = gameData.IndexOf((UserData)selected);
                      app.SignalRClient.SubmitBet(userName, betNum);
                      await Navigation.PushAsync(new WaitingBet(userName));
                  } else
                  {
                      await DisplayAlert("Alert", "Please select one answer you like", "OK");
                  }
              };

            Content = new StackLayout
            {
                Children = {
                    BettingLabel1,
                    answers,
                    confirmBet
                }
            };

        }
    }
}
