using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TILMultiApp
{
    /// <summary>
    /// Info page class.
    /// </summary>
    public partial class InfoPage : ContentPage
    {
        /// <summary>
        /// Opens the developer's webpage on browser.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event.</param>
        void GoToDevSite(object sender, System.EventArgs e)
        {
            Device.OpenUri(new Uri("http://www.ohadgiladi.com"));
        }

        /// <summary>
        /// Opens the developer's GitHub page on browser.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event.</param>
        void GoToGitHub(object sender, System.EventArgs e)
        {
            Device.OpenUri(new Uri("https://github.com/ogiladi"));
        }

        /// <summary>
        /// Opens the options menu.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event.</param>
        async void OpenMenuAsync(object sender, System.EventArgs e)
        {
            var success = await OpenMenuCl.OpenAsync(this);
            if (!success)
                return;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:TILMultiApp.InfoPage"/> class.
        /// </summary>
        public InfoPage()
        {
            InitializeComponent();
            content.Text = "This app takes the first 500 posts from Reddit's" +
                "Today I Learned (TIL) subreddit and randomly " +
                "chooses a post with at least 10 comments";
        }
    }
}
