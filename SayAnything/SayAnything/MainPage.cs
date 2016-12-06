using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SayAnything
{
    public class MainPage : ContentPage
    {
        String name = "tom";

        public MainPage()
        {
            
            // define layout
            AbsoluteLayout absoluteLayout = new AbsoluteLayout
            {
                Padding = new Thickness(50)
            };

            //define children of layout

            //background image, place at bottom
            Image image = new Image
            {
                Source = "SA3.jpg",
                Opacity = 0.8,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            //show players in the room, at top
            var label = new Label { Text = "I'm bottom center on every device." };
            AbsoluteLayout.SetLayoutBounds(label, new Rectangle(.5, 1, .5, .1));
            AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.All);

            //entry box for name
            Entry entry = new Entry
            {
                Placeholder = "Please enter your name here"

            };
            entry.Completed += Entry_Completed;
            AbsoluteLayout.SetLayoutBounds(entry, new Rectangle(0, .5, .3, .1));
            AbsoluteLayout.SetLayoutFlags(entry, AbsoluteLayoutFlags.All);

            //button for join room
            Button join = new Button
            {
                Text = "Join",
                FontSize = 20,

            };
            join.Clicked += Join_Clicked;
            AbsoluteLayout.SetLayoutBounds(join, new Rectangle(.7, .5, .2, .1));
            AbsoluteLayout.SetLayoutFlags(join, AbsoluteLayoutFlags.All);

            //add layout children 
            //absoluteLayout.Children.Add(image, new Point(0, 0));
            absoluteLayout.Children.Add(label);
            absoluteLayout.Children.Add(join);
            absoluteLayout.Children.Add(entry);

            Content = absoluteLayout;
            /*{
                
                Children = {
                    image,
                    new Label
                    {
                        Text = "Hello Page",
                        FontSize = 40
                    }
                }
            };*/
        }

        private async void Join_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Room());
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            Entry entry = (Entry)sender;
            name = entry.Text;
        }
    }
}
