using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Post_It.Bot
{
    public class DiscordBot
    {
        #region private

        private DiscordSocketClient _client;
        private readonly string _token;
        private bool _isReady;

        #endregion

        #region public
        public bool IsReady => _isReady;
        public DiscordSocketClient Client => _client;
        #endregion
        
        public DiscordBot(string token) 
        {
            _token = token;
        }

        internal void Run()
        {
            //setup the client 
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            //Setup events
            _client.Ready += OnClientOnReady;
            _client.Connected += OnClientOnConnected;
            _client.Disconnected += OnClientOnDisconnected;
            _client.Log += OnClientOnLog;
            
            //Login
             _client.LoginAsync(TokenType.Bot, _token);
             _client.StartAsync();
        }

        private async Task OnClientOnLog(LogMessage l)
        {
            Debug.WriteLine(l.Message);
        }

        private async Task OnClientOnDisconnected(Exception e)
        {
            _isReady = false;
            Debug.WriteLine("Bot Disconnected");
        }

        private async Task OnClientOnConnected()
        {
            Debug.WriteLine("Bot Connected");
        }

        private async Task OnClientOnReady()
        {
            _isReady = true;
        }

     
    }
}
