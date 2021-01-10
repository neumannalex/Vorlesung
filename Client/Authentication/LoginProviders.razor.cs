using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Authentication
{
    public partial class LoginProviders : ComponentBase
    {
        public List<Provider> Providers = new List<Provider>
        {
            new Provider { Title = "Google", Name = "google", Icon = "google", Route = "google" },
            new Provider { Title = "Facebook", Name = "facebook", Icon = "facebook", Route = "facebook" },
            new Provider { Title = "Twitter", Name = "twitter", Icon = "twitter", Route = "twitter" },
            new Provider { Title = "Github", Name = "github", Icon = "github", Route = "github" },
            new Provider { Title = "Azure", Name = "microsoft", Icon = "windows", Route = "aad" }
        };
    }

    public class Provider
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
    }
}
