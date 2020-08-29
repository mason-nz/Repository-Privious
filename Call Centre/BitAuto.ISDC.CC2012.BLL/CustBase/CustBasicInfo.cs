using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���CustBasicInfo ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:12 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustBasicInfo
    {
        #region Instance
        public static readonly CustBasicInfo Instance = new CustBasicInfo();
        #endregion

        #region Contructor
        protected CustBasicInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// ���ݵ绰��ȡ�ͻ���Ϣ
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfosByTel(string tel)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfosByTel(tel);
        }
        /// <summary>
        /// ���ݵ绰����ϵ��������ģ��ƥ�䣩��ȡ�ͻ���Ϣ
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfosByTelAndName(string tel, string custName)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfosByTelAndName(tel, custName);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.CustBasicInfo GetCustBasicInfo(string CustID)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfo(CustID);
        }
        /// <summary>
        /// ���ݿͻ����ƺ͵绰����ѯ�ͻ���Ϣ
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="custName"></param>
        /// <returns></returns>
        public Entities.CustBasicInfo GetCustBasicInfo(string tel, string custName)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfo(tel, custName);
        }
        #endregion

        /// <summary>
        /// ����һ������,����CustID
        /// </summary>
        public string Insert(Entities.CustBasicInfo model)
        {
            return Dal.CustBasicInfo.Instance.Insert(model);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.CustBasicInfo model)
        {
            return Dal.CustBasicInfo.Instance.Update(model);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(string custID)
        {
            return Dal.CustBasicInfo.Instance.Delete(custID);
        }

        /// ������Ϣ�������û��⣺�ӿڣ�����
        /// <summary>
        /// ������Ϣ�������û��⣺�ӿڣ�����
        /// </summary>
        /// <param name="CustName">�ͻ�����</param>
        /// <param name="Tels">�绰���룬������</param>
        /// <param name="Sex">�ձ� �У�1,Ů��2</param>
        /// <param name="CreateUserID">������</param>
        /// <param name="msg">���ص��ý����Ϣ</param>
        /// <param name="CustID">�ͻ��ŷ���ֵ</param>
        public bool InsertCustInfo(string CustName, string[] Tels, int Sex, int CreateUserID, int custCategory, int? update_provinceid, int? update_cityid, out string msg, out string CustID)
        {
            CustID = string.Empty;
            msg = string.Empty;

            try
            {
                #region ��֤����
                if (CustName == string.Empty)
                {
                    msg = "'result':'false','errorMsg':'���ݸ�ʽ����,��������Ϊ�գ�'";
                    return false;
                }
                else if (Tels.Length == 0)
                {
                    msg = "'result':'false','errorMsg':'���ݸ�ʽ����,�绰����Ϊ�գ�'";
                    return false;
                }
                else if (Tels.Length > 0)
                {
                    foreach (string tel in Tels)
                    {
                        if (string.IsNullOrEmpty(tel))
                        {
                            msg = "'result':'false','errorMsg':'���ݸ�ʽ����,�绰����Ϊ�գ�'";
                            return false;
                        }
                    }
                }
                //У��ͻ����Ͳ�����ֻ�ܴ���3��4����
                if (custCategory == (int)CustTypeEnum.T01_����)
                {
                    custCategory = (int)CustTypeEnum.T01_����;
                }
                else
                {
                    custCategory = (int)CustTypeEnum.T02_������;
                }
                #endregion

                CustID = GetMaxNewCustBasicInfoByTel(Tels);
                Entities.CustBasicInfo model = null;
                if (!string.IsNullOrEmpty(CustID))
                {
                    #region �޸Ĳ���
                    //���¿ͻ�
                    model = BLL.CustBasicInfo.Instance.GetCustBasicInfo(CustID);
                    if (update_provinceid.HasValue)
                    {
                        model.ProvinceID = update_provinceid;
                    }
                    if (update_cityid.HasValue)
                    {
                        model.CityID = update_cityid;
                    }
                    model.CustCategoryID = custCategory;
                    model.ModifyTime = DateTime.Now;
                    model.Sex = Sex;
                    BLL.CustBasicInfo.Instance.Update(model);
                    //���µ绰
                    foreach (string tel in Tels)
                    {
                        if (!BLL.CustTel.Instance.IsExistsByCustIDAndTel(CustID, tel))
                        {
                            Entities.CustTel modeltel = new Entities.CustTel();
                            modeltel.CreateTime = DateTime.Now;
                            modeltel.CreateUserID = CreateUserID;
                            modeltel.CustID = CustID;
                            modeltel.Tel = tel;
                            try
                            {
                                BLL.CustTel.Instance.Insert(modeltel);
                            }
                            catch (Exception ex)
                            {
                                msg = "'result':'false','errorMsg':'���롾�绰��" + modeltel + "ʧ�ܣ�'";
                                Loger.Log4Net.Error("��������Ϣ�������û��⣺�ӿڡ�", ex);
                                return false;
                            }
                        }
                    }
                    #endregion
                    msg = "'result':'true','CustID':'" + CustID + "'";
                }
                else
                {
                    #region ��������
                    model = new Entities.CustBasicInfo();
                    model.CustName = CustName;
                    model.Sex = Sex;
                    model.CustCategoryID = custCategory;//3-�����̣�4-���ˣ�
                    int pID = 0, cID = 0;
                    BLL.PhoneNumDataDict.GetAreaId(Tels[0], out pID, out cID);
                    model.ProvinceID = pID == 0 ? -2 : pID;
                    model.CityID = cID == 0 ? -2 : cID;
                    model.CountyID = -1;
                    model.AreaID = "";//�����͸���ʱ�Զ�����
                    model.CallTime = 0;
                    model.Status = 0;
                    model.CreateUserID = model.ModifyUserID = CreateUserID;
                    model.CreateTime = model.ModifyTime = DateTime.Now;
                    CustID = BLL.CustBasicInfo.Instance.Insert(model);

                    foreach (string tel in Tels)
                    {
                        Entities.CustTel model_Tel = new Entities.CustTel();
                        model_Tel.CustID = CustID;
                        model_Tel.CreateTime = DateTime.Now;
                        model_Tel.CreateUserID = CreateUserID;
                        model_Tel.Tel = tel;
                        try
                        {
                            BLL.CustTel.Instance.Insert(model_Tel);
                        }
                        catch (Exception ex)
                        {
                            msg = "'result':'false','errorMsg':'���롾�绰��" + model_Tel + "ʧ�ܣ�";
                            Loger.Log4Net.Error("��������Ϣ�������û��⣺�ӿڡ�", ex);
                            return false;
                        }
                    }
                    #endregion
                    msg = "'result':'true','CustID':'" + CustID + "'";
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("��������Ϣ�������û��⣺�ӿڡ�", ex);
                msg = "'result':'false','errorMsg':'" + ex.Message + "'";
                return false;
            }
            return true;
        }

        /// ���ݵ绰������ȡֵ��ΪIMϵͳ��
        /// <summary>
        /// ���ݵ绰������ȡֵ��ΪIMϵͳ��
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfoForIM(string tel)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfoForIM(tel);
        }
        /// ���� ʡ ���� ���� ���¼��������Ϣ
        /// <summary>
        /// ���� ʡ ���� ���� ���¼��������Ϣ
        /// qiangfei
        /// 2014-12-17
        /// </summary>
        /// <param name="model"></param>
        public void ReCalcAreaDistrict(Entities.CustBasicInfo model)
        {
            BitAuto.YanFa.Crm2009.Entities.AreaInfo info = Util.GetAreaInfoByPCC(
                CommonFunction.ObjectToString(model.ProvinceID),
                CommonFunction.ObjectToString(model.CityID),
                CommonFunction.ObjectToString(model.CountyID));
            if (info != null)
            {
                model.AreaID = info.District;
            }
            else
            {
                model.AreaID = "";
            }
        }
        /// ���ݵ绰�����ѯ��ʷ�ͻ����°棩 ǿ� 2016-4-7
        /// <summary>
        /// ���ݵ绰�����ѯ��ʷ�ͻ����°棩 ǿ� 2016-4-7
        /// ����ʱ������ȡ���µĿͻ�
        /// </summary>
        /// <param name="tels"></param>
        /// <param name="taskid"></param>
        /// <param name="retList"></param>
        public void GetCallRecordORGIHistory(string tels, string taskid, out List<string[]> retList)
        {
            Dal.CustBasicInfo.Instance.GetCallRecordORGIHistory_New(tels, taskid, out retList);
        }
        /// �����ֻ������ѯ���µĿͻ�ID
        /// <summary>
        /// �����ֻ������ѯ���µĿͻ�ID
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public string GetMaxNewCustBasicInfoByTel(params string[] tels)
        {
            return Dal.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(tels);
        }

        #region ������¹�����ѯ
        /// ��ѯ�����û���Ϣ
        /// <summary>
        /// ��ѯ�����û���Ϣ
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryCallInfo"></param>
        /// <param name="queryDealerInfo"></param>
        /// <param name="queryCustHistoryInfo"></param>
        /// <param name="outField"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfo(QueryCustBasicInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfo(query, order, currentPage, pageSize, out totalCount);
        }
        #endregion

        //������δʵ��
        public void Update(SqlTransaction tran, Entities.CustBasicInfo custmodel)
        {
            throw new NotImplementedException();
        }
        //������δʵ��
        public string Insert(SqlTransaction sqltran, Entities.CustBasicInfo model)
        {
            throw new NotImplementedException();
        }
        //������δʵ��
        public bool IsExistsByCustNameAndTel(string UserName, string Tel1)
        {
            throw new NotImplementedException();
        }
    }
}

