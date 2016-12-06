using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace SayAnything
{
    public class RoundResult : ContentPage
    {
        ObservableCollection<UserData> gameData;
        int roundNo;
        App app = Application.Current as App;
        string userName;

        public RoundResult(string name)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = Color.White;

            //get gameData from server
            List<UserData> playerList = app.SignalRClient.GetScore();

            app.SignalRClient.OnGameEnd += async (gameEnd) =>
            {
                if (gameEnd)
                {
                    await Navigation.PushAsync(new FinalResults(userName));
                }
                else
                {
                    await Navigation.PushAsync(new Room(userName));
                }
            };

            roundNo = 1;
            gameData = gameData = new ObservableCollection<UserData>(playerList);

            Label RRLabel1 = new Label
            {
                TextColor = Color.Blue,
                FontSize = 20,
                Text = "Result of this round:"
            };

            ListView roundResult = new ListView();
            roundResult.ItemsSource = gameData;
            roundResult.VerticalOptions = LayoutOptions.FillAndExpand;
            //roundResult.RowHeight = 60;
            roundResult.ItemTemplate = new DataTemplate(() =>
            {
                Label players = new Label
                {
                    TextColor = Color.Blue,
                    FontSize = 18
                };
                players.SetBinding(Label.TextProperty, "UserName");

                Label numOfBets = new Label
                {
                    TextColor = Color.Blue,
                    FontSize = 12
                };
                numOfBets.SetBinding(Label.TextProperty, new Binding("BetRecieved", BindingMode.OneWay, null, null, "Bets recieved: {0}"));

                Label totalScore = new Label
                {
                    TextColor = Color.Blue,
                    FontSize = 12
                };
                totalScore.SetBinding(Label.TextProperty, new Binding("TotalScore", BindingMode.OneWay, null, null, "Total Score: {0}"));

                Label thisRoundScore = new Label
                {
                    TextColor = Color.Blue,
                    FontSize = 12
                };
                thisRoundScore.SetBinding(Label.TextProperty, new Binding("ThisRoundScore", BindingMode.OneWay, null, null, "This Round: {0}"));

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
                        Spacing = 0,
                        Children =
                            {
                                new StackLayout
                                {
                                    Spacing = 15,
                                    Orientation = StackOrientation.Horizontal,
                                    Children =
                                    {
                                        players, numOfBets, totalScore, thisRoundScore
                                    }
                                },
                                playerAnswer
                            }
                    }
                };
            });

            int countDown = 10;
            Label RRCoundDown = new Label
            {
                TextColor = Color.Blue,
                FontSize = 20,
                Text = "This round will finish in " + countDown + " seconds"
            };

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                countDown--;
                RRCoundDown.Text = "This round will finish in " + countDown + " seconds";
                if (countDown == 0)
                {
                    app.SignalRClient.RoundFinish();
                    return false;
                }
                return true;
            });

            Content = new StackLayout
            {
                Children = {
                    RRLabel1,
                    roundResult
                }
            };
        }

    }
}
