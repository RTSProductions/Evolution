using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public MemoryType type;
}
public enum MemoryType
{
    lotsOfFood = (1 << 0),
    highPopulation = (1 << 1),
    lowFood = (1 << 2),
    lowPopulation = (1 << 3),
    baren = (1 << 4),
}
