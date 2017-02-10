using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace DatabaseUtilities
{
    public class DatabaseEnumerator
    {
        public string displaythis = string.Empty;

        public DatabaseEnumerator()
        { }

        /// <summary>
        /// Enumerates all SQL Server instances on the machine.
        /// </summary>
        /// <returns></returns>
        public bool EnumerateSQLInstances()
        {
            string correctNamespace = GetCorrectWmiNameSpace();
            if (string.Equals(correctNamespace, string.Empty))
            {
                return false;
            }
            string query = string.Format("select * from SqlServiceAdvancedProperty where SQLServiceType = 1 and PropertyName = 'instanceID'");
            ManagementObjectSearcher getSqlEngine = new ManagementObjectSearcher(correctNamespace, query);
            if (getSqlEngine.Get().Count == 0)
            {
                return false;
            }

            displaythis = "SQL Server database instances discovered :\r\n";
            //Console.WriteLine("SQL Server database instances discovered :");
            string instanceName = string.Empty;
            string serviceName = string.Empty;
            string version = string.Empty;
            string edition = string.Empty;

            displaythis += "Instance Name \t ServiceName \t Edition \t Version \t\r\n";
            //Console.WriteLine("Instance Name \t ServiceName \t Edition \t Version \t");
            foreach (ManagementObject sqlEngine in getSqlEngine.Get())
            {
                serviceName = sqlEngine["ServiceName"].ToString();
                instanceName = GetInstanceNameFromServiceName(serviceName);
                version = GetWmiPropertyValueForEngineService(serviceName, correctNamespace, "Version");
                edition = GetWmiPropertyValueForEngineService(serviceName, correctNamespace, "SKUNAME");

                displaythis += instanceName + "\t" + serviceName + "\t" + edition + "\t" + version + "\r\n";
                //Console.Write("{0} \t", instanceName);
                //Console.Write("{0} \t", serviceName);
                //Console.Write("{0} \t", edition);
                //Console.WriteLine("{0} \t", version);
            }

            return true;
        }

        /// <summary>
        /// Method returns the correct SQL namespace to use to detect SQL Server instances.
        /// </summary>
        /// <returns>namespace to use to detect SQL Server instances</returns>
        public static string GetCorrectWmiNameSpace()
        {
            String wmiNamespaceToUse = "root\\Microsoft\\sqlserver";
            List<string> namespaces = new List<string>();
            try
            {
                // Enumerate all WMI instances of
                // __namespace WMI class.
                ManagementClass nsClass =
                    new ManagementClass(
                    new ManagementScope(wmiNamespaceToUse),
                    new ManagementPath("__namespace"),
                    null);
                foreach (ManagementObject ns in nsClass.GetInstances())
                {
                    namespaces.Add(ns["Name"].ToString());
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine("Exception = " + e.Message);
            }
            if (namespaces.Count > 0)
            {
                if (namespaces.Contains("ComputerManagement10"))
                {
                    //use katmai+ namespace
                    wmiNamespaceToUse = wmiNamespaceToUse + "\\ComputerManagement10";
                }
                else if (namespaces.Contains("ComputerManagement"))
                {
                    //use yukon namespace
                    wmiNamespaceToUse = wmiNamespaceToUse + "\\ComputerManagement";
                }
                else if (namespaces.Contains("ComputerManagement11"))
                {
                    //use v11 namespace
                    wmiNamespaceToUse = wmiNamespaceToUse + "\\ComputerManagement11";
                }
                else if (namespaces.Contains("ComputerManagement12"))
                {
                    //use v12 namespace
                    wmiNamespaceToUse = wmiNamespaceToUse + "\\ComputerManagement12";
                }
                else if (namespaces.Contains("ComputerManagement13"))
                {
                    //use v13 namespace
                    wmiNamespaceToUse = wmiNamespaceToUse + "\\ComputerManagement13";
                }
                else
                {
                    wmiNamespaceToUse = string.Empty;
                }
            }
            else
            {
                wmiNamespaceToUse = string.Empty;
            }
            return wmiNamespaceToUse;
        }

        /// <summary>
        /// method extracts the instance name from the service name
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static string GetInstanceNameFromServiceName(string serviceName)
        {
            if (!string.IsNullOrEmpty(serviceName))
            {
                if (string.Equals(serviceName, "MSSQLSERVER", StringComparison.OrdinalIgnoreCase))
                {
                    return serviceName;
                }
                else
                {
                    return serviceName.Substring(serviceName.IndexOf('$') + 1, serviceName.Length - serviceName.IndexOf('$') - 1);
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the WMI property value for a given property name for a particular SQL Server service Name
        /// </summary>
        /// <param name="serviceName">The service name for the SQL Server engine service to query for</param>
        /// <param name="wmiNamespace">The wmi namespace to connect to </param>
        /// <param name="propertyName">The property name whose value is required</param>
        /// <returns></returns>
        public static string GetWmiPropertyValueForEngineService(string serviceName, string wmiNamespace, string propertyName)
        {
            string propertyValue = string.Empty;
            string query = String.Format("select * from SqlServiceAdvancedProperty where SQLServiceType = 1 and PropertyName = '{0}' and ServiceName = '{1}'", propertyName, serviceName);
            ManagementObjectSearcher propertySearcher = new ManagementObjectSearcher(wmiNamespace, query);
            foreach (ManagementObject sqlEdition in propertySearcher.Get())
            {
                propertyValue = sqlEdition["PropertyStrValue"].ToString();
            }
            return propertyValue;
        }
    }
}


