using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���BusinessGroupLineMapping ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2015-04-09 06:39:11 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class BusinessGroupLineMapping
    {

        public static readonly BusinessGroupLineMapping Instance = new BusinessGroupLineMapping();
        protected BusinessGroupLineMapping()
        { }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(Entities.BusinessGroupLineMapping model)
        {
            Dal.BusinessGroupLineMapping.Instance.Insert(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int bgid)
        {
            return Dal.BusinessGroupLineMapping.Instance.Delete(bgid);
        }
    }
}

