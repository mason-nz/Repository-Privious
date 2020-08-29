using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media
{
    public class MediaWeixin
    {
        #region Instance

        public static readonly MediaWeixin Instance = new MediaWeixin();

        #endregion Instance

        #region Contructor

        protected MediaWeixin()
        { }

        #endregion Contructor

        public Entities.Media.MediaWeixin GetEntity(int mediaId)
        {
            return Dal.Media.MediaWeixin.Instance.GetEntity(mediaId);
        }

        public Entities.Media.MediaWeixin GetEntity(string number, int filterMediaId = 0)
        {
            return Dal.Media.MediaWeixin.Instance.GetEntity(number, string.Empty, filterMediaId);
        }

        public Entities.Media.MediaWeixin GetEntityByName(string name, int filterMediaId = 0)
        {
            return Dal.Media.MediaWeixin.Instance.GetEntity(null, name, filterMediaId);
        }

        public Entities.Media.MediaWeixin GetNormalEntityByWxID(int wxID, int userID, bool IsAE)
        {
            return Dal.Media.MediaWeixin.Instance.GetNormalEntityByWxID(wxID, userID, IsAE);
        }

        public RespMediaForMediaRoleDto GetItemForMediaRole(int mediaId)
        {
            var info = Dal.Media.MediaWeixin.Instance.GetItemForMediaRole(mediaId);
            if (info != null)
            {
                return Mapper.Map<Entities.Media.MediaWeixin, RespMediaForMediaRoleDto>(info);
            }
            return null;
        }

        public int Insert(Entities.Media.MediaWeixin entity)
        {
            return Dal.Media.MediaWeixin.Instance.Insert(entity);
        }

        public int Update(Entities.Media.MediaWeixin entity)
        {
            return Dal.Media.MediaWeixin.Instance.Update(entity);
        }

        public int VerifyMediaCountByRole(string wxNumber, string roleId)
        {
            return Dal.Media.MediaWeixin.Instance.VerifyMediaCountByRole(wxNumber, roleId);
        }

        /// <summary>
        /// 插入Fans_Weixin表，粉丝数比例数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fansMalePer">男粉丝数比例</param>
        /// <param name="fansFemalePer">女粉丝数比例</param>
        /// <returns></returns>
        public int InsertAuthFansWeixinByFansSex(Entities.Media.MediaFansArea entity, decimal fansMalePer,
            decimal fansFemalePer)
        {
            return Dal.Media.MediaFansArea.Instance.InsertAuthFansWeixinByFansSex(entity, fansMalePer, fansFemalePer);
        }

        public List<RespSearchMediaDto> SearchMedia(MediaQuery<RespSearchMediaDto> query)
        {
            return Dal.WeixinOAuth.Instance.SearchMedia(query);
        }

        public int UpdateSqlTest()
        {
            return Dal.Media.MediaWeixin.Instance.UpdateSqlTest();
        }
    }
}