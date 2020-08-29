using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat
{
    public class LE_WXUserScene
    {
        public static readonly LE_WXUserScene Instance = new LE_WXUserScene();

        public object ToSkip(ref string errorMsg)
        {
            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            errorMsg = string.Empty;
            //if (!req.CheckSelfModel(out errorMsg))
            //    return "操作失败";

            var model = new Entities.WeChat.LE_WXUserScene() {
                UserID= userId,
                SceneID=-3,//跳过
                SceneName="跳过",
                Status=0
            };
            bool isOK = Dal.WeChat.LE_WXUserScene.Instance.Insert(model);
            if (isOK)
            {
                return "操作成功";
            }
            else
            {
                errorMsg = "保存失败";
                return "操作失败";
            }
        }
        public object GetSceneInfoByUserId(ref string errorMsg)
        {
            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            errorMsg = string.Empty;
            var dt = Dal.WXUserScene.Instance.GetUserSceneByUserIdV2_5(userId);
            var CategoryList = BLL.Util.DataTableToList<Dto.GetUserSceneByUserId.Category>(dt);
            //添加推荐场景
            CategoryList.Insert(0, new Dto.GetUserSceneByUserId.Category()
            {
                SceneID = 0,
                SceneName = "推荐",
                IsSelected = 0,
                Counts = 10000
            });
            //CategoryList.Add(new Dto.GetUserSceneByUserId.Category()
            //{
            //    SceneID = 0,
            //    SceneName = "推荐",
            //    IsSelected = 1,
            //    Counts = 10000
            //});
            //保存 跳过 分类。
            var userSceneModel = Dal.WeChat.LE_WXUserScene.Instance.GetModel(new Entities.WeChat.LE_WXUserScene()
            {
                UserID = userId,
                SceneID = -3//跳过
            });
            
            var resDto = new Dto.GetUserSceneByUserId.ResDto()
            {
                CategoryList = new List<Dto.GetUserSceneByUserId.Category>(CategoryList),
                IsSkip = userSceneModel != null ? true : false
            };

            return resDto;
        }

        #region V2.8
        /// <summary>
        /// 根据场景ID获取场景名称
        /// </summary>
        /// <param name="sceneID"></param>
        /// <returns></returns>
        public Entities.UserScene GetSceneByID(int sceneID)
        {
            return Dal.WXUserScene.Instance.GetSceneByID(sceneID);
        }
        #endregion        
    }
}
