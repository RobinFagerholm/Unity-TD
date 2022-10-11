using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SquareController : MonoBehaviour
{
    public GameObject towerPrefab;
    public Material pathMaterial;
    private GameManager gameManager;
    private bool canPlaceTower;
    public GameObject towerCircle;

    void Start()
    {
        canPlaceTower = true;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        towerCircle = GameObject.Find("Tower Circle");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag== "PathMaker")
        {
            canPlaceTower = false;
            Renderer Renderer = GetComponent<Renderer>();
            Renderer.material = pathMaterial;
        }
    }
    
    void OnMouseUp()
    {
        if (canPlaceTower && gameManager.isGameActive && gameManager.coins >= Math.Floor(gameManager.towerCost))
        {
            canPlaceTower = false;
            Instantiate(towerPrefab, transform.position, Quaternion.identity);
            gameManager.SpawnTower();
            Destroy(gameObject.GetComponent<Collider2D>());
        }
    }
}
