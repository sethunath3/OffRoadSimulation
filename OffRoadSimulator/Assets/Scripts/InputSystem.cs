using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarControllerInput
{
    public float GetSteeringInput();
    public float GetAccelarationInput();
}

public class CarControllerKeyBoard : MonoBehaviour, ICarControllerInput
{
    public float GetAccelarationInput()
    {
        return Input.GetAxis("Vertical");
    }

    public float GetSteeringInput()
    {
        return Input.GetAxis("Horizontal");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

public class CarControllerJoyStick : MonoBehaviour, ICarControllerInput
{
    public float GetAccelarationInput()
    {
        throw new System.NotImplementedException();
    }

    public float GetSteeringInput()
    {
        throw new System.NotImplementedException();
    }
}
