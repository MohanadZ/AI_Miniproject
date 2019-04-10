using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFindingAlgorithm : MonoBehaviour
{
    public Transform seeker, target, seeker2, seeker3;

    GridPathFinding grid;
    List<Node> path = new List<Node>();
    List<Node> path2 = new List<Node>();
    List<Node> path3 = new List<Node>();

    List<int> nodeX = new List<int>();
    List<int> nodeY = new List<int>();

    List<int> nodeX2 = new List<int>();
    List<int> nodeY2 = new List<int>();

    List<int> nodeX3 = new List<int>();
    List<int> nodeY3 = new List<int>();

    Vector3 currentTile, currentTile2, currentTile3;
    Vector3 targetPos, targetPos2, targetPos3;
    int targetIndex = 0, targetIndex2 = 0, targetIndex3 = 0;
    bool trigger = true, trigger2 = true, trigger3 = true;

    Vector3 mousePos;

    void Awake()
    {
        grid = GetComponent<GridPathFinding>();

        currentTile = seeker.transform.position;
        currentTile2 = seeker2.transform.position;
        currentTile3 = seeker3.transform.position;
    }

    void Update()
    {
        targetPos2MousePos();
        firstSeekerPath();
        secondSeekerPath();
        thirdSeekerPath();
    }

    void targetPos2MousePos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 47.0f));
            //Debug.Log("Mouse Pos is " + mousePos);
            mousePos.y = 0;
            target.transform.position = mousePos;
            //Debug.Log("Target pos is " + target.transform.position);
        }
    }

    void firstSeekerPath()
    {
        if (seeker.transform.position == targetPos)
        {
            trigger = true;           
            currentTile = seeker.transform.position;
            targetIndex = 0;
            nodeX.Clear();
            nodeY.Clear();
            path.Clear();
        }

        if (HelloRequester.algorithmPath.Length != 0)
        {
            if (trigger == true)
            {
                trigger = false;
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
                //print(targetIndex);

                if (seeker.transform.position != targetPos)
                {
                    seeker.transform.position = Vector3.MoveTowards(seeker.transform.position, currentTile, 0.1f);
                }
                
            }
        }
    }

    void secondSeekerPath()
    {
        if (seeker2.transform.position == targetPos2)
        {
            trigger2 = true;
            currentTile2 = seeker2.transform.position;
            targetIndex2 = 0;
            nodeX2.Clear();
            nodeY2.Clear();
            path2.Clear();
        }

        if (HelloRequester2.secondSeekerPath.Length != 0)
        {
            if (trigger2 == true)
            {
                trigger2 = false;
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

                targetPos2 = path2[path2.Count - 1].worldPosition;

                grid.path2 = path2;
            }

            //Moving Seeker
            if (path2 != null)
            {
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

                if (seeker2.transform.position != targetPos2)
                {
                    seeker2.transform.position = Vector3.MoveTowards(seeker2.transform.position, currentTile2, 0.1f);
                } 
            }
        }
    }

    void thirdSeekerPath()
    {
        if (seeker3.transform.position == targetPos3)
        {
            trigger3 = true;
            currentTile3 = seeker3.transform.position;
            targetIndex3 = 0;
            nodeX3.Clear();
            nodeY3.Clear();
            path3.Clear();
        }

        if (HelloRequester3.thirdSeekerPath.Length != 0)
        {
            if (trigger3 == true)
            {
                trigger3 = false;
                for (int i = 0; i < HelloRequester3.thirdSeekerPath.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        nodeY3.Add(Mathf.Abs(HelloRequester3.thirdSeekerPath[i] - 32));
                        nodeX3.Add(HelloRequester3.thirdSeekerPath[i + 1]);
                    }
                }

                //print("Test this now " + HelloRequester2.secondSeekerPath.Length);

                for (int j = 0; j < nodeX3.Count; j++)
                {
                    Node tile3 = grid.NodeFromWorldPoint(new Vector3(nodeX3[j], 0.0f, nodeY3[j]));

                    path3.Add(tile3);
                }

                targetPos3 = path3[path3.Count - 1].worldPosition;

                grid.path3 = path3;
            }

            //Moving Seeker
            if (path3 != null)
            {
                if (seeker3.transform.position == currentTile3)
                {
                    if (targetIndex3 < path3.Count)
                    {
                        targetIndex3++;

                        if (targetIndex3 >= path3.Count)
                        {
                            targetIndex3 = 0;
                        }
                        currentTile3 = path3[targetIndex3].worldPosition;
                    }
                }
                //print("this is " + targetIndex);

                if (seeker3.transform.position != targetPos3)
                {
                    seeker3.transform.position = Vector3.MoveTowards(seeker3.transform.position, currentTile3, 0.1f);
                }
            }
        }
    }
}
