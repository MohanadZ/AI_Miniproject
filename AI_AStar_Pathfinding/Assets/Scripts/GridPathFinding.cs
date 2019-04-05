﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPathFinding : MonoBehaviour
{
    public LayerMask unwalkableMask, grassMask, mudMask, waterMask;
    public Vector2 gridWorldSize;           //The area in world coordinates that the grid covers
    public float nodeRadius;                //How much space each individual node covers
    Node[,] grid;                           //Array of nodes
    public List<Node> path;
    public List<Node> path2;
    float nodeDiameter;
    int gridSizeX, gridSizeY;               //The total number of nodes in x and y

    public string gridToArray = "";

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        createGrid();
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        /*
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        */

        int x = Mathf.RoundToInt(worldPosition.x / nodeDiameter); //- 1;
        int y = Mathf.RoundToInt(worldPosition.z / nodeDiameter); //- 1;


        return grid[x, y];
    }

    void createGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position /*- Vector3.right*/ * gridWorldSize.x / 2 /*- 2 * Vector3.forward*/ * gridWorldSize.y / 2;

        for (int y = gridSizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                bool isGrass = Physics.CheckSphere(worldPoint, nodeRadius, grassMask);
                bool isMud = Physics.CheckSphere(worldPoint, nodeRadius, mudMask);
                bool isWater = Physics.CheckSphere(worldPoint, nodeRadius, waterMask);

                grid[x, y] = new Node(walkable, isGrass, isMud, isWater, worldPoint, x, y);

                //print("World Bottom left " + worldBottomLeft);
                //print("World Point " + worldPoint);

                if (walkable && !isGrass && !isMud && !isWater)
                {
                    gridToArray += "0";
                }
                else if (!walkable)
                {
                    gridToArray += "1";
                }
                else if (isGrass && walkable && !isMud && !isWater)
                {
                    gridToArray += "2";
                }
                else if (isWater && walkable && !isGrass && !isMud)
                {
                    gridToArray += "3";
                }
                else if (isMud && walkable && !isGrass && !isWater)
                {
                    gridToArray += "4";
                }      
            }
        }

        //Debug.Log(gridToArray);

        /*
        print("This is ... " + worldBottomLeft);
        for(int y = gridSizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                print("This x = " + x + " world point is " + worldPoint);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);

                if (walkable)
                {
                    gridToArray += "0";
                }
                else
                {
                    gridToArray += "1";
                }
            }
        }
         */

        //print(gridToArray);
    }

    /*
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
    */

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        Gizmos.DrawWireCube(transform.position + Vector3.right * gridWorldSize.x / 2 + Vector3.forward * gridWorldSize.y / 2, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            //Node playerNode = NodeFromWorldPoint(player.position);
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;

                if(n.walkable && n.grass)
                {
                    Gizmos.color = Color.green;
                }
                if(n.walkable && n.water)
                {
                    Gizmos.color = new Color(0f, 0.35f, 1f, 1f); //blue
                }
                if(n.walkable && n.mud)
                {
                    Gizmos.color = new Color(0.48f, 0.3f, 0.04f, 1f); //brown
                }

                if(path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                if(path2 != null)
                {
                    if (path2.Contains(n))
                    {
                        Gizmos.color = Color.yellow;
                    }
                }

                Gizmos.DrawCube(n.worldPosition, new Vector3(1, 0.3f, 1) * (nodeDiameter - 0.1f));
            }
        }
    }
}