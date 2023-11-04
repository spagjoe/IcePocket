using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemyScript : EnemyScript
{
    private bool isExploding;
    public float triggerRadius;
    public float fuseTime;
    private float explTimer;
    public float explRadius;
    public float explForce;
    public BombEnemyScript() {
        pointValue = 50;
    }
    override public void FixedUpdate() {
        if (currFreeze >= maxFreeze && !isFrozen) {
            isFrozen = true;
            toggleAnimation();
            rb.velocity = new Vector2(0, 0);
            sprite.color = new Color(0.345098f, 0.427451f, 0.6980392f, 1);
        }
        if (target) {
            hasTarget = true;
            if (!isFrozen) {
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

            if (target.transform.position.x < this.transform.position.x) {
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

        if (!isExploding && target != null && Vector2.Distance(target.transform.position, this.transform.position) < triggerRadius && !isFrozen) {
            isExploding = true;
            animator.SetBool("isFused", true);
            maxSpeed = maxSpeed / 2;
        }

        if (isExploding) {
            if (explTimer >= fuseTime) {
                Vector2 explPosition = transform.position;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(explPosition, explRadius);
                foreach (Collider2D hit in colliders) {
                    Rigidbody2D hitRB = hit.GetComponent<Rigidbody2D>();
                    if (hitRB != null && hitRB != this.rb && (hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Player")) {
                        hitRB.AddExplosionForce(explForce, this.transform.position, explRadius);
                    }
                }
                this.pointValue = 0;
                audioSource.playBomb();
                Destroy(this.gameObject);
            }
            else {
                explTimer += Time.fixedDeltaTime;
            }
        }
    }
}
