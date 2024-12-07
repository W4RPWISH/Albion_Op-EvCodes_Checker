using Albion.Network;

namespace OpEvChecker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PacketDeviceSelector packets = new PacketDeviceSelector(ReceiverBuilder.Create().Build());
            packets.Start();

            while (true) { }
        }
    }
}
