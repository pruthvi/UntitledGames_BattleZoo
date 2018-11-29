/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;

public class GenerateClouds : MonoBehaviour
{

    #region Variables

    public GameObject cloud;
    
    #endregion

    void Start()
    {
        CreateCloud();
    }

    void Update()
    {
    }

    public void CreateCloud()
    {
        int No = Random.Range(8, 15);
        for (int i = 0; i < No; i++)
        {
                                           //   x range             y range
            Vector2 position = new Vector2(Random.Range(-15, 20), Random.Range(-2, 6));
            float scale = Random.Range(5.0f, 20.0f);
            // Debug.Log("Random Scale:" + scale);
            GameObject gb = Instantiate(cloud, position, Quaternion.identity);
            gb.transform.SetParent(transform);
            gb.transform.localScale = new Vector2(scale, scale);
            // Debug.Log("Randomly Scale!");
        }
    }

}