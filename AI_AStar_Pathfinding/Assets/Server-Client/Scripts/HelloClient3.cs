using UnityEngine;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;

public class HelloClient3 : MonoBehaviour
{
    private HelloRequester3 _helloRequester;
    public bool SendPack = true;

    public GameObject seeker3Position, targetPosition;

    void Update()
    {
        if (SendPack)
        {
            string startNodeCoordinates = Mathf.RoundToInt(seeker3Position.transform.position.z).ToString() + " " + Mathf.RoundToInt(seeker3Position.transform.position.x).ToString();

            string endNodeCoordinates = " " + Mathf.RoundToInt(targetPosition.transform.position.z).ToString() + " " + Mathf.RoundToInt(targetPosition.transform.position.x).ToString();

            string gridArray = " " + GameObject.Find("AStar").GetComponent<GridPathFinding>().gridToArray;

            _helloRequester.messageToSend = startNodeCoordinates + endNodeCoordinates + gridArray;

            _helloRequester.Continue();
        }
        else if (!SendPack)
        {
            _helloRequester.Pause();
        }
    }

    private void Start()
    {
        _helloRequester = new HelloRequester3();
        _helloRequester.Start();
    }

    private void OnDestroy()
    {
        _helloRequester.Stop();
    }
}