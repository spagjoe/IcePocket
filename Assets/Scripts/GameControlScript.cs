using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControlScript : MonoBehaviour
{
    private float spawnCooldown;
    private float spawnTimer;
    //private float elapsedTime;
    private int difficultyLevel;
    private float difficultyInterval; //time before level increments, in seconds
    private static int maxLevel = 5;
    
    private GameObject player;
    public GameObject smallEnemy;
    public GameObject largeEnemy;
    public GameObject bombEnemy;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject settingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        spawnCooldown = 7;
        spawnTimer = 4; //First spawn will be x seconds early
        difficultyLevel = 0;
        //elapsedTime = 0;
        difficultyInterval = 30;
        Time.timeScale = 1;

        InvokeRepeating("increaseDifficulty", difficultyInterval, difficultyInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pauseScreen.activeInHierarchy) {
                if (settingsPanel.activeInHierarchy) {
                    toggleSettings();
                }
                else {
                    unpauseGame();
                }
            }
            else {
                pauseGame();
            }
        }
        //elapsedTime += Time.deltaTime;
        if(spawnTimer >= spawnCooldown && GameObject.Find("Player")) {
            //instantiate enemy at predetermined locations
            float temp = Random.value;
            switch (difficultyLevel) {
                case 0:
                    spawnEnemy(smallEnemy);
                    break;

                case 1:
                    if (temp < 0.1) {
                        spawnEnemy(largeEnemy);
                    }
                    else spawnEnemy(smallEnemy);
                    break;

                case 2:
                    if (temp < 0.1) {
                        spawnEnemy(largeEnemy);
                    }
                    else spawnEnemy(smallEnemy);
                    break;
                case 3:
                    if (temp < 0.15) {
                        spawnEnemy(largeEnemy);
                    }
                    else if (temp > 0.9) {
                        spawnEnemy(bombEnemy);
                    }
                    else spawnEnemy(smallEnemy);
                    break;
                case 4:
                    if (temp < 0.2) {
                        spawnEnemy(largeEnemy);
                    }
                    else if (temp > 0.85) {
                        spawnEnemy(bombEnemy);
                    }
                    else spawnEnemy(smallEnemy);
                    break;
                case 5:
                    if (temp < 0.2) {
                        spawnEnemy(largeEnemy);
                    }
                    else if(temp > 0.95) {
                        spawnEnemy(smallEnemy);
                        spawnEnemy(smallEnemy);
                    }
                    else if (temp > 0.75) {
                        spawnEnemy(bombEnemy);
                    }
                    else spawnEnemy(smallEnemy);
                    break;
                default:
                    break;
            }
            spawnTimer = 0;

            //TODO: adjust rate/type of enemy spawns between cycles
        }
        else {
            spawnTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// Places specified enemy on stage, on opposite side from player
    /// </summary>
    /// <param name="enemy"></param>
    public void spawnEnemy(GameObject enemy) {
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        if (Mathf.Abs(playerX) >= Mathf.Abs(playerY)) {
            if (playerX >= 0.4) {
                Instantiate(enemy, new Vector3(-3.2f, -0.2f, 0), Quaternion.identity);
            }
            else {
                Instantiate(enemy, new Vector3(4.3f, -0.2f, 0), Quaternion.identity);
            }
        }
        else {
            if (playerY <= -0.2) {
                Instantiate(enemy, new Vector3(0.44f, 3.34f, 0), Quaternion.identity);
            }
            else {
                Instantiate(enemy, new Vector3(0.44f, -4.35f, 0), Quaternion.identity);
            }
        }
    }

    private void increaseDifficulty() {
        if(difficultyLevel < maxLevel) {
            difficultyLevel++;
            Debug.Log("Level up");
        }

        switch (difficultyLevel) {
            case 1:
                spawnCooldown = 6;
                break;
            case 2:
                spawnCooldown = 5.5f;
                break;
            case 3:
                spawnCooldown = 5f;
                break;
            case 4:
                spawnCooldown = 4.5f;
                break;
            case 5:
                spawnCooldown = 4f;
                break;
        }
    }

    public void gameOver() {
        gameOverScreen.SetActive(true);
    }

    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quitGame() {
        Application.Quit();
    }

    public void goToMenu() {
        SceneManager.LoadSceneAsync("Assets/Scenes/MainMenu.unity");
    }

    public void pauseGame() {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void unpauseGame() {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void toggleSettings() {
        if (settingsPanel.activeInHierarchy) {
            closeSettings();
        }
        else {
            openSettings();
        }
    }
    private void openSettings() {
        settingsPanel.SetActive(true);
    }

    private void closeSettings() {
        settingsPanel.SetActive(false);
    }
}
