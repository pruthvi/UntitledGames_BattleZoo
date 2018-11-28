/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;

public class GenerateClouds : MonoBehaviour
{

    #region Variables

    public GameObject cloud;

    [Range(1, 50)]
    public int maxClouds;
    [Range(1, 50)]
    public int minClouds;

    [Range(1.0f, 10.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float minSpeed;


    [Range(1.0f, 10.0f)]
    public float maxScale;
    [Range(1.0f, 20.0f)]
    public float minScale;

    public float maxY = 6;
    public float minY = -2;

    public float maxX = 20;
    public float minX= -15;

    public Color col;

    #endregion

    void Start()
    {
        int N = Random.Range(minClouds, maxClouds);

        CreateCloud(N);

    }

    void Update()
    {

    }

    public void CreateCloud(int No)
    {
        for (int i = 0; i < No; i++)
        {
                                           //   x range             y range
            Vector2 position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            float scale = Random.Range(minScale, maxScale);
            GameObject gb = Instantiate(cloud, position, Quaternion.identity);
            gb.transform.localScale = new Vector2(scale, scale);
            gb.AddComponent<FlyingClouds>();
            gb.GetComponent<FlyingClouds>().speed = Random.Range(minSpeed, maxSpeed);
            gb.GetComponent<FlyingClouds>().col = col;

        }
    }

}