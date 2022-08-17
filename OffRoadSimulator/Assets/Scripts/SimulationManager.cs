using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [SerializeField] CarView carView;
    [SerializeField] CameraFollow mainCamera;
    [SerializeField] CameraFollow sideCamera;
    void Start()
    {
        CarControllerKeyBoard inputController = new CarControllerKeyBoard();

        // Can add support for other platform inputs here by replacing

        CarController carController = new CarController(carView, inputController);
    }
}
