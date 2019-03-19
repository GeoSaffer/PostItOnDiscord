using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Discord.Commands;
using Discord.WebSocket;
using DiscordWebhook;
using Embed = DiscordWebhook.Embed;
using Uri = System.Uri;
using static Post_It.WebHooks.Extensions;
using Post_It.Models;
using System.IO;
using Post_It.Utils;
using System;

namespace Post_It
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Discord.Color Color { get; set; }
        private DiscordSocketClient _client;
        private CommandService _commands;
        private string _defaultImageUrl = "http://i.imgur.com/XLgutLX.png";

        private string imageFolderLocation;


        public MainWindow()
        {
            InitializeComponent();
            GetSettings();
        }

        private Settings GetSettings()
        {
            var settings = new Settings();
            try
            {
                var settingsFile = myFiles.Json.Settings;
                if (!File.Exists(settingsFile))
                {
                    JsonFile.Create<Settings>(settingsFile, new JsonObjects.Empty().Settings());
                }

                if (File.Exists(settingsFile))
                {
                    settings = JsonFile.Load<Settings>(settingsFile);
                    imageFolderLocation = settings.ImageFolderLocation;
                    PopulateChannelDropdown(settings);
                }
            }
            catch (Exception e)
            {
                
            }
            return settings;
        }

        private void PopulateChannelDropdown(Settings settings)
        {
            foreach (var channel in settings.DiscordChannels)
            {
                var item = new ComboBoxItem()
                {
                    Content = channel.Name,
                    Tag = channel.Webhook
                };
                cmbChannelPicker.Items.Add(item);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void UseTitle(Embed myEmbed)
        {
            myEmbed.Title = TxtTitle.Text;
        }

        private void UseMessage(Embed myEmbed)
        {
            myEmbed.Description = TxtMessage.Text;
        }

        private void UseThumbnail(Embed myEmbed)
        {
            var thumb = new DiscordWebhook.EmbedThumbnail();
            thumb.Url = EmbedImage.Source.ToString();
            myEmbed.Thumbnail = thumb;
        }

        private List<Embed> CheckWhatToSend()
        {
            //create new Embed
            var myEmbed = new Embed();

            //check if we cshould add a title
            if (!string.IsNullOrEmpty(TxtTitle.Text))
                UseTitle(myEmbed);

            //check if we should add a message
            if (!string.IsNullOrEmpty(TxtMessage.Text))
                UseMessage(myEmbed);

            //check if we should add a thumbnail
            if (EmbedImage.Source.ToString() != _defaultImageUrl)
                UseThumbnail(myEmbed);

            //assign the colour for the Embed
            myEmbed.Color = SelectedColour.Fill.ToString().FromHexString();

            //Add Embeds to list
            var embeds = new List<Embed>();
            embeds.Add(myEmbed);
            return embeds;
        }

        private async Task SendMessage()
        {
            var embeds = CheckWhatToSend();
            if (embeds.Count > 0)
            {
                var selectedChannelWebhook = ((ComboBoxItem)cmbChannelPicker.SelectedItem)?.Tag.ToString();
                if (!string.IsNullOrEmpty(selectedChannelWebhook))
                {
                    //create new webhook
                    var newHook = new Webhook($"{selectedChannelWebhook}");
                    newHook.Content = string.Empty;
                    newHook.Username = "Grumpy";
                    newHook.Embeds = embeds;
                    await newHook.Send();
                }
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

        private void CmbColorPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color = Discord.Color.Blue;
        }

        #endregion

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: use impersonation to get network drives visibility
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
                if (dlg.FileName.ToLower().Contains("http://") || dlg.FileName.ToLower().Contains("https://"))
                {
                    if (CheckImageFromUrlExist(dlg.FileName))
                    {
                        EmbedImage.Source = new BitmapImage(new Uri(dlg.FileName));
                    }
                }
                else
                {
                    var filename = dlg.SafeFileName;
                    var myWebPath = $"{imageFolderLocation}/{filename}";
                    if (CheckImageFromUrlExist(myWebPath))
                    {
                        EmbedImage.Source = new BitmapImage(new Uri(myWebPath));
                    }
                }
            }
        }
    }
}
