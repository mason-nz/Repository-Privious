using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Funnel
{
    [Serializable]
    public class FunnelMaterialDetailInfo
    {


        /// <summary>
        /// 渠道名称
        /// </summary>		
        private string _channelname = string.Empty;
        public string ChannelName
        {
            get { return _channelname; }
            set { _channelname = value; }
        }

        /// <summary>
        /// 场景名称
        /// </summary>		
        private string _scenename = string.Empty;
        public string SceneName
        {
            get { return _scenename; }
            set { _scenename = value; }
        }
        /// <summary>
        /// 头部文章账号类型名称
        /// </summary>		
        private string _aascoretypename = string.Empty;
        public string AAScoreTypeName
        {
            get { return _aascoretypename; }
            set { _aascoretypename = value; }
        }
        /// <summary>
        /// 封装
        /// </summary>		
        private int _encapsulate;
        public int Encapsulate
        {
            get { return _encapsulate; }
            set { _encapsulate = value; }
        }
        /// <summary>
        /// 分发
        /// </summary>		
        private int _distribute;
        public int Distribute
        {
            get { return _distribute; }
            set { _distribute = value; }
        }
        /// <summary>
        /// 转发
        /// </summary>		
        private int _forward;
        public int Forward
        {
            get { return _forward; }
            set { _forward = value; }
        }
        /// <summary>
        /// 线索
        /// </summary>		
        private int _clue;
        public int Clue
        {
            get { return _clue; }
            set { _clue = value; }
        }

    }
}
