using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Data;

namespace BitAuto.ISDC.CC2012.WebService.Market
{
    public class MarketServiceHelper
    {
        #region Instance
        public static readonly MarketServiceHelper Instance = new MarketServiceHelper();
        private string MarketTokenKey = System.Configuration.ConfigurationManager.AppSettings["MarketTokenKey"];//市场活动API——TokenKey
        private string MarketAuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["MarketAuthorizeCode"];//市场活动API——AuthorizeCode
        MarketService.Recommend service = new MarketService.Recommend();
        MarketService.ReSoapHeader soapheader = new MarketService.ReSoapHeader();
        #endregion

        #region Contructor
        public MarketServiceHelper()
        {
            soapheader.TokenKey = new Guid(MarketTokenKey);
            soapheader.AuthorizeCode1 = MarketAuthorizeCode;
            service.ReSoapHeaderValue = soapheader;
        }
        #endregion

        public DataSet GetDataXml3(int brandid, int provinceid,string activityname)
        {
            string str = service.GetDataXml(brandid, provinceid, activityname);
            return ConvertDataTableByString(str);
        }

        /// <summary>
        /// 根据车型ID（一级ID或二级ID）、地区ID（省份ID或城市ID）查找推荐活动内容
        /// </summary>
        /// <param name="brandid">车型ID（一级ID或二级ID）</param>
        /// <param name="provinceid">地区ID（省份ID或城市ID）</param>
        /// <returns>返回List</returns>
        public DataSet GetDataXml(int brandid, int provinceid)
        {
            string str = service.GetDataXml(brandid, provinceid);
            return ConvertDataTableByString(str);
        }

        /// <summary>
        /// 根据GUID字符串数组，查询推荐活动列表
        /// </summary>
        /// <param name="guids">GUID字符串数组</param>
        /// <returns></returns>
        public DataSet GetDataXml(string[] guids)
        {
            string str = service.GetDataXml(guids);
            return ConvertDataTableByString(str);
        }


        private DataSet ConvertDataTableByString(string xmlData)
        {
            DataSet ds = new DataSet();
            switch (xmlData)
            {
                case "0": break;//查询不到相关记录
                case "-1": throw new Exception("调用市场活动接口，验证失败"); //验证失败
                case "-2": throw new Exception("调用市场活动接口，系统异常"); //系统异常
                default:
                    #region 转换DataTable
                    StringReader stream = null;
                    XmlTextReader reader = null;
                    try
                    {
                        stream = new StringReader(xmlData);
                        reader = new XmlTextReader(stream);
                        ds.ReadXml(reader);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("调用市场活动接口，转换DataTable异常", ex);
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                    #endregion
                    break;
            }
            return ds;
        }
    }
}
