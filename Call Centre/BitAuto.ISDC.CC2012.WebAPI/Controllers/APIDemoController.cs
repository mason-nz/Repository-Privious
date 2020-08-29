using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BitAuto.ISDC.CC2012.WebAPI.Models.ChartModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BitAuto.ISDC.CC2012.WebAPI.Filter;

namespace BitAuto.ISDC.CC2012.WebAPI.Controllers
{
    [BitAutoAuthorizeAttribute(NeedCheckIP = true)]
    public class APIDemoController : ApiController
    {
        //达人排名
        [HttpGet]
        public string GetExpertConnect()
        {
            Random r = new Random();
            series s1 = new series()
            {
                data = new List<ChartNode>() {
                            new ChartNode(){x=0,name="马双君",  y = r.Next(),color="red"},
                            new ChartNode(){x=1, name="李鹤峰",  y = r.Next(),color="blue"},
                            new ChartNode(){x=2, name="齐志强", y = r.Next(),color="#f25e0f"},
                            new ChartNode(){x=3,name="毕帆",  y = r.Next()},
                            new ChartNode(){x=4,name="强斐",  y = r.Next()},
                            new ChartNode(){x=5, name="月宾", y = r.Next()},
                            new ChartNode(){ x=6,name="魏淑珍", y = r.Next()},
                            new ChartNode(){ x=7,name="刘萍", y = r.Next()},
                            new ChartNode(){ x=8,name="张媛", y = r.Next()},
                            new ChartNode(){ x=9,name="张媛2", y = r.Next()}
                }
            };
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s1 }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        //呼出业务接通情况实时监控 
        public string GetBusinessConnect()
        {
            Random r = new Random();
            series s1 = new series()
            {
                name = "外呼量",
                color = "blue",
                data = new List<ChartNode>() {
                            new ChartNode(){x=0,  y = r.Next()},
                            new ChartNode(){x=1,  y = r.Next()},
                            new ChartNode(){x=2, y = r.Next()},
                            new ChartNode(){x=3, y = r.Next()},
                            new ChartNode(){x=4, y = r.Next()},
                            new ChartNode(){x=5, y = r.Next()},
                            new ChartNode(){ x=6,y = r.Next()},
                            new ChartNode(){ x=7,y = r.Next()},
                            new ChartNode(){ x=8,y = r.Next()},
                            new ChartNode(){ x=9,y = r.Next()},
                            new ChartNode(){ x=10,y = r.Next()},
                            new ChartNode(){ x=11,y = r.Next()},
                            new ChartNode(){ x=12,y = r.Next()},
                            new ChartNode(){ x=13,y = r.Next()},
                            new ChartNode(){ x=14,y = r.Next()},
                            new ChartNode(){ x=15,y = r.Next()},
                            new ChartNode(){ x=16,y = r.Next()},
                            new ChartNode(){ x=17,y = r.Next()},
                            new ChartNode(){ x=18,y = r.Next()}
                            
                }
            };


            series s2 = new series()
            {
                name = "接通量",
                color = "#33FF00",
                data = new List<ChartNode>() {
                            new ChartNode(){x=0,  y = r.Next()},
                            new ChartNode(){x=1,  y = r.Next()},
                            new ChartNode(){x=2, y = r.Next()},
                            new ChartNode(){x=3, y = r.Next()},
                            new ChartNode(){x=4, y = r.Next()},
                            new ChartNode(){x=5, y = r.Next()},
                            new ChartNode(){ x=6,y = r.Next()},
                            new ChartNode(){ x=7,y = r.Next()},
                            new ChartNode(){ x=8,y = r.Next()},
                            new ChartNode(){ x=9,y = r.Next()},
                            new ChartNode(){ x=10,y = r.Next()},
                            new ChartNode(){ x=11,y = r.Next()},
                            new ChartNode(){ x=12,y = r.Next()},
                            new ChartNode(){ x=13,y = r.Next()},
                            new ChartNode(){ x=14,y = r.Next()},
                            new ChartNode(){ x=15,y = r.Next()},
                            new ChartNode(){ x=16,y = r.Next()},
                            new ChartNode(){ x=17,y = r.Next()},
                            new ChartNode(){ x=18,y = r.Next()}
                }
            };
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s1, s2 }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

        }

        //呼出业务队列实时监控
        public string GetBusinessQueue()
        {
            Random r = new Random();
            series s1 = new series()
            {
                data = new List<ChartNode>() {
                                new ChartNode(){ name="Call", x=0,y = r.Next(),color="red"},
                            new ChartNode(){ name="Free", x=1,y = r.Next(),color="#f25e0f"},
                            new ChartNode(){ name="Busy", x=2,y = r.Next(),color="green"},
                            new ChartNode(){ name="Online SCR", x=3,y = r.Next()}
                }
            };

            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s1 }, ErrorMsg = "", Success = true, ErrorNumber = 0 };

            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

        }

        public string GetTest(string id)
        {
            return "this is just a demo" + id;
        }

        //justDemo
        [Authorize]
        [NonAction]
        public string GetCallOut()
        {
            Random r = new Random();
            series s1 = new series()
            {
                name = "外呼量",
                data = new List<ChartNode>() {
                    new ChartNode(){ name="Call", x=0,y = r.Next(),color="red"},
                    new ChartNode(){ name="Free", x=1,y = r.Next(),color="#ffee00"},
                    new ChartNode(){ name="Busy", x=2,y = r.Next(),color="green"},
                    new ChartNode(){ name="Online SCR", x=3,y = r.Next()}
                }
            };


            series s2 = new series()
            {
                name = "接通量",
                data = new List<ChartNode>() {
                    new ChartNode(){ name="Call", x=0,y = r.Next()},
                    new ChartNode(){ name="Free", x=1,y = r.Next(),color="#f0f0fe"},
                    new ChartNode(){ name="Busy", x=2,y = r.Next()},
                    new ChartNode(){ name="Online SCR", x=3,y = r.Next()}
                }
            };
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s1, s2 }, ErrorMsg = "", Success = true, ErrorNumber = 0 };

            //var s = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
            var sContent = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return sContent;
            //return JavaScriptConvert.SerializeObject(result);
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
