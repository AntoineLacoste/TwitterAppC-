namespace TwitterUniversalApp.Service
{
    class Token
    {
        public readonly string token;
        public readonly string tokenSecret;

        public Token(string token, string tokenSecret)
        {
            this.token = token;
            this.tokenSecret = tokenSecret;
        }
    }
}
