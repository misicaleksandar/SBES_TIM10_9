using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args) {

            string signCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name) + "_sign";

            string srvCertCN = "PubSubEngine";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Engine"),
                                      new X509CertificateEndpointIdentity(srvCert));


            ClientProxy proxy = new ClientProxy(binding, address);

            Random r = new Random();

            X509Certificate2 certificateSign = CertManager.GetCertificateFromStorage(StoreName.My,
                   StoreLocation.LocalMachine, signCertCN);

            string alarmMessageBase = AlarmMessage.GetAlarmMessage;

            while (true)
            {
                int risk = r.Next(1, 100);

                AlarmType alarmType = GetAlarmTypeForRisk(risk);

                string alarm = String.Format(alarmMessageBase, DateTime.Now, alarmType, risk);

                byte[] signature = DigitalSignature.Create(alarm, HashAlgorithm.SHA1, certificateSign);


                proxy.SendDataToEngine(alarm, signature);

                Thread.Sleep(5000);

            }

        }

        public static AlarmType GetAlarmTypeForRisk(int risk)
        {
            if (risk >= 0 && risk <= 20)
            {
                return AlarmType.NO_ALARM;
            }
            else if (risk >= 21 && risk <= 40)
            {
                return AlarmType.FALSE_ALARM;
            }
            else if (risk >= 41 && risk <= 60)
            {
                return AlarmType.INFO;
            }
            else if (risk >= 61 && risk <= 80)
            {
                return AlarmType.WARNING;
            }
            else
            {
                return AlarmType.ERROR;
            }
        }


    }
}
