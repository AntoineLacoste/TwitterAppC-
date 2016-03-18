namespace TwitterUniversalApp.Models
{
    class TwitterConnectionInfoSingleton
    {
        private static TwitterConnectionInfo instance;

        public  static TwitterConnectionInfo getInstance()
        {
            if (instance == null)
            {
                instance = new TwitterConnectionInfo();
            }

            return instance;
        }
    }
}
