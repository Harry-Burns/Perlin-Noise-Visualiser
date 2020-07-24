using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[,] positions;

    public float width = 32;
    public float height = 32;
    public float depth = 1;

    public float scale;

    public float offsetX;
    public float offsetY;

    public GameObject cube;

    private GameObject tempCube;

    public Scrollbar sizeBar;
    public Scrollbar scaleBar;
    public Scrollbar depthBar;
    public Scrollbar speedBar;
    public Button reset;
    public Toggle moveX;

    private float boxOffset;

    private float sizeAll;

    private bool stopped;

    void Update()
    {
        scale = scaleBar.value * 2;
        depth = depthBar.value * 50;
        boxOffset = depth * 0.3f + scale * 0.2f;

        if (!stopped)
        {
            CalcNoise();
        }
    }

    void Redo()
    {
        stopped = true;
        sizeAll = Mathf.Round(sizeBar.value * 100f);

        width = sizeAll;
        height = sizeAll;
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Cube"))
        {
            Destroy(o);
        }

        float xzPos = -9f - sizeAll / 20;
        Camera.main.transform.position = new Vector3(xzPos, sizeAll/5 + 8f, xzPos);
        Begin();
    }

    private void FixedUpdate()
    {
        if (moveX.isOn)
        {
            offsetX += (speedBar.value * 0.02f);
        }
    }

    void Start()
    {
        stopped = true;
        reset.onClick.AddListener(Redo);
        Begin();
    }

    void Begin()
    {
        positions = new GameObject[(int)width, (int)height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tempCube = Instantiate(cube, new Vector3(x, 0, y), Quaternion.identity);
                positions[x, y] = tempCube;
            }
        }
        stopped = false;
    }

    void Calc()
    {
        CalcNoise();
    }

    void CalcNoise()
    {
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                float xCoord = (float)x / (float)width * scale + offsetX;
                float yCoord = (float)y / (float)height * scale + offsetY;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                //sample = Mathf.Round(sample * 100f) / 100f;
                positions[x, y].transform.position = new Vector3(x, (sample * depth) - boxOffset, y);
            }
        }
    }
}
