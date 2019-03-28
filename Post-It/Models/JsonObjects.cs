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
                    ServerName = string.Empty,
                    BotToken = string.Empty
                };
            }
        }
    }
}
