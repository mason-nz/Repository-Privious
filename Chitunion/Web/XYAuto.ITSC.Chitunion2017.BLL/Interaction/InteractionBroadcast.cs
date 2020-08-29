using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Interaction
{
    public class InteractionBroadcast
    {
        #region Instance
        public static readonly InteractionBroadcast Instance = new InteractionBroadcast();
        #endregion

        #region Contructor
        protected InteractionBroadcast()
        { }
        #endregion

        public Entities.Interaction.InteractionBroadcast GetEntity(int mediaId)
        {
            return Dal.Interaction.InteractionBroadcast.Instance.GetEntity(mediaId);
        }

        public int Insert(Entities.Interaction.InteractionBroadcast entity)
        {
            return Dal.Interaction.InteractionBroadcast.Instance.Insert(entity);
        }

        public int Update(Entities.Interaction.InteractionBroadcast entity)
        {
            return Dal.Interaction.InteractionBroadcast.Instance.Update(entity);
        }
    }
}
