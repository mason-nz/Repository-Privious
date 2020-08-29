namespace XYAuto.ITSC.Chitunion2017.BLL.Media.ErrorException
{
    public class NotFindMediaException : System.Exception
    {
        public NotFindMediaException(string message = "请传入合法的媒体类型")
            : base(message)
        {

        }
    }

    public class RequestParamsException: System.Exception
    {
        public RequestParamsException(string message = "请传入正确类型的参数")
            : base(message)
        {

        }
    }

    /*
     MediaErrorCode (1000-1999) MAX:
     
     
     */
}
