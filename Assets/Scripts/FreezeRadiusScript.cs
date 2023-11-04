using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRadiusScript : MonoBehaviour
{
    public delegate void EnemyEnterEvent(EnemyScript enemy);
    public delegate void EnemyExitEvent(EnemyScript enemy);
    public EnemyEnterEvent onEnemyEnter;
    public EnemyExitEvent onEnemyExit;
    private List<EnemyScript> enemiesInRadius = new List<EnemyScript>();
    public float freezeDPS;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(EnemyScript enemy in enemiesInRadius)
        {
            enemy.addFreeze(freezeDPS);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<EnemyScript>(out EnemyScript enemy))
        {
            enemiesInRadius.Add(enemy);
            onEnemyEnter?.Invoke(enemy);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyScript>(out EnemyScript enemy))
        {
            enemiesInRadius.Remove(enemy);
            onEnemyExit?.Invoke(enemy);
        }
    }
    private void OnDisable()
    {
        foreach(EnemyScript enemy in enemiesInRadius)
        {
            onEnemyExit?.Invoke(enemy);
        }
    }
}
