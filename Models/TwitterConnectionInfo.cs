using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Credentials;

namespace TwitterUniversalApp.Models
{
    class TwitterConnectionInfo
    {
        private readonly string consumerKey;
        private readonly string consumerSecret;
        private TwitterCredentials appCredential;

        public TwitterConnectionInfo()
        {
                this.consumerKey = "zTFaSvnBjjYxngDGB6eAGWQeP";
                this.consumerSecret = "5cAS9Qz4Xx3O2b17puO9wEjZpolSE0jhLkGyaMYqDhRkmYxXLJ";
                this.appCredential = new TwitterCredentials(this.consumerKey, this.consumerSecret);
        }
        public TwitterCredentials GetAppCredentials()
        {
            return this.appCredential;
        }
    }
}
