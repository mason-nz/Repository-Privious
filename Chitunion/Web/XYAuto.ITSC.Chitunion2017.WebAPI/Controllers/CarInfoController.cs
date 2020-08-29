using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    public class CarInfoController : ApiController
    {
        // GET api/carinfo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/carinfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/carinfo
        public void Post([FromBody]string value)
        {
        }

        // PUT api/carinfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/carinfo/5
        public void Delete(int id)
        {
        }

        #region 添加商品到购物车
        public Common.JsonResult AddGoods2CarInfo()
        {
            Common.JsonResult jr = new Common.JsonResult();

            return jr;
        }
        #endregion
    }
}
