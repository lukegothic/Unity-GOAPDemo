using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuyFlowerAction : GOAPAction
{
    public BuyFlowerAction() : base() {
        Preconditions.Add(new GOAPState("hasflower", false));
        Preconditions.Add(new GOAPState("hasmoney", true));
        Effects.Add(new GOAPState("hasflower", true));
    }
}
