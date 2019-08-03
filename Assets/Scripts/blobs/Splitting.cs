using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Life))]
public class Splitting : MonoBehaviour
{

    public Life LifePool;

    // Start is called before the first frame update
    void Start()
    {
        if (LifePool == null)
        {
            LifePool = GetComponent<Life>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Split the attached GameObject into two. Life is split evenly,
    // everything else is copied.
    public void Split()
    {        
        GameObject child1 = Instantiate(gameObject);
        child1.GetComponent<Life>().MaxHP = LifePool.MaxHP / 2;
        child1.GetComponent<Life>().HP = LifePool.HP / 2;

        GameObject child2 = Instantiate(gameObject);
        child2.GetComponent<Life>().MaxHP = (LifePool.MaxHP + 1) / 2;
        child2.GetComponent<Life>().HP = (LifePool.HP + 1) / 2;

        Destroy(gameObject);
    }

    // Combine the attached GameObject with another one. Life is combined.
    public void Combine(GameObject other)
    {
        // Actually just "eats" the other object
        Life otherLife = other.GetComponent<Life>();
        if (otherLife == null) return;

        LifePool.MaxHP += otherLife.MaxHP;
        LifePool.HP += otherLife.HP;

        Destroy(other);
    }
}
