using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIPerception : MonoBehaviour
{
    [SerializeField] string tagName = "";
    [SerializeField] float distance = 1, maxAngle = 45;

    public string TagName => tagName;
    public float Distance => distance;
    public float MaxAngle => maxAngle;

    public abstract GameObject[] GetGameobjects();
}