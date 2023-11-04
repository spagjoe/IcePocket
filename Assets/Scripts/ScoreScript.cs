using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public static int score;
    public static int lives;
    Text scoreDisp;
    private GameObject player;
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
        scoreDisp = GetComponent<Text>();
        score = 0;
        lives = 3;
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisp.text = "SCORE: \n" + score + "\n\nLIVES: \n" + lives;
    }

    public void addScore(int points, int multiplier) {
        if (playerScript.isAlive) {
            score = score + (points * multiplier);
        }
    }
}
