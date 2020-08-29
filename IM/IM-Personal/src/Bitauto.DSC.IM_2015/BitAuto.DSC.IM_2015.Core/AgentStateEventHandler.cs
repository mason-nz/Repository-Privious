using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Core
{
    public delegate void AgentStateEventHandler(object sender, AgentStateEventArgs args);

    public class AgentStateEventArgs : EventArgs
    {
        private AgentStatusDetail agentState;

        public AgentStateEventArgs(AgentStatusDetail agentState)
        {
            this.agentState = agentState;
        }

        public AgentStatusDetail AgentState
        {
            get { return this.agentState; }
        }
    }
}
