using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TILMultiApp
{
    /// <summary>
    /// A static class to handel option menu dialogs.
    /// </summary>
    public static class OpenMenuCl
    {
        /// <summary>
        /// Open an options menu alert dialog. The dialog varies according to 
        /// the page calling it.
        /// </summary>
        /// <returns>Whether a dialog has been opened.</returns>
        /// <param name="p">Content page.</param>
        public static async Task<bool> OpenAsync(ContentPage p)
        {
            string[] input = {
                "Show All Facts",
                    "About This App",
                    "About Background Picture"};

            if (p.GetType() == typeof(WelcomePage) 
                || p.GetType() == typeof(TILPage))
            {
                input = new string[] {"Show All Facts",
                    "About This App",
                    "About Background Picture"};
            }
            else if (p.GetType() == typeof(ListPage))
            {
                input = new string[] {"About This App",
                    "About Background Picture"};
            }
            else if (p.GetType() == typeof(InfoPage))
            {
                input = new string[] {"Show All Facts",
                    "About Background Picture"};
            }
            else
                input = new string[] {""};

            var response = await p.DisplayActionSheet("Options Menu",
                                                    "Close Menu",
                                                    null,
                                                    input);
            if (response == null)
                return false;

            if (response.Equals("About This App"))
            {                
                await p.Navigation.PushAsync(new InfoPage());
                return true;
            }
            if (response.Equals("Show All Facts"))
            {
                await p.Navigation.PushAsync(new ListPage());
                return true;
            }
            if (response.Equals("About Background Picture"))
            {
                Device.OpenUri(new Uri("https://en.wikipedia.org/wiki/The_School_of_Athens"));
                return true;
            }

            return false;
        }
    }
}
