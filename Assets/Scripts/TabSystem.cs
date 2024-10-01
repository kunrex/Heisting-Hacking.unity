using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private GameObject[] tabIcons;

    [SerializeField] private Material unselectedMat;
    [SerializeField] private Material selectedMat;

    [SerializeField] private GameObject[] toggleTabs;

    // Start is called before the first frame update
    void Start()
    {
        LoadTab(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadTab(int index)
    {
        foreach (GameObject tab in tabs)
            tab.SetActive(false);

        tabs[index].SetActive(true);

        foreach (GameObject tabIcon in tabIcons)
        {
            tabIcon.GetComponent<Image>().material = unselectedMat;
        }

        tabIcons[index].GetComponent<Image>().material = selectedMat;
    }

    public void ToggleTab(int index)
    {
        toggleTabs[index].SetActive(!toggleTabs[index].activeSelf);
    }
}
