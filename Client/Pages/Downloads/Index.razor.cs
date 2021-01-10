using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Vorlesung.Shared.Downloads;

namespace Vorlesung.Client.Pages.Downloads
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private string _host
        {
            get
            {
                if (NavigationManager.BaseUri.Contains("localhost"))
                    return "http://localhost:7071";
                else
                    return string.Empty;
            }
        }

        public List<Download> Downloads { get; set; } = new List<Download>();

        public bool IsLoading { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await GetDownloads();
        }

        public async Task Refresh()
        {
            await GetDownloads();
        }

        private async Task GetDownloads()
        {
            try
            {
                IsLoading = true;
                StateHasChanged();

                Downloads = await HttpClient.GetFromJsonAsync<List<Download>>("api/downloads");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Downloads = new List<Download>();
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private async Task DeleteDownload(string id)
        {
            var response = await HttpClient.DeleteAsync($"api/downloads/{id}");

            await Refresh();
        }
    }
}
