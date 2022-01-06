using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    public class ClientProxy : ChannelFactory<IEngine>, IDisposable
    {
        IEngine factory;

        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            //Credentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            factory = this.CreateChannel();

        }

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);



            factory = this.CreateChannel();
            //Credentials.Windows.AllowNtlm = false;
        }

        

        public void Subscribe(string alarmTypes, string clientAddress)
        {
            try
            {
                factory.Subscribe(alarmTypes, clientAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
        }

        public void Unsubscribe(string clientAddress)
        {
            try
            {
                factory.Unsubscribe(clientAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
        }


    }
}
