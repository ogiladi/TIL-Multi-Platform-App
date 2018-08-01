using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Collections.Specialized;

namespace TILMultiApp
{
    /// <summary>
    /// Loading page class. Presents a progress bar while the app connects to 
    /// Reddit's API and retrieves data.
    /// </summary>
    public partial class LoadingPage : ContentPage
    {
        string accessToken;
        /// <summary>
        /// An App Id which is obtained when registering the app on Reddit.
        /// </summary>
        string appId = ENTER_ID_HERE;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:TILMultiApp.LoadingPage"/> class.
        /// </summary>
        public LoadingPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Connects to Reddit's API and retrives data when the page appears.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            accessToken = await GetTokenAsync();
            List<Post> list = new List<Post>();

            progressBar.Progress = 0.25;
            progressBar.IsVisible = true;

            var success = await AddMultiPagesAsync(list, App.NUM_PAGES);

            if (success)
            {
                Shuffle(ref list);
                ((App)Application.Current).AppPostList = list;
                Application.Current.MainPage = new NavigationPage(new WelcomePage())
                {
                    BarBackgroundColor = (Color)Application.Current.Resources["backToolbar"],
                    BarTextColor = Color.White,
                };
            }
            else
            {
                progressBar.IsVisible = false;
                await DisplayAlert("Error", "Something went wrong",
                                            "Try Again");
                Application.Current.MainPage = new LoadingPage();
            }
        }

        /// <summary>
        /// Prevents the user from interrupting the download process.
        /// </summary>
        /// <returns><c>true</c>, if back button pressed, 
        /// <c>false</c> otherwise.</returns>
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        /// <summary>
        /// Gets the token string for Reddit's OAuth protocol.
        /// </summary>
        /// <returns>The token async.</returns>
        async Task<string> GetTokenAsync()
        {
            string token = null;
            WebClient client = new WebClient();
            client.UploadValuesCompleted += (s, e) =>
            {
                try
                {
                    var responseString = Encoding.Default.GetString(e.Result);
                    var response = 
                        JsonConvert.DeserializeObject<dynamic>(responseString);
                    token = response.access_token.ToString();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error while uploading values: " + ex.Message);
                    return;
                }
            };

            try
            {                                
                var header = 
                    Convert.ToBase64String(Encoding.Default.GetBytes(appId + ":"));
                client.Headers.Add("Authorization", "Basic " + header);
                var postData = new NameValueCollection
                {
                    {"grant_type", "https://oauth.reddit.com/grants/installed_client"},
                    {"device_id", "DO_NOT_TRACK_THIS_DEVICE"},
                };
                var redditUri = new Uri("https://www.reddit.com/api/v1/access_token");
                await client.UploadValuesTaskAsync(redditUri, "POST", postData);

                return token;
            }
            catch (Exception e)
            {
                Console.WriteLine("GetToken error: " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieve data from one page of Reddit. Adds the relevent data to
        /// a list of posts.
        /// </summary>
        /// <returns>Url suffix needed to connect to the next page.</returns>
        /// <param name="after">Url suffix of the current page.</param>
        /// <param name="list">List of posts.</param>
        /// <param name="currPage">Index of current page.</param>
        async Task<string> AddOnePageAsync(string after, List<Post> list,
                                     int currPage)
        {
            if (after == null)
                return null;

            string newAfter = null;
            var client = new WebClient();

            client.DownloadStringCompleted += (s, e) =>
            {
                try
                {
                    JObject jObj = JObject.Parse(e.Result);
                    var data = jObj["data"];
                    foreach (var child in data["children"])
                    {
                        var title = child["data"]["title"].ToString();
                        var permalink = child["data"]["permalink"].ToString();
                        var link = child["data"]["url"].ToString();
                        var numComments = (int)child["data"]["num_comments"];
                        Post post = new Post(title, permalink, link, numComments);
                        if (post.IsGood())
                            list.Add(post);
                    }
                    newAfter = data["after"].ToString();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error while downloadin data: " + ex.Message);
                    return;
                }
            };

            client.DownloadProgressChanged += (s, e) =>
            {
                var currBytes = e.BytesReceived;
                var totalBytes = e.TotalBytesToReceive;
                progressBar.Progress = 0.25 + (currBytes + currPage * totalBytes) * 0.75 /
                               (App.NUM_PAGES * totalBytes);
            };

            try
            {
                client.Headers.Add("Authorization", "Bearer " + accessToken);
                client.Headers.Add("user-agent", "None");
                var uri = new Uri("https://oauth.reddit.com/r/todayilearned/?after=" + after);
                await client.DownloadStringTaskAsync(uri);
                return newAfter;
            }
            catch (Exception e)
            {
                Console.WriteLine("AddOnePage error: " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Add multiple pages to the class's list of posts.
        /// </summary>
        /// <returns><c>true</c> if all pages have been added 
        /// successfully, <c>false</c> otherwise.</returns>
        /// <param name="list">List of posts.</param>
        /// <param name="numPages">Total number of pages.</param>
        async Task<bool> AddMultiPagesAsync(List<Post> list, int numPages)
        {
            var after = "";
            try
            {
                for (int ind = 0; ind < numPages; ind++)
                {
                    after = await AddOnePageAsync(after, list, ind);
                    if (after == null)
                        return false;
                }
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("AddMultiPages error: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Randomly shuffle a list. 
        /// </summary>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        void Shuffle<T>(ref List<T> list)
        {
            var random = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                var temp = list[k];
                list[k] = list[n];
                list[n] = temp;
            }
        }
    }
}
