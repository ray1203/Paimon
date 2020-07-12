using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformCreater : MonoBehaviour
{
    public GameObject platform;
    public GameObject folder;
    void Start()
    {
        for(int i = 0; i < 10; i++)
        {

            GameObject newObject = Instantiate(platform,new Vector2(-10f+2.5f*i,-4.5f),Quaternion.identity);
            newObject.transform.SetParent(folder.transform);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
