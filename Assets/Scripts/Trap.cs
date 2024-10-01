using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    public float damage;
    public enum TrapType
    {
        Range,
        Melee
    }
}
