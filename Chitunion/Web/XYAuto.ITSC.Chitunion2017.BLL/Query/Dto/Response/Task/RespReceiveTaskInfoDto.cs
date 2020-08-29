namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.Task
{
    public class RespReceiveTaskInfoDto
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string MaterialUrl { get; set; }
        public string BillingRuleName { get; set; }
        public string ImgUrl { get; set; }
        public string Synopsis { get; set; }
        public decimal CPCPrice { get; set; }
        public decimal CPLPrice { get; set; }
        public decimal TaskAmount { get; set; }
        public int RuleCount { get; set; }
        public int TakeCount { get; set; }
    }
}
