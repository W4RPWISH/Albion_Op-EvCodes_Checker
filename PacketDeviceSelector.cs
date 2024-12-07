using Albion.Network;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace OpEvChecker
{
    public class PacketDeviceSelector
    {
        private readonly IPhotonReceiver photonReceiver;
    
        public PacketDeviceSelector(IPhotonReceiver photonReceiver) => this.photonReceiver = photonReceiver;
    
        public void Start()
        {
            LibPcapLiveDeviceList instance = LibPcapLiveDeviceList.Instance;

            foreach (LibPcapLiveDevice device in instance)
            {
                device.Open(DeviceModes.Promiscuous, 10);
                device.Filter = "udp port 5056";
                device.OnPacketArrival += new PacketArrivalEventHandler(PacketHandler);
                device.StartCapture();
                break;
            }
        }
    
        private void PacketHandler(object sender, PacketCapture e)
        {
            RawCapture packet = e.GetPacket();
            UdpPacket udpPacket = Packet.ParsePacket(packet.LinkLayerType, packet.Data).Extract<UdpPacket>();

            if (udpPacket == null)
              return;

            try
            {
                photonReceiver.ReceivePacket(udpPacket.PayloadData);
            }
            catch { }
        }
    }
}
