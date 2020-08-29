using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using CC2015_HollyFormsApp.CCWeb.CallRecordService;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace CC2015_HollyFormsApp
{
    public class MDBFileHelper
    {
        public static MDBFileHelper Instance = new MDBFileHelper();
        /// 本地数据库文件位置
        /// <summary>
        /// 本地数据库文件位置
        /// </summary>
        public static string MdbFileName = AppDomain.CurrentDomain.BaseDirectory + "holly.mdb";
        /// 是否有mdb文件
        /// <summary>
        /// 是否有mdb文件
        /// </summary>
        public static bool HasMdb
        {
            get
            {
                return File.Exists(MdbFileName);
            }
        }

        /// 尝试下载mdb文件
        /// <summary>
        /// 尝试下载mdb文件
        /// </summary>
        public void DownLoadMdbFile()
        {
            try
            {
                //校验有没有mdb文件
                if (!HasMdb)
                {
                    //没有，尝试下线
                    string downurl = ConfigurationManager.AppSettings["DefaultURL"].ToString().TrimEnd('/') + "/down/holly.mdb.zip";
                    System.Net.WebClient webClient = new System.Net.WebClient();
                    webClient.DownloadFile(downurl, MdbFileName + ".zip");
                    //解压
                    Common.UnZip(MdbFileName + ".zip", AppDomain.CurrentDomain.BaseDirectory);
                    File.Delete(MdbFileName + ".zip");
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("下载mdb文件失败：" + ex.Message);
            }
        }
        /// 保存未发送成功数据
        /// <summary>
        /// 保存未发送成功数据
        /// </summary>
        /// <param name="BackupModel"></param>
        public void SaveCallRecordORIG(CC2015_HollyFormsApp.CCWeb.CallRecordService.CallRecord_ORIG BackupModel)
        {
            try
            {
                if (BackupModel != null)
                {
                    Loger.Log4Net.Info("[***MDBFileHelper***][SaveCallRecordORIG] 保存未发送成功数据");

                    #region sql
                    string f = "yyyy-MM-dd HH:mm:ss:fff";
                    string sql = @"INSERT  INTO [CallRecord_ORIG]
                                                    ( [SessionID] ,[CallID] ,[ExtensionNum] ,[PhoneNum] ,[ANI] ,[CallStatus] ,[SwitchINNum] ,[OutBoundType] ,[SkillGroup] ,
                                                      [InitiatedTime] ,[RingingTime] ,[EstablishedTime] ,[AgentReleaseTime] ,[CustomerReleaseTime] ,[AfterWorkBeginTime] ,
                                                      [AfterWorkTime] ,[TallTime] ,[AudioURL] ,
                                                      [ConsultTime] ,[ReconnectCall] , [CreateTime] ,
                                                      [CreateUserID],
                                                      [TransferInTime],[TransferOutTime]
	                                              )
                                            VALUES  ( '" + BackupModel.SessionID + @"' ,
                                                      '" + (BackupModel.CallID.HasValue ? BackupModel.CallID.Value.ToString() : "") + @"' ,
                                                      '" + (BackupModel.ExtensionNum == null ? "" : BackupModel.ExtensionNum) + @"' ,
                                                      '" + (BackupModel.PhoneNum == null ? "" : BackupModel.PhoneNum) + @"' ,
                                                      '" + (BackupModel.ANI == null ? "" : BackupModel.ANI) + @"' ,
                                                      " + (BackupModel.CallStatus.HasValue ? BackupModel.CallStatus.ToString() : "null") + @" ,
                                                      '" + (BackupModel.SwitchINNum == null ? "" : BackupModel.SwitchINNum) + @"' ,
                                                      " + (BackupModel.OutBoundType.HasValue ? BackupModel.OutBoundType.ToString() : "null") + @" ,
                                                      '" + (BackupModel.SkillGroup == null ? "" : BackupModel.SkillGroup) + @"' ,

                                                      '" + (BackupModel.InitiatedTime.HasValue ? BackupModel.InitiatedTime.Value.ToString(f) : "") + @"' ,
                                                      '" + (BackupModel.RingingTime.HasValue ? BackupModel.RingingTime.Value.ToString(f) : "") + @"' ,
                                                      '" + (BackupModel.EstablishedTime.HasValue ? BackupModel.EstablishedTime.Value.ToString(f) : "") + @"' ,
                                                      '" + (BackupModel.AgentReleaseTime.HasValue ? BackupModel.AgentReleaseTime.Value.ToString(f) : "") + @"' ,
                                                      '" + (BackupModel.CustomerReleaseTime.HasValue ? BackupModel.CustomerReleaseTime.Value.ToString(f) : "") + @"' ,
                                                      '" + (BackupModel.AfterWorkBeginTime.HasValue ? BackupModel.AfterWorkBeginTime.Value.ToString(f) : "") + @"' ,

                                                      " + (BackupModel.AfterWorkTime.HasValue ? BackupModel.AfterWorkTime.ToString() : "null") + @" ,
                                                      " + (BackupModel.TallTime.HasValue ? BackupModel.TallTime.ToString() : "null") + @" ,
                                                      '" + (BackupModel.AudioURL == null ? "" : BackupModel.AudioURL) + @"' ,

                                                      '" + (BackupModel.ConsultTime.HasValue ? BackupModel.ConsultTime.Value.ToString(f) : "") + @"' ,
                                                      '" + (BackupModel.ReconnectCall.HasValue ? BackupModel.ReconnectCall.Value.ToString(f) : "") + @"' ,
                                                      '" + (BackupModel.CreateTime.HasValue ? BackupModel.CreateTime.Value.ToString(f) : "") + @"' ,

                                                      " + (BackupModel.CreateUserID.HasValue ? BackupModel.CreateUserID.ToString() : "-1") + @" ,

                                                      '" + (BackupModel.TransferInTime.HasValue ? BackupModel.TransferInTime.Value.ToString(f) : "") + @"' ,
                                                      '" + (BackupModel.TransferOutTime.HasValue ? BackupModel.TransferOutTime.Value.ToString(f) : "") + @"' 
	                                              )";
                    #endregion
                    AccessDB adb = new AccessDB();
                    adb.ExecuteSQLNonquery(sql);
                    adb.Close();
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[***MDBFileHelper***][SaveCallRecordORIG] 异常：", ex);
            }
        }

        /// 补录数据
        /// <summary>
        /// 补录数据
        /// </summary>
        public void AdditionalRecording()
        {
            try
            {
                Loger.Log4Net.Info("[***MDBFileHelper***][AdditionalRecording] 补录数据");
                AccessDB adb = new AccessDB();
                DataTable dt = adb.SelectToDataTable("select * from CallRecord_ORIG where iif(isnull([SendCount]),0,[SendCount])<45");

                adb.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    Loger.Log4Net.Info("[***MDBFileHelper***][AdditionalRecording] 循环补录数据：" + dt.Rows.Count);
                    int i = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        bool success = false;
                        CC2015_HollyFormsApp.CCWeb.CallRecordService.CallRecord_ORIG model = new CC2015_HollyFormsApp.CCWeb.CallRecordService.CallRecord_ORIG();
                        try
                        {
                            model.SessionID = CommonFunction.ObjectToString(dr["SessionID"]);
                            model.CallID = CommonFunction.ObjectToLong(dr["CallID"]);
                            model.ExtensionNum = CommonFunction.ObjectToString(dr["ExtensionNum"]);
                            model.PhoneNum = CommonFunction.ObjectToString(dr["PhoneNum"]);
                            model.ANI = CommonFunction.ObjectToString(dr["ANI"]);
                            model.CallStatus = CommonFunction.ObjectToInteger(dr["CallStatus"]);
                            model.SwitchINNum = CommonFunction.ObjectToString(dr["SwitchINNum"]);
                            model.OutBoundType = CommonFunction.ObjectToInteger(dr["OutBoundType"]);
                            model.SkillGroup = CommonFunction.ObjectToString(dr["SkillGroup"]);

                            model.InitiatedTime = Common.ObjectToDateTime(dr["InitiatedTime"]);
                            model.RingingTime = Common.ObjectToDateTime(dr["RingingTime"]);
                            model.EstablishedTime = Common.ObjectToDateTime(dr["EstablishedTime"]);
                            model.AgentReleaseTime = Common.ObjectToDateTime(dr["AgentReleaseTime"]);
                            model.CustomerReleaseTime = Common.ObjectToDateTime(dr["CustomerReleaseTime"]);
                            model.AfterWorkBeginTime = Common.ObjectToDateTime(dr["AfterWorkBeginTime"]);
                            model.AfterWorkTime = CommonFunction.ObjectToInteger(dr["AfterWorkTime"]);
                            model.ConsultTime = Common.ObjectToDateTime(dr["ConsultTime"]);
                            model.ReconnectCall = Common.ObjectToDateTime(dr["ReconnectCall"]);

                            model.TallTime = CommonFunction.ObjectToInteger(dr["TallTime"]);
                            model.AudioURL = CommonFunction.ObjectToString(dr["AudioURL"]);
                            model.CreateTime = Common.ObjectToDateTime(dr["CreateTime"]);
                            model.CreateUserID = CommonFunction.ObjectToInteger(dr["CreateUserID"]);
                            model.TransferInTime = Common.ObjectToDateTime(dr["TransferInTime"]);
                            model.TransferOutTime = Common.ObjectToDateTime(dr["TransferOutTime"]);

                            model.SiemensCallID = -1;
                            model.GenesysCallID = model.CallID.Value.ToString();
                            //调用接口保存
                            success = CallRecordHelper.Instance.AddCallRecordNew(model);
                            Loger.Log4Net.Info("[***MDBFileHelper***][AdditionalRecording] （" + i + "/" + dt.Rows.Count + "）同步数据：" + success);
                            i++;
                        }
                        catch
                        {
                            success = false;
                        }

                        adb = new AccessDB();
                        if (success)
                        {
                            //更新成功，删除数据
                            adb.ExecuteSQLNonquery("delete from CallRecord_ORIG where SessionID='" + model.SessionID + "'");
                            Loger.Log4Net.Info("[***MDBFileHelper***][AdditionalRecording] 更新成功，删除本地数据：" + model.SessionID);
                        }
                        else
                        {
                            //更新失败，更新数据
                            int SendCount = CommonFunction.ObjectToInteger(dr["SendCount"]);
                            adb.ExecuteSQLNonquery("update CallRecord_ORIG set [SendCount]=iif(isnull([SendCount]),0,[SendCount])+1 where SessionID='" + model.SessionID + "'");
                            Loger.Log4Net.Info("[***MDBFileHelper***][AdditionalRecording] 更新失败，计数本地数据：" + model.SessionID + " 计数：" + (SendCount + 1));
                        }
                        adb.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[***MDBFileHelper***][AdditionalRecording] 异常：", ex);
                if (ex.Message.Contains("不可识别的数据库格式"))
                {
                    try
                    {
                        Loger.Log4Net.Info("[***MDBFileHelper***][AdditionalRecording] [重新下载数据库文件] 删除已有的文件：" + MdbFileName);
                        File.Delete(MdbFileName);
                        DownLoadMdbFile();
                    }
                    catch (Exception ex2)
                    {
                        Loger.Log4Net.Error("[***MDBFileHelper***][AdditionalRecording] [重新下载数据库文件]异常：", ex2);
                    }
                }
            }
        }
    }
}
