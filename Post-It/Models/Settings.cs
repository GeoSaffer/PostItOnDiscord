using System.Collections.Generic;

namespace Post_It.Models
{
    public class Settings
    {
        public string ImageFolderLocation = "";
        public List<DiscordChannel> DiscordChannels { get; set; }
    }
}
