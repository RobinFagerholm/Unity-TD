using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 1.0f;
    public GameObject[] waypoints;
    public int waypointIndex = -1;
    private bool setupDone = false;
    private int coinsToAdd = 1;
    public GameManager gameManager;
    
    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void Setup(GameObject[] wp)
    {
        waypoints = wp;
        setupDone = true;
    }

    void Update()
    {
        if (setupDone)
        {
            if (waypointIndex <= waypoints.Length - 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, speed * Time.deltaTime);
                if (transform.position == waypoints[waypointIndex].transform.position)
                {
                    waypointIndex += 1;
                }
            }
        }
        
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "End" && gameManager.isGameActive)
        {
            Destroy(gameObject);
            gameManager.UpdateLives(1);
        }
    }
}
