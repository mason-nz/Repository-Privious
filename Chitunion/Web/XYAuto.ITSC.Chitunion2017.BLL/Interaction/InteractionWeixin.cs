using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Interaction
{
    public class InteractionWeixin
    {
        #region Instance
        public static readonly InteractionWeixin Instance = new InteractionWeixin();
        #endregion

        #region Contructor
        protected InteractionWeixin()
        { }
        #endregion

        public Entities.Interaction.InteractionWeixin GetEntity(int mediaId)
        {
            return Dal.Interaction.InteractionWeixin.Instance.GetEntity(mediaId);
        }
        public Entities.Interaction.InteractionWeixin GetEntityByWxID(int wxID)
        {
            return Dal.Interaction.InteractionWeixin.Instance.GetEntityByWxID(wxID);
        }

        public int Insert(Entities.Interaction.InteractionWeixin entity)
        {
            return Dal.Interaction.InteractionWeixin.Instance.Insert(entity);
        }

        public int Update(Entities.Interaction.InteractionWeixin entity)
        {
            return Dal.Interaction.InteractionWeixin.Instance.Update(entity);
        }
        public int UpdateByWxID(Entities.Interaction.InteractionWeixin entity)
        {
            return Dal.Interaction.InteractionWeixin.Instance.UpdateByWxID(entity);
        }
    }
}
