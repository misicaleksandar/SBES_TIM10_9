using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class Audit : IDisposable
    {
		private static EventLog customLog = null;
		const string SourceName = "PubSub.Audit";
		const string LogName = "PubSub";

		static Audit()
		{
			try
			{
				if (!EventLog.SourceExists(SourceName))
				{
					EventLog.CreateEventSource(SourceName, LogName);
				}
				customLog = new EventLog(LogName, Environment.MachineName, SourceName);
			}
			catch (Exception e)
			{
				customLog = null;
				Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
			}
		}


		public static void NewDataStored(string timestamp, string dbName, int id, string digitalSignature, string publicKey)
		{
			if (customLog != null)
			{
				string message = String.Format("Timestamp: {0}, File: {1}, ID: {2}, Publisher's digital signature: {3}, Publisher's public key: {4}", timestamp, dbName, id, digitalSignature, publicKey);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event to event log."));
			}
		}

		public void Dispose()
		{
			if (customLog != null)
			{
				customLog.Dispose();
				customLog = null;
			}
		}
	}
}
