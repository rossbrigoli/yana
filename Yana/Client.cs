using System;
using System.Collections.Generic;
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

        public async Task<IKrakenResponse<ServerTime>> GetServerTime()
        {
            var response = await _pubRestClient.GetStringAsync("Time");
            var result = JsonConvert.DeserializeObject<KrakenResponse<ServerTime>>(response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }
            );

            return result;
        }

        public async Task<IKrakenResponse<Dictionary<string, AssetInfo>>> GetAssetInfo()
        {
            var response = await _pubRestClient.GetStringAsync($"Assets");
            var result = JsonConvert.DeserializeObject<KrakenResponse<Dictionary<string, AssetInfo>>>(response,
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
        public async Task<IKrakenResponse<Dictionary<string, AssetPair>>> GetAssetPairs(string info="info", string pair=null)
        {
            var query = $"AssetPairs?info={info}";
            if (pair != null) query += $"&pair={pair}";
            var response = await _pubRestClient.GetStringAsync(query);
            var result = JsonConvert.DeserializeObject<KrakenResponse<Dictionary<string, AssetPair>>>(response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto}
            );

            return result;
        }

        public async Task<IKrakenResponse<TickerData>> GetTicker(params string[] pairs)
        {
            var response = await _pubRestClient.GetStringAsync($"Ticker?pair={String.Join(",",pairs)}");
            var result = JsonConvert.DeserializeObject<KrakenResponse<TickerData>>(response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }
            );

            return result;
        }

        public async Task<IKrakenResponse<OHLCData>> GetOHLCData(string pair, long? interval = null, int? since = null)
        {
            var query = $"OHLC?pair={pair}";
            if (interval != null) query += $"&interval={interval.Value}";
            if (since != null) query += $"&since={since.Value}";
            var response = await _pubRestClient.GetStringAsync(query);
            var result = JsonConvert.DeserializeObject<KrakenResponse<OHLCData>>(response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }
            );
            return result;
        }

        public async Task<IKrakenResponse<OrderBookData>> GetOrderBook(string pair, int? count = null)
        {
            var query = $"Depth?pair={pair}";
            if (count.HasValue) query += $"&count={count}";
            var response = await _pubRestClient.GetStringAsync(query);
            var result = JsonConvert.DeserializeObject<KrakenResponse<OrderBookData>>(response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }
            );

            return result;
        }

        public async Task<IKrakenResponse<TradeData>> GetRecentTrades(string pair, decimal? since = null)
        {
            var query = $"Trades?pair={pair}";
            if (since.HasValue) query += $"&since={since}";
            var response = await _pubRestClient.GetStringAsync(query);
            var result = JsonConvert.DeserializeObject<KrakenResponse<TradeData>>(response,
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
