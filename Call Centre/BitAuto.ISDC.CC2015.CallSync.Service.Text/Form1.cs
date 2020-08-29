using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BitAuto.ISDC.CC2012.Entities;
using System.Net;
using BitAuto.ISDC.CC2012.BLL;
using Newtonsoft.Json;

namespace BitAuto.ISDC.CC2015.CallSync.Service.Text
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            SyncRedundancy job = new SyncRedundancy();
            job.Run();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SplitCallData job = new SplitCallData();
            job.Run();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CallUploadFTPJob job = new CallUploadFTPJob();
            job.Run();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CallRecordQuery query1 = new CallRecordQuery();
            query1.queryType = QueryTypeHB.sql;
            query1.SelectSql = "aaa";
            query1.WhereSql = "1=1";
            string a = query1.ToString();

            CallRecordQuery query2 = new CallRecordQuery();
            query2.queryType = QueryTypeHB.std;
            query2.selFieldList.Add(SelFiledHBCallRecord.CallID);
            query2.selFieldList.Add(SelFiledHBCallRecord.CallStatus);
            query2.selFieldList.Add(SelFiledHBCallRecord.PhoneNumber);
            query2.selFieldList.Add(SelFiledHBCallRecord.CreateTime);
            query2.selFieldList.Add(SelFiledHBCallRecord.Otherdetail);

            query2.pageSize = 50000;
            query2.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.CallStatus, "1", ConditionHB.Equal, RelationshipHB.AND));
            query2.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.CallID, "1", ConditionHB.In, RelationshipHB.AND));
            query2.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.Oriani, "1", ConditionHB.In, RelationshipHB.AND));
            query2.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.OriDnis, "1", ConditionHB.RangeKK, RelationshipHB.OR));
            query2.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.OriDnis, "1", ConditionHB.RangeBB, RelationshipHB.OR));
            query2.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.OriDnis, "1", ConditionHB.RangeBK, RelationshipHB.OR));
            query2.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.OriDnis, "1", ConditionHB.RangeKB, RelationshipHB.OR));
            string b = query2.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CallRecordQuery query1 = new CallRecordQuery();
            query1.queryType = QueryTypeHB.std;
            query1.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.CallStatus, "1", ConditionHB.Equal, RelationshipHB.AND));
            query1.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.Oriani, "02968211276", ConditionHB.Equal, RelationshipHB.AND));
            var r = BitAuto.ISDC.CC2012.BLL.CallRecordForHB.Instance.GetCallRecordFromHabse(query1);
            var a = r.FieldList;
            var b = r.TrafficDetailCollection;
            var c = r.CallRecordDataList;
            var d = r.Data;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string url = "http://apincc.sys1.bitauto.com/callrecord/QueryAni?Oriani=18602900300";
            HttpWebResponse rep = HttpHelper.CreateGetHttpResponse(url);
            string data = HttpHelper.GetResponseString(rep);
            HttpHBResult result = (HttpHBResult)JsonConvert.DeserializeObject(data, typeof(HttpHBResult));
            CallRecordResult info = (CallRecordResult)Newtonsoft.Json.JsonConvert.DeserializeObject(result.Data, typeof(CallRecordResult));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CallDataExport job = new CallDataExport();
            job.Run();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CallUploadFTPJob job = new CallUploadFTPJob();
            job.RunOld();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DictionaryDataCache.Instance.InitData();
            DictionaryDataCache.Instance.ResetData();
            var a = DictionaryDataCache.Instance.AllAreaInfoList;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CallRecordReportJob job = new CallRecordReportJob();
            job.Run();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ReplenishCallRecordJob job = new ReplenishCallRecordJob();
            job.Run();
        }
    }
}
