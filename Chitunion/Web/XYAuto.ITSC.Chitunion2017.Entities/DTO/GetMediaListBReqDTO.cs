using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    //V1_1
    public class GetMediaListBReqDTO
    {
        public int MediaType { get; set; }
        private int auditstatus = Constants.Constant.INT_INVALID_VALUE;

        public int AuditStatus
        {
            get { return auditstatus; }
            set { auditstatus = value; }
        }

        public string Key { get; set; }
        private int categoryid = Constants.Constant.INT_INVALID_VALUE;

        public int CategoryID
        {
            get { return categoryid; }
            set { categoryid = value; }
        }

        private int leveltype = Constants.Constant.INT_INVALID_VALUE;

        public int LevelType
        {
            get { return leveltype; }
            set { leveltype = value; }
        }

        private int publishstatus = Constants.Constant.INT_INVALID_VALUE;

        public int PublishStatus
        {
            get { return publishstatus; }
            set { publishstatus = value; }
        }

        private int oauthtype = Constants.Constant.INT_INVALID_VALUE;

        public int OAuthType
        {
            get { return oauthtype; }
            set { oauthtype = value; }
        }

        private int oauthstatus = Constants.Constant.INT_INVALID_VALUE;

        public int OAuthStatus
        {
            get { return oauthstatus; }
            set { oauthstatus = value; }
        }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        private int pagesize = Constants.Constant.PageSize;

        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value; }
        }

        private int pageindex = 1;

        public int PageIndex
        {
            get { return pageindex; }
            set { pageindex = value; }
        }

        private int source = Constants.Constant.INT_INVALID_VALUE;

        public int Source
        {
            get { return source; }
            set { source = value; }
        }

        private int orderby = -2;

        public int OrderBy
        {
            get { return orderby; }
            set { orderby = value; }
        }

        //运营待审核
        public string SubmitStartDate { get; set; }

        public string SubmitEndDate { get; set; }
        public string AuditStartDate { get; set; }
        public string AuditEndDate { get; set; }
        public string SubmitUserName { get; set; }

        public int IsAreaMedia { get; set; } = Constants.Constant.INT_INVALID_VALUE;
        public int AreaProvniceId { get; set; } = Constants.Constant.INT_INVALID_VALUE;
        public int AreaCityId { get; set; } = Constants.Constant.INT_INVALID_VALUE;
    }
}