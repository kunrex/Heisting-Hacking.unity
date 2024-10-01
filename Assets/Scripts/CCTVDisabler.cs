using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVDisabler : MonoBehaviour
{
    [SerializeField] private CCTV cctv;
    [SerializeField] private DragHere dragHere;

    [SerializeField] private float radius;
    public bool near;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cctv.enabled == false)
        {
            near = false;
            return;
        }

        near = PlayerNear();

        if (near && Input.GetKeyDown(KeyCode.E))
        {
            dragHere.Enable(cctv);
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
        foreach(Collider2D col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }
}
