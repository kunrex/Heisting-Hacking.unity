using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHere : MonoBehaviour
{
    [SerializeField] private GameObject dragObjectHereName;

    [SerializeField] private float timeLimit, timeToEnable;
    [SerializeField] private Vector2[] posHere;
    [SerializeField] private Vector2[] posDrag;
    private float timeElapsed;

    [SerializeField] private CCTV cctv;
    private bool isHacking = false;
    public bool reCharging = false;
    [SerializeField] private float reChargeTime;

    public void Enable(CCTV tv)
    {
        cctv = tv;

        int i = Random.Range(0, posDrag.Length - 1);
        transform.parent.gameObject.SetActive(true);
        transform.localPosition = posHere[i];
        dragObjectHereName.transform.localPosition = posDrag[i];
        isHacking = true;
    }

    private void Update()
    {
        if(isHacking)
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= timeLimit)
            {
                isHacking = false;
                transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHacking)
            return;

        if(collision.gameObject == dragObjectHereName)
        {
            isHacking = false;
            cctv.Disable();

            GameManager.instance.StartCoroutine(GameManager.instance.enableCCTV(cctv, timeToEnable));
            transform.parent.gameObject.SetActive(false);
            reCharging = true;
            RobberUI.instance.SendMessageToPlayer("1 cctv disabled");
            GameManager.instance.StartCoroutine(ReCharge());
        }
    }

    IEnumerator ReCharge()
    {
        yield return new WaitForSeconds(reChargeTime);

        reCharging = false;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        reCharging = true;
        GameManager.instance.StartCoroutine(ReCharge());
    }
}
