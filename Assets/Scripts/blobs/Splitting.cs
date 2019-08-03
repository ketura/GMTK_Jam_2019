using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Life))]
public class Splitting : MonoBehaviour
{

    private Life lifePool;
    public int StartingHP;
    private double scaleFactor;

    // Start is called before the first frame update
    void Start()
    {
        lifePool = GetComponent<Life>();
        ScaleVolumeAndMass();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Split the attached GameObject into two. Life is split evenly,
    // everything else is copied.
    public void Split()
    {

        Vector3 offset = VectorUtils.RandomHorizontalUnitVector() * (float)scaleFactor/2;

        GameObject child1 = Instantiate(gameObject);
        child1.GetComponent<Life>().MaxHP = lifePool.MaxHP / 2;
        child1.GetComponent<Life>().HP = lifePool.HP / 2;
        child1.GetComponent<Transform>().position += offset;

        GameObject child2 = Instantiate(gameObject);
        child2.GetComponent<Life>().MaxHP = (lifePool.MaxHP + 1) / 2;
        child2.GetComponent<Life>().HP = (lifePool.HP + 1) / 2;
        child2.GetComponent<Transform>().position -= offset;


        Destroy(gameObject);
    }

    // Combine the attached GameObject with another one. Life and momenta are combined.
    public void Combine(GameObject other)
    {
        // Actually just "eats" the other object
        Life otherLife = other.GetComponent<Life>();
        if (otherLife == null) return;

        GameObject combined = Instantiate(gameObject);

        // Combine life totals
        Life combinedLife = combined.GetComponent<Life>();
        combinedLife.MaxHP = lifePool.MaxHP + otherLife.MaxHP;
        combinedLife.HP = lifePool.HP + otherLife.HP;
        combined.GetComponent<Splitting>().Start();

        // Combine momenta
        Rigidbody myBody = GetComponent<Rigidbody>();
        Rigidbody otherBody = other.GetComponent<Rigidbody>();
        if (myBody != null && otherBody != null)
        {
            Vector3 totalMomentum = myBody.mass * myBody.velocity + otherBody.mass * otherBody.velocity;
            Rigidbody combinedBody = combined.GetComponent<Rigidbody>();
            combinedBody.velocity = totalMomentum / combinedBody.mass;
        }

        // clean up
        Destroy(other);
        Destroy(gameObject);
    }

    private void ScaleVolumeAndMass()
    {
        Transform transform = GetComponent<Transform>();
        double volFactor = (double)lifePool.MaxHP / StartingHP;
        scaleFactor = Math.Pow(volFactor, (double) 1/3);
        transform.localScale = Vector3.one * (float) scaleFactor;

        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
        {
            body.mass = (float)volFactor;
        }
    }
}
