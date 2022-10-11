using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyForward : MonoBehaviour
{
    public float Speed = 10f;
    private float yBound = 7;
    private float xBound = 12;

    void Update()
    {
        if (transform.position.y > yBound || transform.position.y < -yBound || transform.position.x > xBound || transform.position.x < -xBound)
        {
            DestroyObject(gameObject);
        }
        else
        {
            transform.Translate(Vector2.up * Time.deltaTime * Speed);
        }   
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            //Kills give coins?
            //gameManager.UpdateCoins(1);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}