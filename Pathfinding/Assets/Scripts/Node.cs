using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable, grass, mud, water;
    public Vector3 worldPosition;
    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public Node parent;

    public Node(bool _walkable, bool _grass, bool _mud, bool _water, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkable;
        grass = _grass;
        mud = _mud;
        water = _water;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
