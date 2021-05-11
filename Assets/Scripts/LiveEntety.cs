using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveEntety : MonoBehaviour
{
    public Species species;

    public void Die (causeOfDeath cause)
    {
        if (cause != causeOfDeath.eaten)
        {
            Debug.Log(transform.name + " died from '" + cause + "'.");
        }
        else
        {
            Debug.Log(transform.name + " died from being '" + cause + "'.");
        }

        Destroy(gameObject);
    }

    public float AmountRemaining
    {
        get
        {
            return amountRemaining;
        }
    }

  
    public float amountRemaining = 1;
    const float consumeSpeed = 8;

    public float Consume(float amount)
    {
        float amountConsumed = Mathf.Max(0, Mathf.Min(amountRemaining, amount));
        amountRemaining -= amount;

        transform.localScale = Vector3.one * amountRemaining;

        if (amountRemaining <= 0)
        {
            Die(causeOfDeath.eaten);
        }

        return amountConsumed;
    }

    public float growAmount = 0;

    public void GrowUp(float amount)
    {
        growAmount += amount;

        transform.localScale = Vector3.one * growAmount;
    }
}

public enum causeOfDeath
{
    hunger = (1 << 0),
    eaten = (1 << 1),
    fighting = (1 << 2),
}
