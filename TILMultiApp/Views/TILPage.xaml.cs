using System;
using Xamarin.Forms;

namespace TILMultiApp
{
    /// <summary>
    /// A page the presents data from a given post.
    /// </summary>
    public partial class TILPage : ContentPage
    {
        Post currPost;
        Easing easing = Easing.Linear;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:TILMultiApp.TILPage"/> class.
        /// </summary>
        /// <param name="post">Post.</param>
        public TILPage(Post post)
        {
            InitializeComponent();
            currPost = post;
            content.Text = currPost.Title;
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
        /// Opens a new TIL page with another post.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event.</param>
        async void SomethingElseAsync(object sender, System.EventArgs e)
        {
            Post newPost = ((App)Application.Current).GetNextPost();
            await Navigation.PushAsync(new TILPage(newPost));
        }

        /// <summary>
        /// Goes to the Reddit thread.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void GoToReddit(object sender, System.EventArgs e)
        {
            Device.OpenUri(new Uri(currPost.Permalink));
        }

        /// <summary>
        /// Goes to link in that Reddit post.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event.</param>
        void GoToLink(object sender, System.EventArgs e)
        {
            Device.OpenUri(new Uri(currPost.Link));
        }
    }
}
