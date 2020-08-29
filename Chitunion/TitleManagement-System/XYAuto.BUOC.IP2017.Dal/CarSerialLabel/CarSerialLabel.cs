using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.CarSerialLabel
{
    public class CarSerialLabel : DataBase
    {
        public static readonly CarSerialLabel Instance = new CarSerialLabel();
        public Tuple<DataTable, DataTable> RenderBatchCar(int batchMediaID)
        {
            var sbSql = new StringBuilder();
            //车信息
            sbSql.Append($@"
                            SELECT  BM.BatchMediaID ,
                                    CB.MasterId ,
                                    BM.BrandID ,
                                    BM.SerialID ,
                                    CMB.Name MasterName ,
                                    CB.Name BrandName ,
                                    CS.ShowName SerialName ,
                                    BM.SubmitTime CreateTime ,
                                    BM.AuditTime ,
                                    VUI.SysName AuditUser
                            FROM    dbo.BatchMedia BM
                                    JOIN BaseData2017.dbo.CarBrand CB ON CB.BrandID = BM.BrandID
                                    LEFT JOIN BaseData2017.dbo.CarSerial CS ON CS.SerialID = BM.SerialID
                                    JOIN BaseData2017.dbo.CarMasterBrand CMB ON CMB.MasterID = CB.MasterId
                                    LEFT JOIN Chitunion2017.dbo.v_UserInfo VUI ON VUI.UserID = BM.AuditUserID
                            WHERE   BM.BatchMediaID = {batchMediaID}");
            //固定标签项
            sbSql.Append(Dal.MediaLabel.MediaLabel.Instance.GetLabelSql((int)Entities.ENUM.ENUM.EnumLabelType.IP));

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return new Tuple<DataTable, DataTable>(data.Tables[0], data.Tables[1]);
        }
        public Tuple<DataTable, DataTable, DataTable, DataTable, DataTable, DataTable, DataTable> ViewBatchCar(int batchMediaID, int batchAuditID)
        {
            var sbSql = new StringBuilder();
            //车信息
            sbSql.Append($@"
                            SELECT  BM.BatchMediaID ,
                                    CB.MasterId ,
                                    BM.BrandID ,
                                    BM.SerialID ,
                                    CMB.Name MasterName ,
                                    CB.Name BrandName ,
                                    CS.ShowName SerialName ,
                                    BM.SubmitTime CreateTime ,
                                    BM.AuditTime ,
                                    VUI1.SysName OperateUser ,
                                    VUI2.SysName AuditUser
                            FROM    dbo.BatchMedia BM
                                    JOIN BaseData2017.dbo.CarBrand CB ON CB.BrandID = BM.BrandID
                                    LEFT JOIN BaseData2017.dbo.CarSerial CS ON CS.SerialID = BM.SerialID
                                    JOIN BaseData2017.dbo.CarMasterBrand CMB ON CMB.MasterID = CB.MasterId
                                    JOIN Chitunion2017.dbo.v_UserInfo VUI1 ON VUI1.UserID = BM.CreateUserID
                                    LEFT JOIN Chitunion2017.dbo.v_UserInfo VUI2 ON VUI2.UserID = BM.AuditUserID
                            WHERE   BM.BatchMediaID = {batchMediaID}");
            //打标签项item2
            sbSql.Append($@"SELECT  BLH.Type ,
                                    BLH.TitleID DictId,
                                    BLH.LabelID ,
                                    BLH.Name DictName
                            FROM    dbo.BatchLabelHistory BLH
                            WHERE   BLH.BatchMediaID = {batchMediaID};");

            //审标签项item3
            sbSql.Append($@"SELECT  BAP.TitleID DictId,
                                    BAP.Name DictName,
                                    BAP.AuditLabelID LabelID ,
                                    BAP.Type
                            FROM    dbo.BatchAuditPassed BAP
                                    JOIN dbo.BatchMediaAudit BMA ON BAP.BatchAuditID = BMA.BatchAuditID
                                    JOIN dbo.BatchMedia BM ON BM.BatchAuditID = BMA.BatchAuditID
                            WHERE   BM.BatchMediaID = {batchMediaID};");

            //子IP项item4
            sbSql.Append($@"SELECT  IPSUB.TitleID DictId,
                                    IPSUB.LabelID ,
                                    IPSUB.SubIPID ,
                                    TBI.Name DictName
                            FROM    dbo.IPSubLabel IPSUB
                                    JOIN dbo.TitleBasicInfo TBI ON TBI.TitleID = IPSUB.TitleID
                            WHERE   IPSUB.BatchMediaID = {batchMediaID};");

            //子IP下的标签项item5
            sbSql.Append($@"SELECT  SON.SubIPID ,
                                    SON.Name DictName,
                                    SON.BatchMediaID
                            FROM    dbo.SonIPLabel SON
                            WHERE   SON.BatchMediaID = {batchMediaID};");

            //子IP项（审核）item6
            sbSql.Append($@"SELECT  IPSUBAudit.TitleID DictId,
                                    IPSUBAudit.AuditLabelID LabelID,
                                    IPSUBAudit.AuditSubIPID SubIPID,
                                    TBI.Name DictName
                            FROM    dbo.IPSubLabelAuditPassed IPSUBAudit
                                    JOIN dbo.TitleBasicInfo TBI ON TBI.TitleID = IPSUBAudit.TitleID
                            WHERE   IPSUBAudit.BatchAuditID = {batchAuditID};");

            //子IP下的标签项（审核）item7
            sbSql.Append($@"SELECT  SONAudit.AuditSubIPID SubIPID,
                                    SONAudit.Name DictName,
                                    SONAudit.BatchAuditID
                            FROM    dbo.SonIPLabelAuditPassed SONAudit
                            WHERE   SONAudit.BatchAuditID = {batchAuditID};");

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return new Tuple<DataTable, DataTable, DataTable, DataTable, DataTable, DataTable, DataTable>(data.Tables[0], data.Tables[1], data.Tables[2], data.Tables[3], data.Tables[4], data.Tables[5], data.Tables[6]);
        }
    }
}
