namespace XYAuto.BUOC.BOP2017.Infrastruction.ErrorException
{
    public class ErrorCodeMap
    {
        /* Exception */
        public const int SystemException = -99;
        public const string SystemExceptionMsg = "系统内部错误";
        public const int NotSourceIdException = -100;

        //public const string NotSourceIdExceptionMsg = "";
        public const int ApiParamsException = -101;

        public const string ExportBusinessTypeExceptionMsg = "物料分发-导出数据业务类型错误";
    }
}