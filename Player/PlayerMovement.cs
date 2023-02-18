using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float rotateSpeed = 180f;

    private PlayerInput playerInput; 
    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (this.transform.position.y < -5)
        {
            GameManager.instance.PlayerDead();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
}

    private void Move()
    {
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    private void Rotate()
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
            playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Rigidbody otherRB = collision.rigidbody;
            otherRB.AddExplosionForce(300, collision.contacts[0].point, 10);
        }
    }
}
