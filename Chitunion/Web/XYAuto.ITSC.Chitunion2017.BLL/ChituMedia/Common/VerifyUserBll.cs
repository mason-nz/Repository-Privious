using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common
{
    public class VerifyUserBll
    {
        #region 单例
        //private VerifyUserBll() { }

        public static VerifyUserBll instance = null;
        public static readonly object padlock = new object();

        public static VerifyUserBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new VerifyUserBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        public readonly LoginUser loginUser;
        public VerifyUserBll()
        {
            try
            {
                if (UserInfo.GetLoginUser() != null)
                {
                    loginUser = UserInfo.GetLoginUser();
                }
                else
                {

                    loginUser = new LoginUser();
                }
            }
            catch (Exception ex)
            {

                loginUser = new LoginUser();
            }
        }

        /// <summary>
        /// 媒体主
        /// </summary>
        //public bool IsMediaUser
        //{
        //    get
        //    {
        //        return loginUser.Category == 29002 ? true : false;
        //    }
        //}

        ///// <summary>
        ///// 广告主用户
        ///// </summary>
        //public bool IsAdvertiser
        //{
        //    get
        //    {
        //        return loginUser.Category == 29001 ? true : false;
        //    }
        //}
    }
}
