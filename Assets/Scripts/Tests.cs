using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tests : MonoBehaviour
{
    public BreadScript a;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = $"{a.transform.position}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
