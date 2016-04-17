using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace AUTO_IPGW
{
    class NetworkAnalyzer
    {
        static ICaptureDevice device;



        public static void listen()
        {
            // Print SharpPcap version 
            string ver = SharpPcap.Version.VersionString;
            Console.WriteLine("SharpPcap {0}, Example1.IfList.cs", ver);

            // Retrieve the device list
            CaptureDeviceList devices = CaptureDeviceList.Instance;

            // If no devices were found print an error
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return;
            }

            Console.WriteLine("\nThe following devices are available on this machine:");
            Console.WriteLine("----------------------------------------------------\n");

            // Print out the available network devices
            foreach (ICaptureDevice dev in devices)
                Console.WriteLine("{0}\n", dev.ToString());

            //Console.Write("Hit 'Enter' to exit...");
            //Console.ReadLine();

            // Extract a device from the list
            device = devices[3];

            // Register our handler function to the
            // 'packet arrival' event
            device.OnPacketArrival +=
                new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);

            // Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

            //string filter = "ip and tcp";
            string filter = "ip";
            device.Filter = filter;


            Console.WriteLine("-- Listening on {0}, hit 'Enter' to stop...",
                device.Description);

            // Start the capturing process
            device.StartCapture();


            // Wait for 'Enter' from the user.
            Console.ReadLine();


        }

        public static void stopListen()
        {
            // Stop the capturing process
            device.StopCapture();

            // Close the pcap device
            device.Close();
        }



        /// <summary>
        /// Prints the time and length of each received packet
        /// </summary>
        private static void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            DateTime time = e.Packet.Timeval.Date;
            int len = e.Packet.Data.Length;
            //Console.WriteLine("{0}:{1}:{2},{3} Len={4}",
            //  time.Hour, time.Minute, time.Second, time.Millisecond, len);



            //// open the output file
            //string s = "cap.txt";
            //var captureFileWriter = new CaptureFileWriterDevice(e.Device, s);
            //// write the packet to the file
            //captureFileWriter.Write(e.Packet);
            //Console.WriteLine("Packet dumped to file.");

            var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data); //Raw基础包对象  
            var ipPacket = IpPacket.GetEncapsulated(packet);
            string srcIp = ipPacket.SourceAddress.ToString();
            string dstIp = ipPacket.DestinationAddress.ToString();

            if (ipPacket.Protocol == IPProtocolType.TCP)
            {
                Console.WriteLine(ipPacket.Protocol.ToString());
                var tcpPacket = (TcpPacket)ipPacket.Extract(typeof(TcpPacket));
                string srcport = tcpPacket.SourcePort.ToString();
                string dstPort = tcpPacket.DestinationPort.ToString();
                //if(srcIp == "192.168.1.108")
                Console.WriteLine(srcIp+":"+srcport + "\t>\t" + dstIp + ":" + dstPort);
            }



        }
    }
}
