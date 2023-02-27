using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody rigid;
    RaycastHit hit;
    float rayDistance;
    GameObject model;
    Vector3 vertical;
    Vector3 horizontal;
    Quaternion rotate;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1f; //20f
        rayDistance = transform.position.y + 1;
        rigid = GetComponent<Rigidbody>();
        model = transform.GetChild(0).gameObject;
        rotate = Quaternion.identity;
    }

    void FixedUpdate()
    {
        vertical = Vector3.forward * Input.GetAxisRaw("Vertical");
        horizontal = Vector3.right * Input.GetAxisRaw("Horizontal");
        Move();
        Rotate();
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

    void Move()
    {
        Vector3 dir =  vertical + horizontal;
        if(vertical != Vector3.zero && horizontal != Vector3.zero) //대각선
        {
            //rigid.MovePosition(rigid.position + locDir * (moveSpeed / 2) * Time.fixedDeltaTime);
            transform.Translate(dir * (moveSpeed/2));
        }
        else
        {
            transform.Translate(dir*moveSpeed);
            //rigid.MovePosition(rigid.position + locDir * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void Rotate()
    {
        Vector3 dir = vertical + horizontal;
        if(dir == new Vector3(0, 0, 1))
        {
            rotate = Quaternion.Euler(0, 0, 0);
        }
        else if (dir == new Vector3(1, 0, 0))
        {
            rotate = Quaternion.Euler(0, 90, 0);
        }
        else if (dir == new Vector3(0, 0, -1))
        {
            rotate = Quaternion.Euler(0, 180, 0);
        }
        else if (dir == new Vector3(-1, 0, 0))
        {
            rotate = Quaternion.Euler(0, 270, 0);
        }
        else if (dir == new Vector3(1, 0, 1))
        {
            rotate = Quaternion.Euler(0, 45, 0);
        }
        else if (dir == new Vector3(1, 0, -1))
        {
            rotate = Quaternion.Euler(0, 135, 0);
        }
        else if (dir == new Vector3(-1, 0, 1))
        {
            rotate = Quaternion.Euler(0, 315, 0);
        }
        else if (dir == new Vector3(-1, 0, -1))
        {
            rotate = Quaternion.Euler(0, 225, 0);
        }
        model.transform.localRotation = rotate;
    }
}
