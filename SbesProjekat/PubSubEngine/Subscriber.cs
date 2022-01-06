using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubEngine
{
    public class Subscriber
    {
        List<AlarmType> alarms;
        ClientProxy proxy;
        string subscriberName;

        public Subscriber(List<AlarmType> alarms, ClientProxy proxy, string subscriberName)
        {
            this.Alarms = alarms;
            this.Proxy = proxy;
            this.subscriberName = subscriberName;
        }

        public List<AlarmType> Alarms { get => alarms; set => alarms = value; }
        public ClientProxy Proxy { get => proxy; set => proxy = value; }
        public string SubscriberName { get => subscriberName; set => subscriberName = value; }
    }
}
