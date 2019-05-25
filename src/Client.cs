using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.rossbrigoli.Yana
{
    public class Client : IDisposable
    {
        private static readonly string _krakenPubAddress = "https://api.kraken.com/0/public/";
        private static readonly string _krakenPvtAddress = "https://api.kraken.com";
        private static readonly string _krakenPvtApi = "/0/private/";
        private string _apiKey;
        private string _apiSecret;
        private static object _nonceLock = new object();
        private readonly HMACSHA512 _sha512ApiSecret;
        private readonly SHA256 _sha256 = SHA256.Create();
        private HttpClient _pubRestClient;
        private HttpClient _pvtRestClient;

        public Client(string key = null, string secret = null)
        {
            _apiKey = key;
            _apiSecret = secret;
            _pubRestClient = new HttpClient();
            _pubRestClient.BaseAddress = new Uri(_krakenPubAddress);

            _pvtRestClient = new HttpClient();
            _pvtRestClient.BaseAddress = new Uri(_krakenPvtAddress);
            if (_apiKey != null && _apiSecret != null)
            {
                _pvtRestClient.DefaultRequestHeaders.Add("API-Key", _apiKey);
                _sha512ApiSecret = new HMACSHA512(Convert.FromBase64String(_apiSecret));
            }
        }

        private Func<long> GetNonce { get; set; } = () => 
        { 
            lock(_nonceLock) 
            { 
                return DateTime.UtcNow.Ticks;
            } 
        };

        private async Task<IKrakenResponse<T>> RequestPublic<T>(string urlQuery)
        {
            var response = await _pubRestClient.GetStringAsync(urlQuery);
            var result = JsonConvert.DeserializeObject<KrakenResponse<T>>(
                response,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }
            );

            return result;
        }

        private async Task<IKrakenResponse<T>> RequestPrivate<T>(string api, Dictionary<string, string> query = null)
        {
            var urlPath = _krakenPvtApi + api;

            var nonce = GetNonce().ToString(CultureInfo.InvariantCulture);
            if (query == null) 
            {
                query = new Dictionary<string, string>();
            }

            query.Add("nonce", nonce);

            var postData = string.Join("&", 
                query.Where(c => c.Value != null)
                     .Select(d => $"{d.Key}={WebUtility.UrlEncode(d.Value)}")
                    );

            byte[] urlBytes = Encoding.UTF8.GetBytes(urlPath);
            byte[] dataBytes = _sha256.ComputeHash(Encoding.UTF8.GetBytes(nonce + postData));

            var buffer = new byte[urlBytes.Length + dataBytes.Length];
            Buffer.BlockCopy(urlBytes, 0, buffer, 0, urlBytes.Length);
            Buffer.BlockCopy(dataBytes, 0, buffer, urlBytes.Length, dataBytes.Length);
            byte[] apiSignature = _sha512ApiSecret.ComputeHash(buffer);

            var request = new HttpRequestMessage(HttpMethod.Post, urlPath)
            {
                Content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded")
            };

            request.Headers.Add("API-Sign", Convert.ToBase64String(apiSignature));

            var response = await _pvtRestClient.SendAsync(request);
            var strResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<KrakenResponse<T>>(strResponse,
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

        public async Task<IKrakenResponse<TradeBalanceData>> GetTradeBalance(string assetClass = "currency", string asset = "ZUSD")
        {
            var query = new Dictionary<string, string>();
            query.Add("aclass", assetClass);
            query.Add("asset", asset);
            var result =  await RequestPrivate<TradeBalanceData>("TradeBalance", query);
            return result;

        }

        public void Dispose()
        {
            _pubRestClient.Dispose();
        }
    }

}
