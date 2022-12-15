using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using static WorkOrderUpdaterServer.Logger;

var host = Dns.GetHostEntry(Dns.GetHostName());
var address = host.AddressList[0];
var localEndPoint = new IPEndPoint(address, 7240);
var listener = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
listener.Bind(localEndPoint);
listener.Listen(100);


Log($"Host: {address}");
Log("Port: 7240");
try
{
    while(true)
    {
        var client = await listener.AcceptAsync();
        Log($"Request sent by {client.RemoteEndPoint}");
        try
        {
            var controller = new ServiceController("COSSERP_Enterprise_Manager_x64");
            controller.Stop();
            controller.WaitForStatus(ServiceControllerStatus.Stopped);
            controller.Start();
            Log("COSS ERP Manager restarted!");
            var message = Encoding.ASCII.GetBytes("SUCCESS");
            client.Send(message);
        }
        catch(Exception e)
        {
            try
            {
                Log("Could not restart the COSS ERP Manager");
                Log(e.Message);
            } catch { }
            var message = Encoding.ASCII.GetBytes("FAILURE");
            client.Send(message);
        }
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }
}
catch(Exception e)
{
    try
    {
        Log("Internal Server Error!");
        Log(e.Message);
    }
    catch { }
}