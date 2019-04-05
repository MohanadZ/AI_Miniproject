using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFindingAlgorithm : MonoBehaviour
{
    public Transform seeker, target, seeker2;

    GridPathFinding grid;
    List<Node> path = new List<Node>();
    List<Node> path2 = new List<Node>();

    List<int> nodeX = new List<int>();
    List<int> nodeY = new List<int>();

    List<int> nodeX2 = new List<int>();
    List<int> nodeY2 = new List<int>();

    Vector3 currentTile;
    Vector3 currentTile2;
    Vector3 targetPos;
    int targetIndex = 0;
    int targetIndex2 = 0;

    bool test = false;
    bool test1 = false;

    void Awake()
    {
        grid = GetComponent<GridPathFinding>();

        currentTile = seeker.transform.position;
        currentTile2 = seeker2.transform.position;
    }

    private void Start()
    {
        target.hasChanged = false;
    }

    void Update()
    {
        firstSeekerPath();
        //secondSeekerPath();
    }

    void firstSeekerPath()
    {
        if (target.hasChanged)
        {
            test1 = false;           
            currentTile = seeker.transform.position;
            nodeX.Clear();
            nodeY.Clear();
            path.Clear();

            target.hasChanged = false;
        }

        if (HelloRequester.algorithmPath.Length != 0)
        {
            if (test1 == false)
            {
                test1 = true;
                for (int i = 0; i < HelloRequester.algorithmPath.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        nodeY.Add(Mathf.Abs(HelloRequester.algorithmPath[i] - 32));
                        nodeX.Add(HelloRequester.algorithmPath[i + 1]);
                    }
                }

                for (int j = 0; j < nodeX.Count; j++)
                {
                    Node tile = grid.NodeFromWorldPoint(new Vector3(nodeX[j], 0.0f, nodeY[j]));

                    path.Add(tile);
                }

                targetPos = path[path.Count - 1].worldPosition;
                
                grid.path = path;
            }

            //Moving Seeker
            if (path != null)
            {
               
                if (seeker.transform.position == currentTile)
                {
                    if (targetIndex < path.Count)
                    {
                        targetIndex++;

                        if (targetIndex >= path.Count)
                        {

                            targetIndex = 0;
                        }

                        currentTile = path[targetIndex].worldPosition;

                    }
                }

                if (seeker.transform.position != targetPos)
                {
                    seeker.transform.position = Vector3.MoveTowards(seeker.transform.position, currentTile, 0.1f);
                }
                
            }
        }
    }

    void secondSeekerPath()
    {
        nodeX2.Clear();
        nodeY2.Clear();

        path2.Clear();

        for (int i = 0; i < HelloRequester2.secondSeekerPath.Length; i++)
        {
            if (i % 2 == 0)
            {
                nodeY2.Add(Mathf.Abs(HelloRequester2.secondSeekerPath[i] - 32));
                nodeX2.Add(HelloRequester2.secondSeekerPath[i + 1]);
            }
        }

        //print("Test this now " + HelloRequester2.secondSeekerPath.Length);

        for (int j = 0; j < nodeX2.Count; j++)
        {
            Node tile2 = grid.NodeFromWorldPoint(new Vector3(nodeX2[j], 0.0f, nodeY2[j]));

            path2.Add(tile2);
        }

        grid.path2 = path2;

        //print("path2 count is " + path2.Count);
        //print("grid path count is " + grid.path.Count);

        //Moving Seeker
        if (path2 != null)
        {
            //while (true)
            //{
            if (seeker2.transform.position == currentTile2)
            {
                if (targetIndex2 < path2.Count)
                {
                    targetIndex2++;

                    if (targetIndex2 >= path2.Count)
                    {
                        targetIndex2 = 0;
                    }
                    currentTile2 = path2[targetIndex2].worldPosition;
                }
            }
            //print("this is " + targetIndex);
            seeker2.transform.position = Vector3.MoveTowards(seeker2.transform.position, currentTile2, 0.1f);
            //}
        }
    }
}
