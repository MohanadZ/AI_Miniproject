using UnityEngine;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;

public class HelloClient2 : MonoBehaviour
{
    private HelloRequester2 _helloRequester;
    public bool SendPack = true;

    public GameObject seekerPosition, targetPosition;

    void Update()
    {
        if (SendPack)
        {
            string startNodeCoordinates = Mathf.RoundToInt(seekerPosition.transform.position.z).ToString() + " " + Mathf.RoundToInt(seekerPosition.transform.position.x).ToString();

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
        _helloRequester = new HelloRequester2();
        _helloRequester.Start();
    }

    private void OnDestroy()
    {
        _helloRequester.Stop();
    }
}