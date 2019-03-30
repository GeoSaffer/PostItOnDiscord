using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Discord.WebSocket;
using Embed = Discord.Embed;
using Uri = System.Uri;
using Post_It.Models;
using System.IO;
using Post_It.Utils;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using Discord;
using Post_It.Bot;
using Post_It.Dialogs;
using Color = Discord.Color;

namespace Post_It
{
    public partial class MainWindow : Window
    {
        private string _selectedImagePath;
        private string _fileName;
        private readonly DiscordSocketClient _client;
        private string _serverName;
        private string _botToken;
        private SocketGuild _guild;
        private IReadOnlyCollection<SocketTextChannel> _channels;
        private SocketTextChannel _selectedChannel;
        
        public MainWindow()
        {
            InitializeComponent();
            GetSettings();
            //start bot
            var bot = new DiscordBot(_botToken);
            bot.Run();
            _client = bot.Client;
            _client.Ready += OnClientOnReady;
        }

        #region Onload

        private Task OnClientOnReady()
        {
            //get guild and channels
            _guild = _client.Guilds.Single(g => g.Name == _serverName);
            _channels = _guild.TextChannels;

            PopulateChannelDropdown();
            
            return Task.CompletedTask;
        }

        private void GetSettings()
        {
            try
            {
                var settingsFile = myFiles.Json.Settings;
                if (!File.Exists(settingsFile))
                {
                    JsonFile.Create<Settings>(settingsFile, new JsonObjects.Empty().Settings());
                }

                if (File.Exists(settingsFile))
                {
                    var settings = JsonFile.Load<Settings>(settingsFile);
                   _serverName = settings.ServerName;
                   _botToken = settings.BotToken;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                DumpFile.Create(e);
                throw;
            }
        }
       
        private void PopulateChannelDropdown()
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    var channelItems = _channels
                        .Select(s => new ComboBoxItem
                        {
                            Content = (s.Category == null ? $"{s.Name} " : $"({s.Category?.Name}) {s.Name} "), Tag = s.Id
                        })
                        .OrderBy(o => o.Content).ToList();

                    channelItems.ForEach(item => cmbChannelPicker.Items.Add(item));
                }, DispatcherPriority.ContextIdle);
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                DumpFile.Create(e);
                throw;
            }
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        #region EmbedBuilder

        private void UseTitle(EmbedBuilder myEmbed)
        {
            myEmbed.Title = TxtTitle.Text;
        }

        private void UseMessage(EmbedBuilder myEmbed)
        {
            myEmbed.Description = TxtMessage.Text;
        }

        private void UseThumbnail(EmbedBuilder myEmbed)
        {
            myEmbed.ThumbnailUrl = $"attachment://{_fileName}";
        }

        private Embed CheckWhatToSend()
        {
            //create new Embed
            var myEmbed = new EmbedBuilder();

            //check if we should add a title
            if (!string.IsNullOrEmpty(TxtTitle.Text))
                UseTitle(myEmbed);

            //check if we should add a message
            if (!string.IsNullOrEmpty(TxtMessage.Text))
                UseMessage(myEmbed);
            
            //check if we should add a thumbnail
            if (_selectedImagePath != null)
                UseThumbnail(myEmbed);

            //assign the color for the Embed
            myEmbed.Color = new Color(SelectedColour.Fill.ToString().FromHexString());

            // add footer
            myEmbed.Footer = new EmbedFooterBuilder()
            {
                Text = "....created by 'Post It on Discord'"
            };
                
            //build and return embed
            return myEmbed.Build();
        }

        private Embed CheckWhatToSend_ThreadSafe()
        {
            return Dispatcher.Invoke(CheckWhatToSend);
        }

        #endregion

        private void SendMessage()
        {
            if (_selectedChannel != null)
            {
                try
                {
                    PleaseWait(() =>
                    {
                        var embed = CheckWhatToSend_ThreadSafe();

                        if (embed.Thumbnail == null)
                        {
                            _selectedChannel.SendMessageAsync(null, false, embed);
                        }
                        else
                        {
                            _selectedChannel.SendFileAsync(_selectedImagePath, string.Empty, false, embed,
                                RequestOptions.Default);
                        }
                    });
                    ShowMessage(this, "Message sent to Discord.");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    DumpFile.Create(e);
                    throw;
                }
            }
            else
            {
                ShowMessage(this, "Please select a channel to continue.", "Select Channel", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #region colour button events

        private void BtnRed_Click(object sender, RoutedEventArgs e)
        {
            SelectedColour.Fill = new SolidColorBrush(Colors.Red);
        }

        private void BtnBlue_Click(object sender, RoutedEventArgs e)
        {
            SelectedColour.Fill = new SolidColorBrush(Colors.Blue);
        }

        private void BtnGreen_Click(object sender, RoutedEventArgs e)
        {
            SelectedColour.Fill = new SolidColorBrush(Colors.Green);
        }

        private void BtnYellow_Click(object sender, RoutedEventArgs e)
        {
            SelectedColour.Fill = new SolidColorBrush(Colors.Yellow);
        }

        private void BtnWhite_Click(object sender, RoutedEventArgs e)
        {
            SelectedColour.Fill = new SolidColorBrush(Colors.White);
        }

        private void BtnBlack_Click(object sender, RoutedEventArgs e)
        {
            SelectedColour.Fill = new SolidColorBrush(Colors.Black);
        }

        #endregion

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: if important...... use impersonation to get network drives visibility
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                // Set filter for file extension and default file extension 
                DefaultExt = ".png",
                Filter = "Image Files |*.jpeg; *.jpg; *.png; *.gif; *.bmp"
            };

            // Display OpenFileDialog by calling ShowDialog method 
            var result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                _fileName = CacheFile.Create(dlg.FileName);
                _selectedImagePath = Path.GetFullPath($@"Cache\{_fileName}");
                EmbedImage.Source = new BitmapImage(new Uri(_selectedImagePath));
            }
        }

        private void CmbChanelPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tag = ((ComboBoxItem) cmbChannelPicker.SelectedItem)?.Tag;
            if (tag == null) return;
            var channelId = (ulong) tag;
            _selectedChannel = _channels.Single(c => c.Id == channelId);
        }

        public void PleaseWait(Action action)
        {
            var myEffect = new BlurEffect();
            using (var w = new WaitDialog(this.Width, action))
            {
                myEffect.Radius = 5;
                Opacity = 0.4;
                Effect = myEffect;
                w.Owner = this;
                w.ShowDialog();
                myEffect.Radius = 0;
                Effect = myEffect;
                Opacity = 1;
            }
        }

        public void PleaseWait(Action action, string message)
        {
            var myEffect = new BlurEffect();
            using (var w = new WaitDialog(this.Width, action, message))
            {
                myEffect.Radius = 5;
                Opacity = 0.4;
                Effect = myEffect;
                w.Owner = this;
                w.ShowDialog();
                myEffect.Radius = 0;
                Effect = myEffect;
                Opacity = 1;
            }
        }

        private void ShowMessage(Window owner, string message)
        {
            Dispatcher.Invoke(new Action<Window, string>((o, m) => 
                MessageBox.Show(o, m))
                , new object[] { owner, message });
        }

        private void ShowMessage(Window owner, string message,string title,MessageBoxButton button, MessageBoxImage image)
        {
            Dispatcher.Invoke(new Action<Window, string, string, MessageBoxButton, MessageBoxImage>((o, m, t, b, i) => 
                MessageBox.Show(o, m, t, b, i)), new object[] { owner, message, title,  button, image });
        }

    }
}
