using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.Recommend
{
    //Media_Recommend
    public class HomeMedia
    {
        public HomeMedia()
        {
            CreateTime = DateTime.Now;
        }

        public int RecID { get; set; }

        public int CategoryID { get; set; }
        public int TemplateID { get; set; }
        public int MediaID { get; set; }
        public int ADDetailID { get; set; }
        public int MediaType { get; set; }

        public int PublishState { get; set; }

        public int SortNumber { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}