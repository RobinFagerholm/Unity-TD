using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Indicator : MonoBehaviour
{
    public GameObject selected;
    
    void Update()
    {
        selected = GameObject.FindGameObjectWithTag("Selected");
        transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y, -1);
    }
}
