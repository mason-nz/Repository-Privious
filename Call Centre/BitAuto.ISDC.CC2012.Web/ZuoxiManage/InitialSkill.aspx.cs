using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Data;
using System.Data;
using System.Configuration;
using System.Net;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.Web.ZuoxiManage
{
    public partial class InitialSkill : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string msg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dt = GetNeedInitialUserInfo();
            int loginid = BLL.Util.GetLoginUserID();

            foreach (DataRow row in dt.Rows)
            {
                InsertCallOutSkillByUserID(CommonFunction.ObjectToInteger(row["UserID"]), loginid);
                string sgidpriority = row["SGID"].ToString() + "=3";
                if (!string.IsNullOrEmpty(row["HasSgidAndPriority"].ToString()))
                {
                    sgidpriority = sgidpriority + ";" + row["HasSgidAndPriority"].ToString();
                }
                GetHeLiDataByUserNameAndAgentNum(row["TrueName"].ToString(), row["AgentNum"].ToString(), sgidpriority, row["BGID"].ToString(), row["Name"].ToString());
            }
            if (string.IsNullOrEmpty(msg))
            {
                int count = 0;
                if (dt != null)
                {
                    count = dt.Rows.Count;
                }
                lb_msg.Text = "初始化外呼技能成功，共初始化 " + count + " 条数据";
            }
            else
            {
                lb_msg.Text = "初始化外呼技能结束，异常信息如下:</br>" + msg;
            }
        }
        /// <summary>
        ///给指定的用户添加外呼技能组技能
        /// </summary>
        /// <param name="TheUserId"></param>
        /// <param name="msg"></param>
        private void InsertCallOutSkillByUserID(int TheUserId, int loginid)
        {
            if (TheUserId > 0 && loginid > 0)
            {
                string strSql = @"
                                 INSERT INTO dbo.UserSkillDataRight
                                         ( UserID ,
                                           SGID ,
                                           SkillPriority ,
                                           CreateTime ,
                                           CreateUserID
                                         )
                                 VALUES  ( '" + TheUserId + @"' , 
                                           (SELECT SGID FROM dbo.SkillGroup WHERE Name='外呼技能组') ,  
                                           3 ,  
                                           GETDATE() , 
                                           '" + loginid + @"'   
                                         )";
                int backval = 0;
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionStrings_CC"].ToString()))
                {
                    SqlCommand com = new SqlCommand();
                    com.Connection = conn;
                    com.CommandText = strSql;
                    com.CommandType = CommandType.Text;
                    conn.Open();
                    backval = com.ExecuteNonQuery();
                }
                if (backval <= 0)
                {
                    msg += "添加外呼技能组技能失败： UserID=" + TheUserId + "</br>";
                    BLL.Loger.Log4Net.Error("坐席‘外呼技能组’技能初始化出现异常：UserID=" + TheUserId + "的坐席数据初始化失败");
                }
            }
            else
            {
                msg += "添加外呼技能组技能传递参数异常： UserID=" + TheUserId + ",LoginID=" + loginid + "</br>";
                BLL.Loger.Log4Net.Error("坐席‘外呼技能组’技能初始化传递参数异常：UserID=" + TheUserId + ",LoginID=" + loginid + "");
            }
        }

        private void GetHeLiDataByUserNameAndAgentNum(string TheUserName, string TheAgentNum, string TheSGID,string TheBGID, string TheBusinessGroupName)
        {
            string strUrl = ConfigurationUtil.GetAppSettingValue("HeLiURL") + "/busiService/addAgentInfo?"
                + "userName=" + TheUserName
                + "&agentId=" + TheAgentNum
                + "&deptId=" + TheBGID
                + "&deptName=" + TheBusinessGroupName
                + "&skill=" + TheSGID;
            try
            {
                HttpWebResponse webResp = HttpHelper.CreateGetHttpResponse(strUrl);
                string data = HttpHelper.GetResponseString(webResp);
                ResultJson jsondata = (ResultJson)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(data, typeof(ResultJson));

                if (jsondata != null)
                {
                    if (jsondata.returncode == "0")
                    {
                        msg += "合力厂商接口调用失败，接口返回信息： " + jsondata.returnmsg + " ,访问url(" + strUrl + ")</br>";
                        BLL.Loger.Log4Net.Error("GetHeLiDataByUserNameAndAgentNum（）调用合力厂商接口调用失败：访问url(" + strUrl + "),返回信息[" + jsondata.returnmsg + "]");
                    }
                }
                else
                {
                    msg += "合力厂商接口调用失败，未能返回数据：    访问url(" + strUrl + ")</br>";
                    BLL.Loger.Log4Net.Error("GetHeLiDataByUserNameAndAgentNum（）调用合力厂商接口异常，：访问url(" + strUrl + ")未能返回数据！");
                }
            }
            catch (Exception ex)
            {
                msg += "合合力厂商接口异常，导致无法访问：   访问url(" + strUrl + ")</br>";
                BLL.Loger.Log4Net.Error("GetHeLiDataByUserNameAndAgentNum（）调用合力厂商接口异常：访问url(" + strUrl + "); ", ex);
            }
        }


        public DataTable GetNeedInitialUserInfo()
        {
            try
            {
                string strSql = @"SELECT   a.UserID,a.AgentNum,a.BGID,c.Name,b.TrueName,(SELECT TOP 1 SGID FROM dbo.SkillGroup WHERE Name='外呼技能组') AS SGID
                                ,(SELECT ISNULL(STUFF(( SELECT    ';' + RTRIM(usgdr.SGID) + '=' + RTRIM(usgdr.SkillPriority)
                                                                  FROM   dbo.UserSkillDataRight usgdr INNER JOIN dbo.SkillGroup AS sg ON usgdr.SGID=sg.SGID
                                                                  WHERE  usgdr.UserID =  a.UserID AND  sg.RegionID = 2
                                                                FOR
                                                                  XML PATH('')
                                                                ), 1, 1, ''),'')) AS HasSgidAndPriority
                                FROM dbo.EmployeeAgent AS a INNER JOIN SysRightsManager.dbo.UserInfo AS b
                                ON a.UserID = b.UserID
                                INNER JOIN dbo.BusinessGroup AS c ON a.BGID = c.BGID
                                WHERE a.RegionID=2 AND a.AgentNum !='' AND a.UserID NOT IN(
                                SELECT UserID 
                                FROM dbo.UserSkillDataRight WHERE SGID = ISNULL((SELECT TOP 1 SGID FROM dbo.SkillGroup WHERE Name='外呼技能组'),''))";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionStrings_CC"].ToString()))
                {
                    SqlCommand com = new SqlCommand();
                    com.Connection = conn;
                    com.CommandText = strSql;
                    com.CommandType = CommandType.Text;
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("异常：" + ex.Message);
                Response.End();
                return null;
            }
        }
    }


}