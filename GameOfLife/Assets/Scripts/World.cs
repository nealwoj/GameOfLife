using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    //buffers
    public bool[,] currentBuffer, nextBuffer;

    public bool init;
    public GameObject _alive, _dead;

    private Rules rules;
    private float timeCount, bufferDelay;
    private bool timer;
    private List<GameObject> grid;

    [HideInInspector]
    public int vertical, horizontal, rows, cols;

    //UI
    public Slider sizeSlider, delaySlider, fillSlider;

    // Start is called before the first frame update
    void Start()
    {
        //rule system - John Conway
        rules = gameObject.GetComponent<Rules>();

        timer = false;
        grid = new List<GameObject>();
        GenerateGrid((int)sizeSlider.value, (int)sizeSlider.value);

        init = true;
    }

    public void GenerateGrid(int r, int c)
    {
        ClearGrid();

        rows = r;
        cols = c;

        Camera.main.orthographicSize = rows / 2;
        vertical = (int)Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.height);

        Randomize();
    }

    // Update is called once per frame
    void Update()
    {
        //game loop - update next buffer, then swap to the next buffer, then draw grid from the new buffer
        rules.Step();
        SwapBuffers();
        DrawGrid();
    }

    public void DrawGrid()
    {
        ClearGrid();

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                if (currentBuffer[x, y] == true)
                    grid.Add(DrawSprite(x,y, true));
                else
                    grid.Add(DrawSprite(x,y, false));
            }
        }
    }
    public void ClearGrid()
    {
        for (int i = 0; i < grid.Count; i++)
            Destroy(grid[i]);

        grid.Clear();
    }

    public GameObject DrawSprite(int x, int y, bool val)
    {
        if (val)
        {
            GameObject go = Instantiate(_alive);
            go.transform.SetParent(GameObject.Find("Grid").transform);
            go.transform.position = new Vector3(x - (horizontal - 0.5f), y - (vertical - 0.5f), 0f);
            return go;
        }
        else
        {
            GameObject go = Instantiate(_dead);
            go.transform.SetParent(GameObject.Find("Grid").transform);
            go.transform.position = new Vector3(x - (horizontal - 0.5f), y - (vertical - 0.5f), 0f);
            return go;
        }
    }

    public void SwapBuffers()
    {
        if (init && timer)
        {
            bufferDelay = delaySlider.value;

            if (timeCount > bufferDelay)
            {
                currentBuffer = nextBuffer;
                nextBuffer = new bool[rows, cols];

                timeCount = 0f;
            }
            else
                timeCount += Time.deltaTime;
        }
    }

    public bool Get(int x, int y)
    {
        if (x < 0f)
            x += rows;
        if (x >= rows)
            x %= rows;

        if (y < 0f)
            y += cols;
        if (y >= 0f)
            y %= cols;

        return currentBuffer[x, y];
    }
    public void SetNextBuffer(int x, int y, bool val)
    {
        if (x < 0f)
            x += rows;
        if (x >= rows)
            x %= rows;

        if (y < 0f)
            y += cols;
        if (y >= 0f)
            y %= cols;

        nextBuffer[x, y] = val;
    }
    public void SetCurrentBuffer(int x, int y, bool val)
    {
        if (x < 0f)
            x += rows;
        if (x >= rows)
            x %= rows;

        if (y < 0f)
            y += cols;
        if (y >= 0f)
            y %= cols;

        currentBuffer[x, y] = val;
    }

    public void Randomize()
    {
        currentBuffer = new bool[rows, cols];
        nextBuffer = new bool[rows, cols];

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                int ran = UnityEngine.Random.Range(0, 10);
                if (ran > ((int)fillSlider.value) / 10)
                    currentBuffer[x, y] = true;
                else
                    currentBuffer[x, y] = false;
            }
        }
    }

    //UI
    public void StartButton()
    {
        timer = true;
    }
    public void PauseButton()
    {
        timer = false;
    }
    public void RandomizeButton()
    {
        timer = false;
        Randomize();
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void OnSizeChanged()
    {
        GenerateGrid((int)sizeSlider.value, (int)sizeSlider.value);
    }
}