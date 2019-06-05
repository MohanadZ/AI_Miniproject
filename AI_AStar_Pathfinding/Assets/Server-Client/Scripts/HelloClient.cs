using UnityEngine;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;

public class HelloClient : MonoBehaviour
{
    private HelloRequester _helloRequester;
    public GameObject seekerPosition, targetPosition;
    public bool SendPack = true;

    void Update()
    {
        if (SendPack)
        {
            //The x and y coordinates of the first seeker (start position) and the target (end position)
            string startNodeCoordinates = Mathf.RoundToInt(seekerPosition.transform.position.z).ToString() + " " + Mathf.RoundToInt(seekerPosition.transform.position.x).ToString();

            string endNodeCoordinates = " " + Mathf.RoundToInt(targetPosition.transform.position.z).ToString() + " " + Mathf.RoundToInt(targetPosition.transform.position.x).ToString();

            //The grid tiles
            string gridArray = " " + GameObject.Find("AStar").GetComponent<GridPathFinding>().gridToArray;

            //The coordinates and grid are saved in a HelloRequester instance attribute
            _helloRequester.messageToSend = startNodeCoordinates + endNodeCoordinates + gridArray;

            _helloRequester.Continue();
        } else if (!SendPack)
        {
            _helloRequester.Pause();
        }
    }

    private void Start()
    {
        _helloRequester = new HelloRequester();
        _helloRequester.Start();
    }

    private void OnDestroy()
    {
        _helloRequester.Stop();
    }
}