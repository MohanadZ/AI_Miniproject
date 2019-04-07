using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;
using System.Linq;

public class HelloRequester3 : RunAbleThread
{
    public static int[] thirdSeekerPath; //= new int[64];

    public string messageToSend;

    protected override void Run()
    {
        ForceDotNet.Force();

        using (RequestSocket client = new RequestSocket())
        {
            client.Connect("tcp://localhost:5333");

            while (Running)
            {
                if (Send)
                {
                    //string message = client.ReceiveFrameString();

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
                        //Debug.Log("Received " + message);
                        //Convert the path received from python to a string, then to a integer array
                        var numbers = message.Replace("(", "").Replace(",", "").Replace(")", "");

                        thirdSeekerPath = numbers.Split(' ').Select(int.Parse).ToArray();
                    }
                }
            }
        }

        NetMQConfig.Cleanup();
    }
}
