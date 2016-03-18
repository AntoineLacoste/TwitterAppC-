using System;
using System.IO;
using Windows.Storage;
using Newtonsoft.Json;

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
