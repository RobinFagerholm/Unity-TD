using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMakerController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] waypoints;
    public int waypointIndex = 0;
    private bool setupDone = false;
    public float speed = 1.0f;

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

    // Update is called once per frame
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
        if (other.tag == "End")
        {
            gameManager.isGameActive = true;
            gameManager.SpawnEnemyWave(1);
            Destroy(gameObject);
        }
    }
}
