using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] Transform o1;
    [SerializeField] Transform o2;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var dir = o2.position - o1.position;
        transform.position = (o1.position + o2.position) / 2;
        transform.rotation = Quaternion.LookRotation(new Vector3(-dir.z,dir.y,dir.x).normalized);
        transform.localScale = new Vector3(Vector3.Distance(o1.position, o2.position), transform.localScale.y, transform.localScale.z);
    }
}
