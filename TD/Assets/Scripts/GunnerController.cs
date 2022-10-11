using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class GunnerController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject bulletPrefab;
    public float startDelay;
    public float fireRate;
    public float range;
    public bool isActive;
    public bool canFire;
    private GameObject closestEnemy;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void OnMouseUp()
    {
        Activate();
    }

    public async void Activate()
    {
        if (gameManager.isGameActive && isActive==false)
        {
            gameManager.deSelect();
            while(gameManager.selected=null)
            {
                await Task.Delay(25);
                Debug.Log("awaiting...");
            }
            isActive = true;
            gameObject.tag = "Selected";
            Debug.Log("Selected");
        }
    }

    void Update()
    {
        //Is this gunner selected?
        if (gameManager.isGameActive && isActive && gameObject.tag=="Untagged"){
            isActive = false;
        }
        else if (gameManager.isGameActive && isActive && !(gameObject.tag=="Untagged")){
        findClosestEnemy();
        }
    }

    private async void FireBullet() {
        if (isActive && !(canFire)){
            while (!(canFire)){
                await Task.Delay(25);
            }
        } else if (gameManager.isGameActive && isActive && canFire){
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        } else if(!(isActive)){
            CancelInvoke("FireBullet");
        }
    }

    void findClosestEnemy() {
        
        float distanceToClosestEnemy = Mathf.Infinity;
        EnemyController closestEnemy = null;
        EnemyController[] allEnemies = GameObject.FindObjectsOfType<EnemyController>();

        foreach (EnemyController currentEnemy in allEnemies) {
            float distancetoEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distancetoEnemy < distanceToClosestEnemy && distancetoEnemy < range) {
                distanceToClosestEnemy = distancetoEnemy;
                closestEnemy = currentEnemy;
            }
        }
        if ( !(closestEnemy == null) ) {
            Vector2 direction = closestEnemy.transform.position - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            transform.eulerAngles = new Vector3 (0, 0, angle-90);
        }
        
        if ( !(closestEnemy == null) && IsInvoking("FireBullet")) {
            canFire=true;
        } else if ( !(closestEnemy == null) && !(IsInvoking("FireBullet")) ) {
            InvokeRepeating("FireBullet", startDelay, fireRate);
        } else if (closestEnemy == null && IsInvoking("FireBullet") ) {
            canFire=false;
        }
    }
}
