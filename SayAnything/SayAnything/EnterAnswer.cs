using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SayAnything
{
    public class EnterAnswer : ContentPage
    {
        //get question number from server
        int questionNo;
        string userName;
        App app = Application.Current as App;
        string answer;
        Entry EAAnswerEntry = new Entry
        {
            FontSize = 20,
            Placeholder = "Please enter your response here",
            TextColor = Color.Red
        };

        public EnterAnswer(int qNo, string name)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            questionNo = qNo;
            userName = name;

            BackgroundColor = Color.White;
            QuestionSet questions = new QuestionSet();

            Label EAMainLabel = new Label
            {
                FontSize = 20,
                Text = "The Question of this round is: \n" + questions.QSet[questionNo].Qs,
                TextColor = Color.Blue
            };

            Button EAConfirm = new Button
            {
                Text = "Confirm",
                FontSize = 20,
                BackgroundColor = Color.Silver,
                TextColor = Color.Red,
                BorderRadius = 15
            };
            EAConfirm.Clicked += EAConfirm_Clicked;

            Content = new StackLayout
            {
                Spacing = 20,
                Children = {
                    EAMainLabel,
                    EAAnswerEntry,
                    EAConfirm
                }
            };
        }

        private async void EAConfirm_Clicked(object sender, EventArgs e)
        {
            //send answer to server
            answer = EAAnswerEntry.Text;
            app.SignalRClient.SubmitAnswer(userName, answer);

            await Navigation.PushAsync(new WaitingAnswer(userName));
        }
    }
}
