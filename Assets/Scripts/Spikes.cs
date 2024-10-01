using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Trap
{
    public TrapType type;
    [SerializeField] private LayerMask mask;
    

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
