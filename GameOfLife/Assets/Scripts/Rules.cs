using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour
{
    private int UP = -1, DOWN = 1, LEFT = -1, RIGHT = 1;
    private World world;

    // Start is called before the first frame update
    void Start()
    {
        world = gameObject.GetComponent<World>();
    }

    public void Step()
    {
        JohnConway();
        //more rules
    }

    public void JohnConway()
    {
        for (int x = 0; x < world.rows; x++)
        {
            for (int y = 0; y < world.cols; y++)
            {
                int neighbors = GetNeighbors(x, y);

                if (world.Get(x, y))
                {
                    if (neighbors == 2 || neighbors == 3)
                        world.SetNextBuffer(x, y, true);
                    else
                        world.SetNextBuffer(x, y, false);
                }
                else if (neighbors == 3)
                    world.SetNextBuffer(x, y, true);
            }
        }
    }

    public int GetNeighbors(int x, int y)
    {
        int num = 0;

        //above
        if (world.Get(x, y + UP))
            num++;
        if (world.Get(x + LEFT, y + UP))
            num++;
        if (world.Get(x + RIGHT, y + UP))
            num++;

        //next to
        if (world.Get(x + LEFT, y))
            num++;
        if (world.Get(x + RIGHT, y))
            num++;

        //below
        if (world.Get(x, y + DOWN))
            num++;
        if (world.Get(x + LEFT, y + DOWN))
            num++;
        if (world.Get(x + RIGHT, y + DOWN))
            num++;

        return num;
    }
}