using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody rigid;
    RaycastHit hit;
    float rayDistance;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1f;
        rayDistance = transform.localScale.y + 1;
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward*moveSpeed);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back*moveSpeed);
        else if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left*moveSpeed);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right*moveSpeed);
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.blue, 0.3f);
        if(Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            if(hit.transform.tag == "Portal")
            {
                hit.transform.gameObject.GetComponent<Tile>().WarpPortal();
            }
        }
    }
}
