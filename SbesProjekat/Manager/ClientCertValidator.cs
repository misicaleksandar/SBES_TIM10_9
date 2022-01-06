using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class ClientCertValidator : X509CertificateValidator
	{
		/// <summary>
		/// Implementation of a custom certificate validation on the client side.
		/// Client should consider certificate valid if the given certifiate is not self-signed.
		/// If validation fails, throw an exception with an adequate message.
		/// </summary>
		/// <param name="certificate"> certificate to be validate </param>
		public override void Validate(X509Certificate2 certificate)
		{
			StreamReader sr = new StreamReader("PubSubEngineName.txt");
			string srvCertCN = sr.ReadLine();

			//string srvCertCN = File.ReadAllLines("PubSubEngineName.txt")[0];

			if (!certificate.Subject.Equals(certificate.Issuer))
			{
				throw new Exception("Serverski sertifikat nije self-signed.");
			}
            if (!certificate.SubjectName.Name.Equals("CN=" + srvCertCN))
            {
				throw new Exception("CN nije odgovarajuci.");
			}
		}

	}
}
