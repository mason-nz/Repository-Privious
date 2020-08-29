using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.Utils;

namespace XYAuto.ITSC.Chitunion2017.Entities.LabelTask
{
    public class ENUM
    {
        /// <summary>
        /// 文章媒体类型
        /// </summary>
        public enum EnumResourceType
        {
            微信 = 1,
            汽车之家,
            今日头条,
            网易汽车,
            行圆新闻后台,
            搜狐
        }
        /// <summary>
        /// 标签媒体类型
        /// </summary>
        public enum EnumMediaType
        {
            微信 = 14001,
            APP = 14002,
            微博 = 14003,
            视频 = 14004,
            直播 = 14005,
            头条 = 14006
        }        
        /// <summary>
        /// 标签项目类型
        /// </summary>
        public enum EnumProjectType
        {
            媒体 = 61001,
            文章 = 61002
        }
        /// <summary>
        /// 标签项目状态
        /// </summary>
        public enum EnumProjectStatus
        {
            待执行 = 62001,
            执行中 = 62002,
            执行完毕 = 62003,
            已停止 = 62004,
            删除 = -1
        }
        /// <summary>
        /// 标签任务状态
        /// </summary>
        public enum EnumTaskStatus
        {
            待领 = 63001,
            已领 = 63002,
            待打标签 = 63003,
            已打标签 = 63004,
            待审 = 63005,
            已审 = 63006
        }
        /// <summary>
        /// 标签任务操作日志状态
        /// </summary>
        public enum EnumTaskOptType
        {
            提交 = 64001,
            审核 = 64002,
            修改 = 64003
        }
        /// <summary>
        /// 标签类型
        /// </summary>
        public enum EnumLableType
        {
            分类 = 65001,
            场景 = 65002,
            IP = 65003,
            标签 = 65004,
            子IP=65005
        }
    }
}
