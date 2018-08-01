using System.Collections.Generic;
using System.Text;

namespace TILMultiApp
{
    /// <summary>
    /// A class that represents a Reddit post. Includes the post title, 
    /// link to Reddit thread (permalink), link to a webpage, and the number 
    /// of comments.
    /// </summary>
    public class Post
    {
        string permalink;
        int NumComments;

        public string Permalink 
        { 
            get { return "http://www.reddit.com" + permalink; } 
            private set { permalink = value; }
        }
        public string Title { get; }
        public string Link { get; }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:TILMultiApp.Post"/> class.
        /// </summary>
        /// <param name="title">Post Title.</param>
        /// <param name="permalink">Post Permalink.</param>
        /// <param name="link">Post Link.</param>
        /// <param name="numComments">Number comments of post.</param>
        public Post(string title, string permalink, 
                    string link, int numComments)
        {
            Title = GetCleanTitle(title);
            Permalink = permalink;
            Link = link;
            NumComments = numComments;
        }

        /// <summary>
        /// Determines whether a post has a certain number of comments and 
        /// whether the title is according to Reddit's rules.
        /// </summary>
        /// <returns><c>true</c>, if the Reddit post has enough comments and 
        /// a good title, <c>false</c> otherwise.</returns>
        public bool IsGood() 
        {
            HashSet<string> badStart = 
                new HashSet<string> { "of", "about", "how", "what" };
            if (NumComments < 10) return false;
            if (Title.Length == 0) return false;
            string firstWordLower = GetFirstWord(Title).ToLower();
            if (badStart.Contains(firstWordLower)) return false;
            return true;
        }

        /// <summary>
        /// Capitalize the specified word.
        /// </summary>
        /// <returns>The capitalized word.</returns>
        /// <param name="word">Word.</param>
        string Capitalize(string word)
        {
            if (word.Length <= 1) return word.ToUpper();
            if (!char.IsLetter(word[0]) && word.Length >1)
                return word.Substring(0, 2).ToUpper()
                       + word.Substring(2).ToLower();
            return word.Substring(0, 1).ToUpper() 
                       + word.Substring(1).ToLower();
        }

        /// <summary>
        /// Clean the title of the reddit post. 
        /// </summary>
        /// <returns>The clean title.</returns>
        /// <param name="str">String.</param>
        string GetCleanTitle(string str)
        {
            HashSet<char> charsSet = new HashSet<char> {',', '.',
                '-', ':', ';', ' ', '\t'};
            HashSet<string> thatSet = new HashSet<string> { "that", "that,", 
                "that:", "that;" };
            HashSet<string> qmSet = new HashSet<string> { "\"", "\'" };


            var startInd = 0;
            /// If a title begins whith 'TIL', remove it.
            if (str.Substring(0, 3).ToUpper().Equals("TIL")) startInd = 3;
            /// Remove any commas, semicolons, etc. after the 'TIL'.
            while (startInd < str.Length 
                   && charsSet.Contains(str[startInd])) 
            {
                startInd++;
            }

            List<string> strList = 
                new List<string>(str.Substring(startInd).Split(null));

            /// If the title starts with 'Today I Learned', remove those words.
            if (strList[0].ToLower().Equals("today") &&
                strList[1].ToLower().Equals("i") &&
                strList[2].ToLower().Equals("learned"))
            {
                strList.RemoveAt(0);
                strList.RemoveAt(0);
                strList.RemoveAt(0);
            }

            /// If the title starts with 'I Learned', remove those words.
            if (strList[0].ToLower().Equals("i") &&
                strList[1].ToLower().Equals("learned"))
            {
                strList.RemoveAt(0);
                strList.RemoveAt(0);
            }

            /// Remove 'that' from the beginning of the title.
            if (thatSet.Contains(strList[0].ToLower())) strList.RemoveAt(0);

            /// Capitalize the first word.
            strList[0] = Capitalize(strList[0]);

            /// Remove the period from the last word.
            var lastWord = strList[strList.Count - 1];
            if (lastWord[lastWord.Length-1] == '.') 
            {
                strList[strList.Count - 1] 
                = lastWord.Substring(0, lastWord.Length - 1);
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach(var word in strList)
            {
                stringBuilder.Append(System.Net.WebUtility.HtmlDecode(word));
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString().Trim();
        }

        /// <summary>
        /// Gets the first word in a sentence.
        /// </summary>
        /// <returns>The first word.</returns>
        /// <param name="sentence">Sentence.</param>
        string GetFirstWord(string sentence)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int ind = 0;
            while (ind < sentence.Length && sentence[ind] != ' ')
            {
                stringBuilder.Append(sentence[ind++]);
            }
            return stringBuilder.ToString();
        }
    }
}
