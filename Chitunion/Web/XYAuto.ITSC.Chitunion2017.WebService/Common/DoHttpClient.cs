/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 12:58:59
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.WebService.Common
{
    public class DoHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;

        public DoHttpClient()
        {
            _httpClient = _httpClient ?? new System.Net.Http.HttpClient();
            _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
        }

        public DoHttpClient(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
        }

        private void Init()
        {
            //_httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) };

            //_httpClient.BaseAddress = new Uri();

            ////帮HttpClient热身
            //_httpClient.SendAsync(new HttpRequestMessage
            //{
            //    Method = new HttpMethod("HEAD"),
            //    RequestUri = new Uri(BASE_ADDRESS + "/")
            //})
            //    .Result.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// httpClient post by json
        /// </summary>
        /// <param name="url">请求的ur</param>
        /// <param name="jsonParams">body json 参数</param>
        /// <returns></returns>
        public async Task<string> PostByJson(string url, string jsonParams)
        {
            HttpContent content = new StringContent(jsonParams, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostByForm(string url, string postParams)
        {
            HttpContent content = new StringContent(postParams, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Get(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}