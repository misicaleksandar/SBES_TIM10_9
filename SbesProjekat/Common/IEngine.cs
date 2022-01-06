using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IEngine
    {
        [OperationContract]
        void Subscribe(string alarmTypes, string clientAddress);

        [OperationContract]
        void SendDataToEngine(string alarm, byte[] sign);

        [OperationContract]
        void Unsubscribe(string clientAddress);
    }
}
