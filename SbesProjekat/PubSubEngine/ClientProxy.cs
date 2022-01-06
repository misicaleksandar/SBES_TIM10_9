using Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PubSubEngine
{
    public class ClientProxy : ChannelFactory<ISubscriberForEngine>, ISubscriberForEngine, IDisposable
    {
        ISubscriberForEngine factory;

        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            //Credentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            factory = this.CreateChannel();

        }

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
            //Credentials.Windows.AllowNtlm = false;
        }

        
        public void SendDataToSubscriber(string alarm, byte[] sign, byte[] publisherName)
        {
            try
            {
                factory.SendDataToSubscriber(alarm, sign, publisherName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
        }


    }
}
