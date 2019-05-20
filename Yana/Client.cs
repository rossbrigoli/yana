using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.rossbrigoli.Yana
{
    public class Client : IDisposable
    {
        private string _krakenPubAddress = "https://api.kraken.com/0/public/";
        private string _krakenPvtAddress = "https://api.kraken.com/0/private/";
        private string _apiKey;
        private string _apiSecret;
        private HttpClient _pubRestClient;
        private HttpClient _pvtRestClient;

        public Client(string key = null, string secret = null)
        {
            _apiKey = key;
            _apiSecret = secret;
            _pubRestClient = new HttpClient();
            _pubRestClient.BaseAddress = new Uri(_krakenPubAddress);
        }

        public TimeResponse GetServerTime()
        {
            return GetServerTimeAsync().Result;
        }

        public async Task<TimeResponse> GetServerTimeAsync()
        {
            var response = await _pubRestClient.GetStringAsync("Time");
            var result = JsonConvert.DeserializeObject<TimeResponse>(response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }
            );

            return result;
        }

        public AssetInfoResponse GetAssetInfo()
        {
            return GetAssetInfoAsync().Result;
        }

        public async Task<AssetInfoResponse> GetAssetInfoAsync()
        {
            var response = await _pubRestClient.GetStringAsync($"Assets");
            var result = JsonConvert.DeserializeObject<AssetInfoResponse>(response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto}
            );

            return result;
        }

        /// <summary>
        /// Get Asset Pair information
        /// </summary>
        /// <param name="info">Possible values are info, leverage, fees, margin
        /// info = all info (default)
        /// leverage = leverage info
        /// fees = fees schedule
        /// margin = margin info
        /// </param>
        /// <param name="pair">comma delimited list of asset pairs to get info on (optional.  default = all)</param>
        /// <returns></returns>
        public AssetPairResponse GetAssetPairs(string info="info", string pair=null)
        {
            return GetAssetPairsAsync(info, pair).Result;
        }

        /// <summary>
        /// Get Asset Pair information
        /// </summary>
        /// <param name="info">Possible values are info, leverage, fees, margin
        /// info = all info (default)
        /// leverage = leverage info
        /// fees = fees schedule
        /// margin = margin info
        /// </param>
        /// <param name="pair">comma delimited list of asset pairs to get info on (optional.  default = all)</param>
        /// <returns></returns>
        public async Task<AssetPairResponse> GetAssetPairsAsync(string info="info", string pair=null)
        {
            var query = $"AssetPairs?info={info}";
            if (pair != null) query += $"&pair={pair}";
            var response = await _pubRestClient.GetStringAsync(query);
            var result = JsonConvert.DeserializeObject<AssetPairResponse>(response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto}
            );

            return result;
        }

        public TickerResponse GetTicker(params string[] pairs)
        {
            return GetTickerAsync(pairs).Result;
        }

        public async Task<TickerResponse> GetTickerAsync(params string[] pairs)
        {
            var response = await _pubRestClient.GetStringAsync($"Ticker?pair={String.Join(",",pairs)}");
            var result = JsonConvert.DeserializeObject<TickerResponse>(response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }
            );

            //Console.WriteLine(JsonConvert.SerializeObject(result));
            return result;
        }

        public OHLCResponse GetOHLCData(string pair, long? interval = null, int? since = null)
        {
            return GetOHLCDataAsync(pair, interval, since).Result;
        }

        public async Task<OHLCResponse> GetOHLCDataAsync(string pair, long? interval = null, int? since = null)
        {
            var query = $"OHLC?pair={pair}";
            if (interval != null) query += $"&interval={interval.Value}";
            if (since != null) query += $"&since={since.Value}";
            var response = await _pubRestClient.GetStringAsync(query);
            var result = JsonConvert.DeserializeObject<OHLCResponse>(response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }
            );
            return result;
        }

        public void Dispose()
        {
            _pubRestClient.Dispose();
        }
    }

}
