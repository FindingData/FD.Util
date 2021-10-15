using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Network
{
    public class MacHelper
    {
        public static string GetMacAddress()
        {
            string macAddresses = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces, thereby ignoring any
                // loopback devices etc.
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet) continue;
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            if(macAddresses.Length == 12)
            {
                macAddresses = string.Format("{0}-{1}-{2}-{3}-{4}-{5}",
                          macAddresses.Substring(0, 2), macAddresses.Substring(2, 2), macAddresses.Substring(4, 2),
                          macAddresses.Substring(6, 2), macAddresses.Substring(8, 2), macAddresses.Substring(10, 2));
            }
            return macAddresses;
        }
    }
}
