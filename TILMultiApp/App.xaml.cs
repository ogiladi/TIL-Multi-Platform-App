using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TILMultiApp
{
    /// <summary>
    /// App class.
    /// </summary>
    public partial class App : Application
    {
        public List<Post> AppPostList;
        public const int NUM_PAGES = 20;
        int ind;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:TILMultiApp.App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();
            AppPostList = null;
            ind = 0;
            MainPage = new LoadingPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        /// <summary>
        /// Gets the next post on the posts list.
        /// </summary>
        /// <returns>The next post.</returns>
        public Post GetNextPost()
        {
            if (ind >= AppPostList.Count) return null;
            ind = (ind + 1) % AppPostList.Count;
            return AppPostList[ind];
        }
    }
}
