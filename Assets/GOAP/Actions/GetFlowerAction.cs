using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GetFlowerAction : GOAPAction
{
    public GetFlowerAction() : base() {
        Preconditions.Add(new GOAPState("hasflower", false));
        Effects.Add(new GOAPState("hasflower", true));
    }
}
