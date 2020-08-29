namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.QrLogin
{
    public class ReqPostLoginQrDto
    {
        public LoginType LoginType { get; set; }

        public int ScenceId { get; set; }
    }
    public enum LoginType
    {
        None = -2,
        媒体主 = 1,
        广告主 = 2
    }
}