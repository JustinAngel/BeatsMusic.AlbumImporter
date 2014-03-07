using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace BeatsMusic.Importer.Beats
{
    public class BeatsClient
    {
        public const string BeatsClientId = "<BeatsMusic API client ID here>";
        public const string BeatsClientSecret = "<BeatsMusic API client secret here>";
        public const string BeatsCallbackDomain = "<BeatsMusic API redirect uri here>";

        // todo: clean up importing so you can import into the currently logged in user
        private const string BeatsMusicUserIdToImportInto = "<BeatsMusic user ID to import into>";

        public static Uri GetDefaultAlbumImageUri(string albumId)
        {
            const string ImageAlbumsFetchDefault = @"https://partner.api.beatsmusic.com/v1/api/albums/{0}/images/default?size={1}&client_id={2}";
            return new Uri(String.Format(ImageAlbumsFetchDefault, albumId, "small", BeatsClientId));
        }

        public Uri GetAuthorizationUrl(string returnUrl)
        {
            return new Uri(string.Format(@"https://partner.api.beatsmusic.com/oauth2/authorize?response_type=code&redirect_uri=http%3A%2F%2F{0}&client_id={1}",
                returnUrl,
                BeatsClientId));
        }


        public async Task<List<SearchResult>> SearchStreamableAlbums(string albumName, string artistName)
        {
            const string AlbumSearchUri = @"https://partner.api.beatsmusic.com/v1/api/search?q={0}&type=album&client_id={1}&filters=streamable:true";
            var result =
                await GetWebRequest<SearchResultDataRoot>(
                new Uri(string.Format(AlbumSearchUri, HttpUtility.UrlEncode(albumName + " " + artistName), BeatsClientId)));

            if (result == null || result.ResponseCode.ToLower() != "ok")
            {
                return null;
            }

            return result.Data.ToList();
        }


        public void BulkAddToCollection(string[] ids)
        {
            // Todo: clean up harcoding of user_id
            const string BulkAddUri = @"https://partner.api.beatsmusic.com/v1/api/users/" + BeatsMusicUserIdToImportInto + "/mymusic?access_token={0}&ids={1}";
            PostWebRequest(new Uri(string.Format(BulkAddUri, App.BeatsAccessToken, string.Join("&ids=", ids))), string.Empty);
        }

        public async Task<string> GetAccesstoken()
        {
            const string requestTokenUri = @"https://partner.api.beatsmusic.com/oauth2/token";
            var nvc = new KeyValuePair<string, string>[]
                      {
                          new KeyValuePair<string, string>("client_secret", BeatsClientSecret),
                          new KeyValuePair<string, string>("client_id", BeatsClientId),
                          new KeyValuePair<string, string>("redirect_uri", "http://" + BeatsCallbackDomain),
                          new KeyValuePair<string, string>("code", App.BeatsCode)
                      };
            var result = await PostKeyValuePairWebRequestWithReturnType<RequestTokenRootElement>(
                new Uri(requestTokenUri), nvc);

            if (result.Code.ToLower() == "ok")
            {
                return result.Data.AccessToken;
            }
            else
            {
                return null;
            }
        }

        private async void PostWebRequest(Uri uri, string PostData)
        {
            bool retry = false;
            try
            {
                WebClient wc = new WebClient();
                var result = await wc.UploadStringTaskAsync(uri, "POST", PostData);

            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("403"))
                {
                    Debugger.Break();
                }
                else
                {
                    retry = true;
                    Debug.WriteLine("403: " + ++FourOThreeCounter);
                }
            }

            if (retry)
                PostWebRequest(uri, PostData);

        }

        private async Task<T> PostKeyValuePairWebRequestWithReturnType<T>(Uri uri, KeyValuePair<string, string>[] data)
            where T: class
        {
            try
            {
                var client = new HttpClient();

                HttpResponseMessage response = await client.PostAsync(
                    uri,
                    new FormUrlEncodedContent(data));

                HttpContent responseContent = response.Content;

                using (var sr = new StreamReader(await responseContent.ReadAsStreamAsync()))
                using (var json = new JsonTextReader(sr))
                {
                    var instance = new JsonSerializer().Deserialize<T>(json);
                    return instance;
                }
            }

            catch (Exception ex)
            {
                if (!ex.Message.Contains("403"))
                {
                    Debugger.Break();
                }
                else
                {
                    Debugger.Break();
                }
            }
            return null;
        }

        private async Task<T> GetWebRequest<T>(Uri uri)
            where T : class
        {
            bool retry = false;
            try
            {
                WebClient wc = new WebClient();
                var result = await wc.DownloadStringTaskAsync(uri);

                using (var s = new MemoryStream(Encoding.UTF8.GetBytes(result)))
                using (var sr = new StreamReader(s))
                using (var json = new JsonTextReader(sr))
                {
                    var instance = new JsonSerializer().Deserialize<T>(json);
                    return instance;
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("403"))
                {
                    Debugger.Break();
                }
                else
                {
                    retry = true;
                    Debug.WriteLine("403: " + ++FourOThreeCounter);
                }
            }

            if (retry)
                return await GetWebRequest<T>(uri);
            else
                return null;
        }
        private decimal FourOThreeCounter;
    }
}
