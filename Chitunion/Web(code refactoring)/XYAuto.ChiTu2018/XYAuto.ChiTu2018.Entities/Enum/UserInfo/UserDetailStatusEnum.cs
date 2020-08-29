/********************************
* 项目名称 ：XYAuto.ChiTu2018.Entities.Enum.UserInfo
* 类 名 称 ：UserDetailStatusEnum
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/24 15:35:31
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.Enum.UserInfo
{
    public enum UserDetailStatusEnum
    {
        未认证 = 0,
        待审核,
        已认证,
        认证未通过
    }
}
