using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    public class ValidateDataFormat
    {

        public void Validate(string jsonData, out string msg)
        {
            msg = string.Empty;

            ValidateData vdInfo = new ValidateData();
            vdInfo = (ValidateData)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(jsonData, typeof(ValidateData));

            if (vdInfo == null)
            {

            }

            List<ControlInfo> cInfo = vdInfo.ControlInfo;

            foreach (ControlInfo ci in cInfo)
            {
                msg += ErrorMsg(ci);
            }
             
        }
        private string ErrorMsg(ControlInfo ci)
        {
            string errorMsg = string.Empty;

            if (ci.Value == "undefined")
            {
                return "";
            }

            //如果有验证信息
            string[] array_types = ci.VType.Split('|'); //验证类型 格式：isNull|isTel
            string[] array_vmsg = ci.VMsg.Split('|'); //提示信息 格式：不为空|号码有误             
            for (var k = 0; k < array_types.Length; k++)
            {
                string type = array_types[k];

                string vmsg = array_vmsg[k];

                switch (type)
                {
                    case "isNull": if (ci.Value == "")
                        {
                            errorMsg += vmsg + "<br/>"; //验证不能为空
                        }
                        break;

                    case "Len":
                        string leng = ci.MaxLen;
                        if (int.Parse(leng) < GetLength(ci.Value))
                        {
                            errorMsg += vmsg + "<br/>"; //验证长度
                        }
                        break;

                    case "notFirstOption":
                        if (int.Parse(ci.OptionLen) > 1 && ci.Value == ci.FirstOptionVal)
                        {
                            errorMsg += vmsg + "<br/>"; //验证下拉列表，不能选择第一个选项
                        }
                        break;

                    case "checkNum": if (ci.Value != "" && !CheckNum(ci.Value))
                        {
                            errorMsg += vmsg + "<br/>"; //验证是否为数字
                        }
                        break;

                    case "isNum": if (ci.Value != "" && !isNum(ci.Value))
                        {
                            errorMsg += vmsg + "<br/>"; //验证是否为纯数字
                        }
                        break;

                    //case "isMoney":
                    //    if (value != "")
                    //    {
                    //        if (!isMoney(value))
                    //        {
                    //            errorMsg += vmsg + "<br/>"; //(checkNum一样)验证数字(包括整数、浮点数)
                    //        }
                    //    }
                    //    break;

                    //case "isMoneyAbs":
                    //    if (value != "")
                    //    {
                    //        if (!CheckNum(Math.Abs(decimal.Parse(value))))
                    //        {
                    //            errorMsg += vmsg + "<br/>"; //验证绝对值（即可以为负数）的数字格式(包括整数、浮点数)
                    //        }
                    //    }
                    //    break;

                    case "isFloat": if (ci.Value != "" && !isFloat(ci.Value))
                        {
                            errorMsg += vmsg + "<br/>"; //验证浮点
                        }
                        break;

                    case "isDate":
                        if (ci.Value != "" && !isDate(ci.Value))
                        {
                            errorMsg += vmsg + "<br/>"; //验证日期格式
                        }
                        break;
                    case "isDateTime":
                        if (ci.Value != "" && !isDate(ci.Value))
                        {
                            errorMsg += vmsg + "<br/>"; //验证时间格式
                        }
                        break;

                    case "isMobile": if (ci.Value != "" && !isMobile(ci.Value))
                        {
                            errorMsg += vmsg + "<br/>"; //验证手机格式
                        }
                        break;

                    case "isTel": if (ci.Value != "" && !isTel(ci.Value))
                        {
                            errorMsg += vmsg + "<br/>"; //验证电话格式
                        }
                        break;

                    case "isTelOrMobile": if (ci.Value != "" && !isTelOrMobile(ci.Value))
                        {
                            errorMsg += vmsg + "<br/>"; //验证电话或者手机验证
                        }
                        break;

                    case "isEmail": if (ci.Value != "" && !isEmail(ci.Value))
                        {
                            errorMsg += vmsg + "<br/>"; //验证邮件
                        }
                        break;

                    case "checkIdcard": if (ci.Value != "" && !CheckIDCard(ci.Value))
                        {
                            errorMsg += vmsg + "<br/>"; //验证身份证
                        }
                        break;
                }
            }

            return errorMsg;
        }

        public bool CheckNum(string val)
        {
            string re = @"^-?[1-9]+(\.\d+)?$|^-?0(\.\d+)?$|^-?[1-9]+[0-9]*(\.\d+)?$";
            return Regex.IsMatch(val, re);
        }

        public bool isNum(string val)
        {
            string re = @"^[0-9]*$";
            return Regex.IsMatch(val, re);
        }

        public bool isMoney(string val)
        {
            string re = @"^[0-9]*$";
            return Regex.IsMatch(val, re);
        }

        public bool isFloat(string val)
        {
            string re = @"^[0-9]+.?[0-9]*$";
            return Regex.IsMatch(val, re);
        }

        public bool isDate(string val)
        {
            DateTime shortTime;
            if (DateTime.TryParse(val, out shortTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isMobile(string val)
        {
            string re = @"^(?:13\d|14\d|17\d|15\d|18\d|19\d|14\d)-?\d{5}(\d{3}|\*{3})$";
            return Regex.IsMatch(val, re);
        }

        public bool isTel(string val)
        {
            string re = @"^(([0\+]\d{2,3})?(0\d{2,3}))(\d{7,8})$";
            return Regex.IsMatch(val, re);
        }

        public bool isTelOrMobile(string val)
        {
            return (isTel(val) || isMobile(val));
        }

        public bool isEmail(string val)
        {
            string re = @"^(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)$";
            return Regex.IsMatch(val, re);
        }



        /// <summary>   
        /// 得到字符串的长度，一个汉字算2个字符   
        /// </summary>   
        /// <param name="str">字符串</param>   
        /// <returns>返回字符串长度</returns>   
        public int GetLength(string str)
        {
            if (str.Length == 0) return 0;

            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }

            return tempLen;
        }   



        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns></returns>
        public bool CheckIDCard(string Id)
        {
            if (Id.Length == 18)
            {
                bool check = CheckIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = CheckIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 18位身份证验证
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns></returns>
        private bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }
        /// <summary>
        /// 15位身份证验证
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns></returns>
        private bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }

    }
}