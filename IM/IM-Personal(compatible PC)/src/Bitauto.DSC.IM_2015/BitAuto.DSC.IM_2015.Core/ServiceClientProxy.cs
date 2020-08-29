using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel;
using System.Text;
using BitAuto.DSC.IM_2015.MainInterface;
using BitAuto.DSC.IM_2015.MainInterface;

namespace BitAuto.DSC.IM_2015.Core
{

    public class ServiceClientProxy : RealProxy
    {
        public IIMServices Channel { get; private set; }

        private ICommunicationObject innerChannel;
        public ServiceClientProxy(object wcfClient, string endpointConfigName)
            : base(typeof(IIMServices))
        {
            DuplexChannelFactory<IIMServices> channelFactory = ChannelFactories.GetFactory(wcfClient, endpointConfigName);

            this.innerChannel = (ICommunicationObject)channelFactory.CreateChannel();
            this.Channel = (IIMServices)this.GetTransparentProxy();
        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage methodCall = (IMethodCallMessage)msg;
            object[] args = (object[])Array.CreateInstance(typeof(object), methodCall.ArgCount);
            methodCall.Args.CopyTo(args, 0);
            try
            {
                object ret = methodCall.MethodBase.Invoke(this.innerChannel, args);
                this.innerChannel.Close();
                return new ReturnMessage(ret, args, methodCall.ArgCount, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception ex)
            {
                Exception innerEx = ex.InnerException;
                if (null == innerEx)
                {
                    return new ReturnMessage(ex, methodCall);
                }
                if (innerEx is TimeoutException || innerEx is CommunicationException)
                {
                    this.innerChannel.Abort();
                }
                return new ReturnMessage(innerEx, methodCall);
            }
        }
    }


    public static class ChannelFactories
    {

        private static Dictionary<string, DuplexChannelFactory<IIMServices>> channelFactories = new Dictionary<string, DuplexChannelFactory<IIMServices>>();

        public static DuplexChannelFactory<IIMServices> GetFactory(object wcfContext, string endpointConfigName)
        {
            if (channelFactories.ContainsKey(endpointConfigName))
            {
                return channelFactories[endpointConfigName] as DuplexChannelFactory<IIMServices>;
            }

            lock (channelFactories)
            {
                if (channelFactories.ContainsKey(endpointConfigName))
                {
                    return channelFactories[endpointConfigName] as DuplexChannelFactory<IIMServices>; //ChannelFactory<TChannel>;
                }
                DuplexChannelFactory<IIMServices> channelFactory = new DuplexChannelFactory<IIMServices>(wcfContext, endpointConfigName);
                channelFactory.Open();
                channelFactories[endpointConfigName] = channelFactory;
                return channelFactory;
            }
        }
    }
}



