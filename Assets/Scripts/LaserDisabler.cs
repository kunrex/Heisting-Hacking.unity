using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDisabler : MonoBehaviour
{
    [SerializeField] private Lasers laser;
    [SerializeField] private NumberPuzzle dragHere;

    [SerializeField] private float radius;
    public bool near;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (laser.enabled == false)
        {
            near = false;
            return;
        }

        near = PlayerNear();

        if (near && Input.GetKeyDown(KeyCode.E) && !dragHere.reCharging)
        {
            dragHere.TryHack(laser);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private bool PlayerNear()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }
}
