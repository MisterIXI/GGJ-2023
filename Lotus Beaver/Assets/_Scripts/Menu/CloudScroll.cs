using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScroll : MonoBehaviour
{
    private float _startX;
    public float scrollSpeed = 90f;
    private void Start()
    {
        _startX = transform.position.x;
    }
    private void Update()
    {
        // move cloud
        transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
        if(transform.position.x > 1280 + _startX)
        {
            transform.position = new Vector3(-1280 + _startX, transform.position.y, transform.position.z);
        }
    }
}
