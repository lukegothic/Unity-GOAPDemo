using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public abstract class GOAPAction : MonoBehaviour
{
    public GameObject target;
    public float cost = 1f;
    public float time = 1f;
    public string actionName;
    public List<GOAPState> Preconditions;
    public List<GOAPState> Effects;
    public GOAPAction() {
        Preconditions = new List<GOAPState>();
        Effects = new List<GOAPState>();
    }
}