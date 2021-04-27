using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeHeartAction : GOAPAction
{
    public MakeHeartAction() : base() {
        Preconditions.Add(new GOAPState("hasflower", true));
        Effects.Add(new GOAPState("hasheart", true));
    }
}
