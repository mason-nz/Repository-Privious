using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace BitAuto.DSC.IM_DMS2014.Entities.Messages
{
    [DataContract(Name = "sfm")]
    public class SendFileMessage
    {
        public SendFileMessage()
        {
            this.time = new DateTime(1900, 1, 1);

        }

        [DataMember(Name = "f")]
        private string from;
        [DataMember(Name = "m")]
        private string message;
        [DataMember(Name = "t")]
        private DateTime time;
        [DataMember(Name = "fz")]
        private long filesize;
        [DataMember(Name = "fn")]
        private string filename;
        [DataMember(Name = "ft")]
        private string filetype;
        [DataMember(Name = "fp")]
        private string filepath;
        //会话ID
        [DataMember(Name = "cs")]
        private int csid;
        //文件路径
        public string FilePath
        {
            get
            {
                return this.filepath;
            }
            set
            {
                this.filepath = value;
            }
        }
        //文件大小字节
        public long FileSize
        {
            get
            {
                return this.filesize;
            }
            set
            {
                this.filesize = value;
            }
        }

        //文件名称
        public string FileName
        {
            get
            {
                return this.filename;
            }
            set
            {
                this.filename = value;
            }

        }

        //文件类型
        public string FileType
        {
            get { return this.filetype; }
            set { this.filetype = value; }
        }

        public string From
        {
            get { return this.from; }
            set { this.from = value; }
        }

        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }
        public DateTime Time
        {
            get { return this.time; }
            set { this.time = value; }
        }
        //会话ID
        public int CsID
        {
            get { return this.csid; }
            set { this.csid = value; }
        }
    }
}
