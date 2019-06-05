using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;
using System.Linq;

public class HelloRequester2 : RunAbleThread
{
    public static int[] secondSeekerPath;
    public string messageToSend;

    protected override void Run()
    {
        ForceDotNet.Force();

        using (RequestSocket client = new RequestSocket())
        {
            //Port number for the second client in the TCP connection
            client.Connect("tcp://localhost:5444");

            while (Running)
            {
                if (Send)
                {
                    client.SendFrame(messageToSend);

                    string message = null;
                    bool gotMessage = false;

                    while (Running)
                    {
                        gotMessage = client.TryReceiveFrameString(out message); // this returns true if it's successful
                        if (gotMessage) break;
                    }
                    if (gotMessage)
                    {
                        var numbers = message.Replace("(", "").Replace(",", "").Replace(")", "");
                        secondSeekerPath = numbers.Split(' ').Select(int.Parse).ToArray();
                    }
                }
            }
        }

        NetMQConfig.Cleanup();
    }
}
