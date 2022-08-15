using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarView : GenericView
{
    [SerializeField] public List<WheelSetup> wheels;
    [SerializeField] public SuspensionSO suspensionConfig;
    [SerializeField] public CarSO CarConfig;
}
