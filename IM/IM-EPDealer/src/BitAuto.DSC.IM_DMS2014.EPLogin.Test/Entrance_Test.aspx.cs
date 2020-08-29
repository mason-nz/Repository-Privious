using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.BLL;
using BitAuto.Utils;

namespace BitAuto.DSC.IM_DMS2014.EPLogin.Test
{
    public partial class Entrance_Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Random r = new Random();
            if (!IsPostBack)
            {
                txtJson.Value = "{\r\n" +
                      "\"UserId\": 2784,\r\n" +
                      "\"UserName\": \"hello-hellokity@sina.com\",\r\n" +
                      "\"Mobile\": \"15001112627\",\r\n" +
                      "\"Email\": \"hello-hellokity@sina.com\",\r\n" +
                      "\"Post\": \"市场经理\",\r\n" +
                      "\"DealerId\": 50002218,\r\n" +   
                      "\"DateTimeFormat\": \""+DateTime.Now.ToString("yyyyMMddHHmmss")+"\",\r\n" +
                      "\"AppId\": \"E6803316-A286-4417-97BC-213F13973207\"\r\n" +
                "}";
                txtUserID.Value = r.Next(1000, 9999).ToString();
                txtMemberCode.Value = "50002218";
                btnChanged_Click(null, null);
            }

            /*
                {
                  "UserId": 2784,
                  "UserName": "hello-hellokity@sina.com",
                  "Mobile": "15001112627",
                  "Email": "hello-hellokity@sina.com",
                  "Post": "市场经理",
                  "DealerId": 50002218,
                  "DateTimeFormat": "20141029154346",
                  "AppId": "E6803316-A286-4417-97BC-213F13973207"
                }
             */
            //string key = "xxxx@#$&";
            ////string str = "2gs4bXNBnifgGr/8p0tqDwIkOtM4gxsHzYk7DXZwS37eiqsbCgvB5L9TBJ9193nJkKFttkgzXKgoIFcQoJjtLoSLwXVJeqrEmCIY9xN3TUFQr6QNLhZKMTE1gWzAdqaEKht9g/qx0oXklAY5IGmW0g4xcQEk2FSF0JTsBliHsCyENJs+HpjQ852CWLaCzNfDbHzQDc7NeYrpaeVnNjYbSkF5jiSTm2vziUsNW1F1GRssoX+CuQOYtSY3/fPVwWURnOl9q/W8aIaTukYOwh+dbyvMTwTUuJ/BJ9FOTZhwIXcgHKBZazvSDw==";
            //string str = "jrgBGXryHjbV4LGh1OvTvfcBO7tXw0PJYfESXE+yao1XPuEgtE1SnIec7NSDcqO/CnMmpPz6Zj05pm4x2n1ySsXeY6WV/c0aDKCUbVlZyQbsHN2Ql/xOMeWvkHZcNNjSoomnaL56be5FAFiANTCgNKLEx1/pB+QY81WnSWPq31BhBTjvnuMluLfb7+uM34Py5vCby4wArGmoDIpUIoS6CQBVFgAs1Lay+xkSZTYf8DWNtwFLALxj5dvvheqS0OQvlTYqSbEy6fdKy6GQWBO74w0Q2uHxftVnHWq9VyO8mZYjZpzoTOWjfXf5NLWEPAg1vXOUTOZ3YkQaNlNphMk0DYKjT1IeBPf3OpGkKFtFBGjZDFkSoju3cnLPh7FTQ8dEmOXVjUakZcQ3imNufu0EQ6KXjYoSiqaQ6G+wrhM6Ycttdc3QqRwk/CcsrvtmxlpJUUT7lxC64OI=";
            //EP_DESEncryptor code = new EP_DESEncryptor(key);
            //string eWord = code.DesDecrypt(str);

            //EPInfoEntity model = (EPInfoEntity)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(eWord, typeof(EPInfoEntity));

            //System.Console.WriteLine(model.IsValid);//测试易湃登录账号当前时间戳与当前时间间隔，是否在30分钟之内
            //System.Console.WriteLine(eWord);//输出解密后的json内容
        }

        protected void btnChanged_Click(object sender, EventArgs e)
        {
            txtJsonStr.Value = string.Empty;
            try
            {
                EP_DESEncryptor code = new EP_DESEncryptor(txtKey.Value.Trim());
                if (radioType1.Checked)//手动方式
                {
                    txtJsonStr.Value = code.DesEncrypt(txtJson.Value.Trim());
                }
                else if (radioType2.Checked)//自动方式
                {
                    txtJsonStr.Value = code.DesEncrypt("{\r\n" +
                      "\"UserId\": "+txtUserID.Value.Trim()+",\r\n" +
                      "\"UserName\": \"hello-hellokity@sina.com\",\r\n" +
                      "\"Mobile\": \"15001112627\",\r\n" +
                      "\"Email\": \"hello-hellokity@sina.com\",\r\n" +
                      "\"Post\": \"市场经理\",\r\n" +
                      "\"DealerId\": " + txtMemberCode.Value.Trim() + ",\r\n" +
                      "\"DateTimeFormat\": \""+DateTime.Now.ToString("yyyyMMddHHmmss")+"\",\r\n" +
                      "\"AppId\": \"E6803316-A286-4417-97BC-213F13973207\"\r\n" +
                      "}");
                }
                loginkey.Value = txtJsonStr.Value;
            }
            catch (Exception ex)
            {
                ScriptHelper.ShowAlertScript(this.Page,"易湃转换Json出错："+ex.Message);
                txtJsonStr.Value = string.Empty;
            }
        }
    }
}