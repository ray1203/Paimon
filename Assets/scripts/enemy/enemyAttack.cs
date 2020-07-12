using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && col.gameObject.layer!=9)
        {
            col.GetComponent<playerCtrl>().hit(damage);
            
        }
    }
}
