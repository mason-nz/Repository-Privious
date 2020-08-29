/********************************************************
*创建人：lixiong
*创建时间：2017/6/13 10:26:47
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    public class OperateAuditMsg : DataBase
    {
        #region Instance

        public static readonly OperateAuditMsg Instance = new OperateAuditMsg();

        #endregion Instance

        public void OperateAuditMsgInsert(OperateAuditMsgEntity entity)
        {
            const string storedProcedure = "p_OperateMsgAPPInsert";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@ID",entity.RelationId),
                new SqlParameter("@MsgType",(int)entity.MsgType),
                new SqlParameter("@OptType",(int)entity.OptType),
                new SqlParameter("@CreateUserID",entity.CreateUserId)
            };

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, storedProcedure, sqlParams);
        }

        /// <summary>
        /// 审核日志操作
        /// </summary>
        /// <param name="relationId">媒体ID或广告ID或模板ID	</param>
        /// <param name="msgType">46001媒体审核，46002广告审核,46003广告过期,46004广告下架，模板审核46005</param>
        /// <param name="optType">媒体或广告审核（43001待审核，43002已通过，43003驳回），广告下架：42012,广告过期:42007到42010，模板审核（48001-48003）</param>
        /// <param name="createUserId">当前用户id</param>
        public void OperateAuditMsgInsert(int relationId, int msgType, int optType, int createUserId)
        {
            const string storedProcedure = "p_OperateMsgAPPInsert";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@ID",relationId),
                new SqlParameter("@MsgType",msgType),
                new SqlParameter("@OptType",optType),
                new SqlParameter("@CreateUserID",createUserId)
            };

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, storedProcedure, sqlParams);
        }
    }

    public class OperateAuditMsgEntity
    {
        public int RelationId { get; set; }
        public int CreateUserId { get; set; }
        public OperateAuditMsgType MsgType { get; set; }
        public OperateAuditMsgOptType OptType { get; set; }
    }

    public enum OperateAuditMsgType
    {
        [Description("媒体审核")]
        AuditMedia = 46001,

        [Description("广告审核")]
        AuditAdv = 46002,

        [Description("广告过期")]
        AuditAdvExpress = 46003,

        [Description("广告审核")]
        AuditAdvDownShelf = 46004,

        [Description("广告审核")]
        AuditTemplate = 46005,
    }

    public enum OperateAuditMsgOptType
    {
        [Description("媒体或广告审核-待审核")]
        AuditMediaAppending = 43001,

        [Description("媒体或广告审核-已通过")]
        AuditPass = 43002,

        [Description("媒体或广告审核-驳回")]
        AuditNotPass = 43003,

        [Description("广告下架")]
        AuditAdvDownShelf = 42012,

        [Description("广告过期")]
        AuditExpress = 42007,
    }
}