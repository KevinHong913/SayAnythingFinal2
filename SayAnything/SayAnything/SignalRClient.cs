using System;
using Microsoft.AspNet.SignalR.Client;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Collections.Generic;

namespace SayAnything
{
    public class UserData
    {
        public string Username { get; set; }
        public bool IsHost { get; set; }
        public string Answer { get; set; }
        //public List<int> Scores { get; set; }
        public int TotalScore { get; set; }
        public int ThisRoundScore { get; set; }
        public int BettingChoice { get; set; }
        public string BettingUsername { get; set; }
        public int BetRecieved { get; set; }

        public UserData(string username)
        {
            Username = username;
            IsHost = false;
            Answer = "";
            //Scores = new List<int>();
            TotalScore = 0;
            ThisRoundScore = 0;
            BettingChoice = -1;
            BettingUsername = "";
            BetRecieved = 0;
        }
    }

    public class SignalRClient : INotifyPropertyChanged
    {
        private HubConnection Connection;
        private IHubProxy ChatHubProxy;

        public delegate void UserAdded(List<UserData> userList);
        public event UserAdded OnUserAdded;
        public delegate void UserDeleted(List<UserData> userList);
        public event UserDeleted OnUserDeleted;
        public delegate void GameStarted(List<UserData> userList, int questionNum);
        public event GameStarted OnGameStarted;
        public delegate void ObjectSubmitted(List<UserData> userList, bool isFinished);
        public event ObjectSubmitted OnAnswerSubmitted;
        public event ObjectSubmitted OnBetSubmitted;
        public delegate void GameEnd(bool gameEnd);
        public event GameEnd OnGameEnd;


        public SignalRClient(string url)
        {
            Connection = new HubConnection(url);
            Connection.Error += ex => Debug.WriteLine("SignalR error: {0}", ex.Message);

            Connection.StateChanged += (StateChange obj) => {
                OnPropertyChanged("ConnectionState");
            };

            ChatHubProxy = Connection.CreateHubProxy("SayAnything");

            listenEvents(); // register all listening events

        }


        /***** Listen to Events *****/
        public void listenEvents()
        {
            ChatHubProxy.On<List<UserData>>("UserAdded", userList =>
            {
                OnUserAdded?.Invoke(userList);
            });

            ChatHubProxy.On<List<UserData>>("UserDeleted", userList =>
            {
                OnUserDeleted?.Invoke(userList);
            });

            ChatHubProxy.On<List<UserData>, int>("GameStart", (userList, questionNum) =>
            {
                OnGameStarted?.Invoke(userList, questionNum);
            });

            ChatHubProxy.On<List<UserData>, bool>("AnswerSubmitted", (userList, isFinished) =>
            {
                OnAnswerSubmitted?.Invoke(userList, isFinished);
            });

            ChatHubProxy.On<List<UserData>, bool>("BetSubmitted", (userList, isFinished) =>
            {
                OnBetSubmitted?.Invoke(userList, isFinished);
            });

            ChatHubProxy.On<bool>("GameEnd", (gameEnd) =>
            {
                OnGameEnd?.Invoke(gameEnd);
            });

        }

        /***** Invoke Events *****/
        public void AddNewPlayer(string username)
        {
            ChatHubProxy.Invoke("NewPlayer", username);
        }

        public List<UserData> GetPlayerList()
        {
            var result = ChatHubProxy.Invoke<List<UserData>>("GetPlayerList").Result;
            Debug.WriteLine(result.Count);
            return result;
        }

        public void DeletePlayer(string username)
        {
            ChatHubProxy.Invoke("DeletePlayer", username);
        }

        public void StartGame()
        {
            ChatHubProxy.Invoke("StartGame");
        }

        public void SubmitAnswer(string username, string answer)
        {
            // empty answer filter is not working...
            if (!(answer == "" || answer == null))
                ChatHubProxy.Invoke("SubmitAnswer", username, answer);
        }

        public void SubmitBet(string username, int betNum)
        {
            ChatHubProxy.Invoke("SubmitBet", username, betNum);
        }

        public List<UserData> GetScore()
        {
            var result = ChatHubProxy.Invoke<List<UserData>>("GetPlayerList").Result;
            Debug.WriteLine(result.Count);
            return result;
        }

        public void RoundFinish()
        {
            ChatHubProxy.Invoke("RoundFinish");
        }

        /***** OTHER *****/

        public Task Start()
        {
            return Connection.Start();
        }

        public bool IsConnectedOrConnecting
        {
            get
            {
                return Connection.State != ConnectionState.Disconnected;
            }
        }

        public ConnectionState ConnectionState { get { return Connection.State; } }

        public static async Task<SignalRClient> CreateAndStart(string url)
        {
            var client = new SignalRClient(url);
            await client.Start();
            return client;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}