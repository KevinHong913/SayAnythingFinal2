using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SayAnything
{
    public class HomePage : ContentPage
    {
        
        //pass this data to server when join room
        //use a void function to update gameData from server
        Entry entry = new Entry
        {
            Placeholder = "Please enter your name here"
        };
        App app = Application.Current as App;
        string userName;
        ObservableCollection<UserData> gameData;

        public HomePage()
        {
            List<UserData> list = app.SignalRClient.GetPlayerList();

            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = Color.White;

            //Get gameData from server from server;
            //ObservableCollection OC = new ObeservableCollection(List)
            gameData = new ObservableCollection<UserData>(list);

            //change display names when new user joins
            app.SignalRClient.OnUserAdded += (userList) => {
                gameData = new ObservableCollection<UserData>(userList);
            };

            app.SignalRClient.OnUserDeleted += (userList) =>
            {
                gameData = new ObservableCollection<UserData>(userList);
            };

            var connectionLabel = new Label
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BindingContext = app.SignalRClient
            };
            connectionLabel.SetBinding(Label.TextProperty, "ConnectionState");

            //background image, place at bottom
            Image image = new Image
            {
                Source = "SA3.jpg",
                Opacity = 0.8,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };


            //show players in the room, at top

            //Use ListView to display all players in the room
            ListView existedPlayers = new ListView();
            existedPlayers.ItemsSource = gameData;
            //gameData.Data.Add(new UserData("new player"));
            existedPlayers.VerticalOptions = LayoutOptions.FillAndExpand;
            //existedPlayers.RowHeight = 30;
            existedPlayers.ItemTemplate = new DataTemplate(() =>
            {
                Label players = new Label
                {
                    TextColor = Color.Blue,
                    //BackgroundColor = Color.Yellow,
                    FontSize = 18
                };
                players.SetBinding(Label.TextProperty, "UserName");

                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Children =
                            {
                                players
                            }
                    }
                };
            });

            var label = new Label
            {
                //BackgroundColor = Color.Yellow,
                TextColor = Color.Blue,
                Text = "Players in the room: \n",
                HeightRequest = 30,
                WidthRequest = 200,
                FontSize = 20,
                FontAttributes = FontAttributes.Italic
                
               
            };

            Frame frame = new Frame
            {
                
                OutlineColor = Color.Black,
                
                HasShadow = true,
                Padding = 5,
                //HeightRequest = 150,
                //WidthRequest = 200,
                Content = label
            };

            //button for join room
            Button join = new Button
            {
                Text = "Join",
                FontSize = 20,
                
                BackgroundColor = Color.Silver,
                TextColor = Color.Red,
                BorderRadius = 15

            };
            join.Clicked += Join_Clicked;

            Content = new StackLayout
            {
                Children = {
                    connectionLabel,
                    label,
                    existedPlayers,
                    entry,
                    join,
                    image
                }
            };
        }

        private async void Join_Clicked(object sender, EventArgs e)
        {
            string name = entry.Text;
            app.SignalRClient.AddNewPlayer(name);            
            userName = name;
            await Navigation.PushAsync(new Room(userName));
        }
    }
}
