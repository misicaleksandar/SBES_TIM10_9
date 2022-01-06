using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscriber
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SubscriberForEngine : ISubscriberForEngine
    {
        public void SendDataToSubscriber(string alarm, byte[] sign, byte[] publisherName)
        {
            string startupPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName, "keySubEng.txt");


            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            X509Certificate2 subscriberCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)subscriberCert.PrivateKey;
            byte[] publisherNameBytes = csp.Decrypt(publisherName, false);

            UnicodeEncoding encoding = new UnicodeEncoding();
            string publisherNamee = encoding.GetString(publisherNameBytes);


            string publisherNameSign = publisherNamee + "_sign";
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,
                StoreLocation.LocalMachine, publisherNameSign);

            X509Certificate2 certificate2 = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,
                StoreLocation.LocalMachine, publisherNamee);


            string decryptedAlarm = AES.Decryption.DecryptString(alarm, AES.SecretKey.LoadKey(startupPath));


            if (DigitalSignature.Verify(decryptedAlarm, Manager.HashAlgorithm.SHA1, sign, certificate))
            {
                Console.WriteLine("Sign is valid");
                Console.WriteLine(decryptedAlarm);

                try
                {
                    int count;
                    try
                    {
                        count = File.ReadAllLines("database.txt").Length;
                    }
                    catch (FileNotFoundException)
                    {
                        count = 0;
                    }

                    StreamWriter sw = new StreamWriter("database.txt", true);
                    sw.WriteLine("ID: {0} " + decryptedAlarm.ToString(), count + 1);
                    sw.Close();

                    try
                    {
                        UnicodeEncoding encodingg = new UnicodeEncoding();
                        string str = encodingg.GetString(sign);
                        Audit.NewDataStored(DateTime.Now.ToString(), "database.txt", count + 1, str, certificate2.GetPublicKeyString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            else
            {
                Console.WriteLine("Sign is invalid");
            }

        }
    }
}
