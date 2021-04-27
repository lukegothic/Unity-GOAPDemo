using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PutSaleAction : GOAPAction
{
    public PutSaleAction() : base() {
        Preconditions.Add(new GOAPState("hasheart", true));
        Preconditions.Add(new GOAPState("hassale", false));
        Effects.Add(new GOAPState("hassale", true));
    }
}
