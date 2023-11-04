using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float pSpeed;
    public Rigidbody2D myRigidbody;
    private bool isVulnerable;
    public bool isAlive;
    private SpriteRenderer sprite;
    private float spawnTimer;
    private float invTime;
    public GameControlScript controller;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControlScript>();
        spawnTimer = 0;
        invTime = 3;
        animator = gameObject.GetComponent<Animator>();
        isAlive = true;
        isVulnerable = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xinput = Input.GetAxis("Horizontal");
        float yinput = Input.GetAxis("Vertical");

        if(xinput == 0 && yinput == 0) {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
            Vector2 moveVector = new Vector2(xinput, yinput);
            if(moveVector.magnitude > 1)
            {
                moveVector = moveVector / moveVector.magnitude;
            }

            if(!(spawnTimer > 0 && spawnTimer < 0.5)) {
                myRigidbody.AddForce(moveVector * pSpeed);
            }
           
            if(xinput > 0 && !animator.GetBool("isFacingRight")) {
                animator.SetBool("isFacingRight", true);
            }
            else if(xinput < 0 && animator.GetBool("isFacingRight")) {
                animator.SetBool("isFacingRight", false);
            }
        }

        if (!isVulnerable) {
            if (spawnTimer >= invTime) {
                setVulnerable();
                spawnTimer = 0;
            }
            else {
                spawnTimer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("collision exit");
        if (collision.gameObject.tag == "Floor")
        {
            if (ScoreScript.lives > 0) {
                killPlayer();
            }
            //Debug.Log("Player left play area");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy" && collision.gameObject.GetComponent<EnemyScript>().isFrozen == false && isVulnerable) {
            killPlayer();
        }
    }

    private void setVulnerable() {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        isVulnerable = true;
    }

    private void setInvulnerable() {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        isVulnerable = false;
    } 

    private void killPlayer() {
        ScoreScript.lives -= 1;
        if(ScoreScript.lives <= 0) {
            controller.gameOver();
            //Destroy(this.gameObject);
            isAlive = false;
            gameObject.SetActive(false);
        }
        else {
            setInvulnerable();
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.angularVelocity = 0;
            transform.position = new Vector3(0.419999987f, -0.219999999f, 0);
        }
    }
}
