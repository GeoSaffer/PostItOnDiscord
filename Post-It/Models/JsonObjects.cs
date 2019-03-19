using System.Collections.Generic;
using System.Linq;

namespace Post_It.Models
{
    public class JsonObjects
    {
        public class Empty
        {
            public Settings Settings()
            {
                return new Settings()
                {
                    ImageFolderLocation = "",
                    DiscordChannels = new List<DiscordChannel>()
                    {
                        new DiscordChannel(){ Name = "", Webhook = "" },
                        new DiscordChannel(){ Name = "", Webhook = "" }
                    }
                };
            }
        }
    }
}
