/********************************************************
*创建人：hant
*创建时间：2018/1/12 17:30:28 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.V2_3
{
    public class LE_WXUserScene
    {
        public static readonly LE_WXUserScene Instance = new LE_WXUserScene();

        /// <summary>
        /// 获取用户场景
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<Entities.DTO.V2_3.WXUserSceneRspDTO> GetUserSceneByUserId(int sencetype,int userid)
        {
            if (sencetype == 0)//全部场景
            {
                return Dal.WXUserScene.Instance.GetUserSceneByUserId(userid);
            }
            else
            {
                var item = Dal.WXUserScene.Instance.GetUserSceneByUserId(userid).Where(p => p.IsSelected == 1).ToList();
                Entities.DTO.V2_3.WXUserSceneRspDTO tuijian = new Entities.DTO.V2_3.WXUserSceneRspDTO();
                tuijian.SceneID = 0;
                tuijian.SceneName = "推荐";
                tuijian.IsSelected = 1;
                tuijian.Counts = 10000;
                item.Add(tuijian);
                return item.OrderByDescending(a => a.Counts).ToList();
            }
        }

        /// <summary>
        /// 事务更新用户场景
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool UpdateWeiXinUserScene(int userid,Entities.DTO.V2_3.WXUserSceneResDTO res)
        {
            return Dal.WXUserScene.Instance.UpdateWeiXinUserScene(userid,res);
        }
    }
}
