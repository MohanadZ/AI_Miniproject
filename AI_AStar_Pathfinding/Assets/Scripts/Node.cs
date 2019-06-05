using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Node class that has attribute that correspond to the type of tiles, the position in world coordinates, and the grid dimensions.
public class Node
{
    public bool walkable, grass, mud, water;
    public Vector3 worldPosition; //The point in the world that the node represents
    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public Node parent;

    public Node(bool walkable, bool grass, bool mud, bool water, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.grass = grass;
        this.mud = mud;
        this.water = water;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
