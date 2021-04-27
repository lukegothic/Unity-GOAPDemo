using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPState
{
    public string key;
    public bool val;
    public GOAPState(string key, bool val) {
        this.key = key;
        this.val = val;
    }
}