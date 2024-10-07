using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

//namespace INA_ACS_Server
//{
//    class TcpMonitoring
//    {
//        private async void btnTest_Click(object sender, EventArgs e)
//        {
//            int timeOut = 2000;

//            while (true)
//            {
//                using (TcpClient client = new TcpClient())
//                {
//                    var ca = client.ConnectAsync("127.0.0.1", 9999);
//                    await Task.WhenAny(ca, Task.Delay(timeOut));
//                    client.Close();
//                    if (ca.IsFaulted || !ca.IsCompleted)
//                        //listBox1.Items.Add($"{DateTime.Now.ToString()} Server offline.");
//                        Console.WriteLine($"{DateTime.Now} Server offline.");
//                    else
//                        //listBox1.Items.Add($"{DateTime.Now.ToString()} Server available.");
//                        Console.WriteLine($"{DateTime.Now} Server available.");
//                }
//                // Wait 1 second before trying the test again
//                await Task.Delay(1000);
//            }
//        }
//    }
//}

public static class HelperMethods
{
    public static int ReadExactly(this Socket _socket, byte[] buffer, int offset, int size)
    {
        int totalRead = 0;
        int readRemains = size;

        while (true)
        {
            int readLen = 0;

            try
            {
                readLen = _socket.Receive(buffer, offset, readRemains, SocketFlags.None);
            }
            catch (SocketException ex) { }

            if (readLen == 0)
            {
                _socket.Close();
                break;
            }

            totalRead += readLen;

            if (totalRead >= size)
            {
                break;
            }

            offset += readLen;
            readRemains -= readLen;
        }

        return totalRead;
    }

    public static void TestReadExactly()
    {
        Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        byte[] buf = new byte[4];
        socket.ReadExactly(buf, 0, 4);

        int len = BitConverter.ToInt32(buf, 0);
        buf = new byte[len];
        socket.ReadExactly(buf, 0, len);
    }
}