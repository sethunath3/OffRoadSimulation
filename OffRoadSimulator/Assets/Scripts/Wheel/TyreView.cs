using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyreView : GenericView
{
    
    [SerializeField]private float tyreRadius = 0;
    public float TyreRadius { get { return tyreRadius; } private set { tyreRadius = value; } }
    
}
