using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/CarConfig", fileName = "CarConfig")]
public class CarSO : ScriptableObject
{
    public float WheelBase;
    public float RearTrack;
    public float TurningRadius;
    public float steerResponseDelay;
}
