using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Config/SuspensionConfig", fileName = "SuspensionConfig")]
public class SuspensionSO : ScriptableObject
{
    public float RestLength;
    public float SpringTravel;
    public float SpringStiffness;
    public float DamperStiffness;
}
