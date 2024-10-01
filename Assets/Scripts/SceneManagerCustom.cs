using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    [SerializeField] private int[] levelIndexes;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int index)
    {
        StartCoroutine(Load(index));
    }

    IEnumerator Load(int index)
    {
        animator.SetTrigger("T");

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelIndexes[index]);
    }
}
