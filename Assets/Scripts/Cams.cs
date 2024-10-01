using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cams : MonoBehaviour
{
    [SerializeField] private Hacker hacker;

    private void OnEnable()
    {
        hacker.PlayStatic();
    }

    private void OnDisable()
    {
        hacker.StopStatic();
    }
}
