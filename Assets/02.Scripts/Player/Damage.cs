using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private readonly string e_bullettag = "E_BULLET";
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(e_bullettag))
        {
            col.gameObject.SetActive(false);
        }
    }
}
