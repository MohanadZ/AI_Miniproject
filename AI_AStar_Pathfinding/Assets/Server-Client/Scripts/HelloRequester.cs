using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;
using System.Linq;

public class HelloRequester : RunAbleThread
{
    public static int[] algorithmPath;
    public string messageToSend;
    
    protected override void Run()
    {
        ForceDotNet.Force(); 

        using (RequestSocket client = new RequestSocket())
        {
            //Port number for the first client in the TCP connection
            client.Connect("tcp://localhost:5555");

            while(Running)
            {
                if (Send)
                {
                    //The start and end positions and the grid are sent over the socket
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
                        //Remove unnecessary characters from the path received from the server
                        var numbers = message.Replace("(", "").Replace(",", "").Replace(")", "");

                        //Split the string path whenever a space is encountered,
                        //then convert it into integers and save them in an integer array
                        algorithmPath = numbers.Split(' ').Select(int.Parse).ToArray();
                    }
                }       
            }
        }

        NetMQConfig.Cleanup();
    }
}