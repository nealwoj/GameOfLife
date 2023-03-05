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

    // Update is called once per frame
    void Update()
    {
        //game loop - update next buffer, then swap to the next buffer
        rules.Step();
        SwapBuffers();
    }

    #region Generating Grid
    //generate a grid based on rows and cols
    public void GenerateGrid(int r, int c)
    {
        //ClearGrid();

        rows = r;
        cols = c;

        //grid offset based on camera size
        Camera.main.orthographicSize = rows / 2;
        vertical = (int)Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.height);

        Randomize();
        DrawGrid();
    }
    //randomize grid on current buffer
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
    #endregion

    #region Drawing Grid
    //draw new grid
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
            go.transform.position = new Vector3(x - horizontal, y - vertical, 0f);
            return go;
        }
        else
        {
            GameObject go = Instantiate(_dead);
            go.transform.SetParent(GameObject.Find("Grid").transform);
            go.transform.position = new Vector3(x - horizontal, y - vertical, 0f);
            return go;
        }
    }
    #endregion

    #region Buffer Data
    public void SwapBuffers()
    {
        if (init && timer)
        {
            bufferDelay = delaySlider.value;

            if (timeCount > bufferDelay)
            {
                //rules.Step();

                currentBuffer = nextBuffer;
                nextBuffer = new bool[rows, cols];
                timeCount = 0f;

                DrawGrid();
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
            x = 0;

        if (y < 0f)
            y += cols;
        if (y >= cols)
            y = 0;

        return currentBuffer[x, y];
    }
    public void SetNextBuffer(int x, int y, bool val)
    {
        if (x < 0f)
            x += rows;
        if (x >= rows)
            x = 0;

        if (y < 0f)
            y += cols;
        if (y >= cols)
            y = 0;

        nextBuffer[x, y] = val;
    }
    public void SetCurrentBuffer(int x, int y, bool val)
    {
        if (x < 0f)
            x += rows;
        if (x >= rows)
            x = 0;

        if (y < 0f)
            y += cols;
        if (y >= cols)
            y = 0;

        currentBuffer[x, y] = val;
    }
    #endregion

    #region UI
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
        GenerateGrid((int)sizeSlider.value, (int)sizeSlider.value);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void OnSizeChanged()
    {
        GenerateGrid((int)sizeSlider.value, (int)sizeSlider.value);
    }
    #endregion
}