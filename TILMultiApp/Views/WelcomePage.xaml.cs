using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TILMultiApp
{
    /// <summary>
    /// Welcome page class.
    /// </summary>
    public partial class WelcomePage : ContentPage
    {    
        
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:TILMultiApp.WelcomePage"/> class.
        /// </summary>
        public WelcomePage()
        {
            InitializeComponent();
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
        /// Chooses a post and goes to the TIL page.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event.</param>
        async void SomethingNewAsync(object sender, System.EventArgs e)
        {
            Post newPost = ((App)Application.Current).GetNextPost();
            await Navigation.PushAsync(new TILPage(newPost));
        }
    }
}
