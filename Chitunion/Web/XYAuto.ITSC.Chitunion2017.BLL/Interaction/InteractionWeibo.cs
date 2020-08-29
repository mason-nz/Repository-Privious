using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Interaction
{
    public class InteractionWeibo
    {
        #region Instance
        public static readonly InteractionWeibo Instance = new InteractionWeibo();
        #endregion

        #region Contructor
        protected InteractionWeibo()
        { }
        #endregion

        public Entities.Interaction.InteractionWeibo GetEntity(int mediaId)
        {
            return Dal.Interaction.InteractionWeibo.Instance.GetEntity(mediaId);
        }

        public int Insert(Entities.Interaction.InteractionWeibo entity)
        {
            return Dal.Interaction.InteractionWeibo.Instance.Insert(entity);
        }

        public int Update(Entities.Interaction.InteractionWeibo entity)
        {
            return Dal.Interaction.InteractionWeibo.Instance.Update(entity);
        }
    }
}
