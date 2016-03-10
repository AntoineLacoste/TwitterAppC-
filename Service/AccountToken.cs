using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwitterUniversalApp.Service
{
    class AccountToken
    {
        public static async void SaveAccountData(Token tokens)
        {
            var folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync("config.json",CreationCollisionOption.ReplaceExisting);
            string tokensJson = JsonConvert.SerializeObject(tokens);
            await Windows.Storage.FileIO.WriteTextAsync(file,tokensJson);
        }

        public static Token ReadTokens()
        {
            var tokens = JsonConvert.DeserializeObject<Token>(File.ReadAllText(ApplicationData.Current.LocalFolder.Path + "\\config.json"));

            return tokens;
        }
    }
}
