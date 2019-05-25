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

        private async Task<IKrakenResponse<T>> RequestPublic<T>(string query)
        {
            var response = await _pubRestClient.GetStringAsync(query);
            var result = JsonConvert.DeserializeObject<KrakenResponse<T>>(response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }
            );

            return result;
        }

        public async Task<IKrakenResponse<ServerTime>> GetServerTime()
        {
            var query = "Time";
            return await RequestPublic<ServerTime>(query);
        }

        public async Task<IKrakenResponse<Dictionary<string, AssetInfo>>> GetAssetInfo()
        {
            var query = "Assets";
            return await RequestPublic<Dictionary<string, AssetInfo>>(query);
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
            return await RequestPublic<Dictionary<string, AssetPair>>(query);
        }

        public async Task<IKrakenResponse<TickerData>> GetTicker(params string[] pairs)
        {
            var query = $"Ticker?pair={String.Join(",",pairs)}";
            return await RequestPublic<TickerData>(query);
        }

        public async Task<IKrakenResponse<OHLCData>> GetOHLCData(string pair, long? interval = null, int? since = null)
        {
            var query = $"OHLC?pair={pair}";
            if (interval != null) query += $"&interval={interval.Value}";
            if (since != null) query += $"&since={since.Value}";
            return await RequestPublic<OHLCData>(query);
        }

        public async Task<IKrakenResponse<OrderBookData>> GetOrderBook(string pair, int? count = null)
        {
            var query = $"Depth?pair={pair}";
            if (count.HasValue) query += $"&count={count}";
            return await RequestPublic<OrderBookData>(query);
        }

        public async Task<IKrakenResponse<TradeData>> GetRecentTrades(string pair, decimal? since = null)
        {
            var query = $"Trades?pair={pair}";
            if (since.HasValue) query += $"&since={since}";
            return await RequestPublic<TradeData>(query);
        }

        public async Task<IKrakenResponse<SpreadData>> GetRecentSpreadData(string pair, decimal? since = null)
        {
            var query = $"Spread?pair={pair}";
            if (since.HasValue) query += $"&since={since}";
            return await RequestPublic<SpreadData>(query);
        }

        public void Dispose()
        {
            _pubRestClient.Dispose();
        }
    }

}
