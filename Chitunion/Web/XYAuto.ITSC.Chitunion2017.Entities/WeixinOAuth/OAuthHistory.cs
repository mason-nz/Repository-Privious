using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace XYAuto.ITSC.Chitunion2017.Entities.WeixinOAuth
{
    public class OAuthHistory
    {
      /// <summary>
        /// RecID
        /// </summary>
		public int RecID { get; set; }
        /// <summary>
        /// WxID
        /// </summary>
        public int WxID { get; set; }
        /// <summary>
        /// AppID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }
   }
}

