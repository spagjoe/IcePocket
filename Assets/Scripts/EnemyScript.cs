using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyScript : MonoBehaviour
{
    public float eSpeed;
    public Rigidbody2D rb;
    protected Transform target;
    protected Vector2 movedir;
    public float moveDampPercent;

    public float maxFreeze;
    protected float currFreeze;
    public bool isFrozen;
    public float thawRate;

    public float maxSpeed;
    protected SpriteRenderer sprite;
    protected Animator animator;
    protected bool hasTarget;
    protected bool isFacingRight;
    public ScoreScript scoreScript;
    public GameObject audioPlayer;
    public AudioScript audioSource;
    
    protected int pointValue;
    protected int pointMulti;

    public virtual void Start()
    {
        target = GameObject.Find("Player").transform;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        scoreScript = GameObject.Find("ScoreText").GetComponent<ScoreScript>();
        audioPlayer = GameObject.Find("Audio Player");
        audioSource = audioPlayer.GetComponent<AudioSource>().GetComponent<AudioScript>();
        pointMulti = 1;
        isFacingRight = true;
    }
    public virtual void Update()
    {
        if (target)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            movedir = dir;
        }
    }
    public virtual void FixedUpdate()
    {
        if(currFreeze >= maxFreeze && !isFrozen)
        {
            isFrozen = true;
            rb.velocity = new Vector2(0, 0);
            sprite.color = new Color(0.345098f, 0.427451f, 0.6980392f, 1);
            toggleAnimation();
        }
        if (target) {
            hasTarget = true;
            if(!isFrozen) {
                moveToPlayer();
                hasTarget = true;
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
            }
            else {
                if (currFreeze <= 0) {
                    isFrozen = false;
                    sprite.color = new Color(1, 1, 1, 1);
                    currFreeze = 0;
                    toggleAnimation();
                }
                else {
                    subFreeze(thawRate);
                }
            }

            if(target.transform.position.x < this.transform.position.x) {
                if (isFacingRight && !isFrozen) {
                    flipSprite();
                }
            }
            else {
                if (!isFacingRight && !isFrozen) {
                    flipSprite();
                }
            }
        }
        else {
            hasTarget = false;
        }

    }
    protected void moveToPlayer()
    {
        Vector2 targetVector = new Vector2(movedir.x, movedir.y);
        //Wacky movement dampening algo
        float angle = Vector2.SignedAngle(targetVector, rb.velocity);
        if (angle > 90)
        {
            angle -= 90;
        }
        else if (angle < -90)
        {
            angle += 90;
        }
        Vector2 moveVector = Quaternion.Euler(0, 0, angle * moveDampPercent * -1) * targetVector;
        //conditional normalize
        if (moveVector.magnitude > 1)
        {
            moveVector = moveVector / moveVector.magnitude;
        }

        //Force based movement
        rb.AddForce(moveVector * eSpeed);
    }
    public void addFreeze(float amt)
    {
        currFreeze += amt * Time.deltaTime;
    }
    public void subFreeze(float amt)
    {
        currFreeze -= amt * Time.deltaTime;
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Bonus") {
            pointMulti = 2;
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            //ScoreScript.score += (pointValue * pointMulti);
            scoreScript.addScore(pointValue, pointMulti);
            audioSource.playSplash();
            if(pointMulti > 1) {
                audioSource.playBonus();
            }
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.tag == "Bonus") {
            pointMulti = 1;
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        if ((collision.gameObject.tag == "Wall") && isFrozen) {
            audioSource.playDink();
        }
        else if ((collision.gameObject.tag == "Enemy") && isFrozen && collision.gameObject.GetComponent<EnemyScript>().isFrozen) {
            audioSource.playDink();
        }
    }

    protected void flipSprite() {
        Vector2 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        this.transform.localScale = currentScale;

        isFacingRight = !isFacingRight;
    }

    protected void toggleAnimation() {
        if(animator.speed == 0) {
            animator.speed = 1;
        }
        else {
            animator.speed = 0;
        }
    }
}
