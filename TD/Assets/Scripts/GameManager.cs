using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject pathMakerPrefab;
    public GameObject[] waypoints;
    private GameObject start;
    public int enemyCount;
    public int waveNumber;
    public bool isGameActive;
    public bool isSpawningEnemyWave;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI towerCostText;
    public TextMeshProUGUI gameOverText;
    public Text waveIndicator;
    public Button startButton;
    public GameObject settings;
    public Button settingsButton;
    public int WaveCooldown = 3;
    public Text WaveCooldownButtonText;
    public GameObject titleScreen;
    private int lives;
    public double coins;
    public int coinsPerSecond;
    public double towerCost;
    public float spawnRate;
    public GameObject selected;

    void Start()
    {
        isGameActive = false;
        isSpawningEnemyWave = false;
        start = GameObject.Find("Start");
        coins = 7;
        lives = 15;
        spawnRate = 50;
        waveNumber = 0;
        UpdateLives(0);
        UpdateCoins(0);
        towerCost = 10;
        towerCostText.text = "Tower Cost: " + Math.Floor(towerCost);
    }

    void Update()
    {
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        if (enemyCount == 0 && isGameActive && !isSpawningEnemyWave)
        {
            Debug.Log("No more enemies.");
            waveNumber++;
            UpdateCoins(3);
            StartCoroutine(SpawnEnemyWave(waveNumber));
            StartCoroutine(NextWave(waveNumber));
        }
        selected = GameObject.FindGameObjectWithTag("Selected");
    }

    public void OpenSettings()
    {
        settingsButton.gameObject.SetActive(false);
        titleScreen.gameObject.SetActive(false);
        settings.gameObject.SetActive(true);
    }
    public void ChangeWaveCooldown(){
        if (WaveCooldown <= 9){
            WaveCooldown++;
        } else {
            WaveCooldown = 0;
        }
        WaveCooldownButtonText.text = "Wave Cooldown: " + WaveCooldown + "s";
    }
    public void CloseSettings(){
        settings.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(true);
        titleScreen.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        settingsButton.gameObject.SetActive(false);
        titleScreen.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        livesText.gameObject.SetActive(true);
        coinsText.gameObject.SetActive(true);
        towerCostText.gameObject.SetActive(true);
        GameObject tempPathMaker = Instantiate(pathMakerPrefab, GenerateSpawnPosition(), pathMakerPrefab.transform.rotation);
        tempPathMaker.GetComponent<PathMakerController>().Setup(waypoints);
        //InvokeRepeating("Income", 5, 5);
    }

    /*
    Remember to remove the "//" from the invoke in StartGame.
    void Income()
    {
        UpdateCoins(1); 
    }
    */

    public IEnumerator NextWave(int waveNumber){
        if (coins >= towerCost){
            waveIndicator.text = "Wave " + waveNumber + "\n" + "+3 Coins" + "\n" + "You can afford a new tower!!!";
        } else {
            waveIndicator.text = "Wave " + waveNumber + "\n" + "+3 Coins";
        }
        waveIndicator.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        waveIndicator.gameObject.SetActive(false);
    }

    public IEnumerator SpawnEnemyWave(int enemiesToSpawn) {
        isSpawningEnemyWave = true; 
        yield return new WaitForSeconds(WaveCooldown);
        for (int i = 0; i < enemiesToSpawn*1.5; i++) {
            GameObject tempEnemy = Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
            tempEnemy.GetComponent<EnemyController>().Setup(waypoints);
            
            yield return new WaitForSeconds(spawnRate/100);
        }
        spawnRate = (spawnRate/waveNumber)+30;
        isSpawningEnemyWave = false;
    }

    private Vector3 GenerateSpawnPosition()
    {
        //Add multi spawnPos support?
        Vector3 spawnPos = start.transform.position;
        return spawnPos;
    }

    public void SpawnTower() 
    {
        UpdateCoins(-Math.Floor(towerCost));
        towerCost *= 1.1;
        towerCostText.text = "Tower Cost: " + Math.Floor(towerCost);
    }

    public void deSelect()
    {
        selected.tag = "Untagged";
        selected = null;
    }

    public void UpdateLives(int livesToRemove)
    {
        lives -= livesToRemove;
        livesText.text = "Lives: " + lives;
        if ( lives <= 0) {
            isGameActive = false;
            gameOverText.text = "Game Over" + "\n" + "Wave: " + waveNumber;
            gameOverText.gameObject.SetActive(true);
        }
    }

    public void UpdateCoins(double coinsToAdd)
    {
        coins += coinsToAdd;
        coinsText.text = "Coins: " + Math.Floor(coins);
    }

    public void Reload()
    {
    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    Resources.UnloadUnusedAssets();
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
