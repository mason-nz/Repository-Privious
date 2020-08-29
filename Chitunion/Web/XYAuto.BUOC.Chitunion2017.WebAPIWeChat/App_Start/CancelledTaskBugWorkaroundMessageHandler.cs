/********************************************************
*创建人：lixiong
*创建时间：2017/9/18 17:08:13
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat
{
    /// <summary>
    /// auth:lixiong
    /// System.Threading.CancellationToken.ThrowOperationCanceledException()
    /// 为了解决这个bug
    /// </summary>
    public class CancelledTaskBugWorkaroundMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // Try to suppress response content when the cancellation token has fired; ASP.NET will log to the Application event log if there's content in this case.
            if (cancellationToken.IsCancellationRequested)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return response;
        }
    }
}