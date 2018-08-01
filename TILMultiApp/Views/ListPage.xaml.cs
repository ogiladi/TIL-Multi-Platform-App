using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TILMultiApp
{
    /// <summary>
    /// List page class -- a page with all the posts presented in a list.
    /// </summary>
    public partial class ListPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:TILMultiApp.ListPage"/> class.
        /// </summary>
        public ListPage()
        {
            InitializeComponent();

            listView.ItemsSource = ((App)Application.Current).AppPostList;
        }

        /// <summary>
        /// Opens an options menu.
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
        /// Handels the event when an item is tapped
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event.</param>
        async void ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var post = e.Item as Post;
            await Navigation.PushAsync(new TILPage(post));
        }
    }
}
