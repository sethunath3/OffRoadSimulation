using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WheelPosition
{
    FRONT_LEFT,
    FRONT_RIGHT,
    REAR_LEFT,
    REAR_RIGHT
}

[System.Serializable]
public struct WheelSetup
{
    public WheelPosition wheelPosition;
    public SuspensionView suspension;
    public TyreView tyre;
    public Transform forceCornerPoint;
}

public class CarController : GenericController<CarController>
{
    Rigidbody rigidBody = null;
    ICarControllerInput inputController = null;

    CarSO carConfig = null;

    List<SuspensionController> carSuspensionConntrollers;
    List<TyreController> carTyreConntrollers;

    float ackermannAngleLeft;
    float ackermannAngleRight;


    public CarController(CarView _view, ICarControllerInput _inputHandler) : base(_view)
    {
        rigidBody = view.GetComponent<Rigidbody>();
        inputController = _inputHandler;
        carConfig = _view.CarConfig;
        InitializeCarSuspensions();
    }

    public override void Update(float deltaTime)
    {
        ApplySteeringInput(inputController.GetSteeringInput());
        UpdateTyreSteerAngle();

        ApplyAccelarationinput(inputController.GetAccelarationInput());
    }

    public void ApplyAccelarationForceOnCarBodyAt(Vector3 _force, Vector3 _position)
    {
        rigidBody.AddForceAtPosition(_force, _position);
    }

    public void ApplySuspensionForceOnCarBodyAt(Vector3 _force, WheelPosition _position)
    {
        Vector3 forcePont = ((CarView)view).wheels.Find(x => x.wheelPosition == _position).forceCornerPoint.position;
        rigidBody.AddForceAtPosition(_force, forcePont);
    }

    public Vector3 GetPointVelocityFor(Vector3 referencePoint)
    {
        return rigidBody.GetPointVelocity(referencePoint);
    }

    void ApplyAccelarationinput(float _accelarationInput)
    {
        foreach(TyreController tyre in carTyreConntrollers)
        {
            tyre.ApplyAccelaration(_accelarationInput);
        }
    }

    void ApplySteeringInput(float _steeringInput)
    {
        if (_steeringInput > 0) //turning right
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(carConfig.WheelBase / (carConfig.TurningRadius + (carConfig.RearTrack / 2))) * inputController.GetSteeringInput();
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(carConfig.WheelBase / (carConfig.TurningRadius - (carConfig.RearTrack / 2))) * inputController.GetSteeringInput();
        }
        else if (_steeringInput < 0) //turning left
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(carConfig.WheelBase / (carConfig.TurningRadius - (carConfig.RearTrack / 2))) * inputController.GetSteeringInput();
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(carConfig.WheelBase / (carConfig.TurningRadius + (carConfig.RearTrack / 2))) * inputController.GetSteeringInput();
        }
        else
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }
    }

    void UpdateTyreSteerAngle()
    {
        foreach(TyreController tyre in carTyreConntrollers)
        {
            if(tyre.TyrePosition == WheelPosition.FRONT_LEFT)
            {
                tyre.UpdateTyreSteerAngle(ackermannAngleLeft);
            }
            else if (tyre.TyrePosition == WheelPosition.FRONT_RIGHT)
            {
                tyre.UpdateTyreSteerAngle(ackermannAngleRight);
            }
        }
    }

    void InitializeCarSuspensions()
    {
        carSuspensionConntrollers = new List<SuspensionController>();
        carTyreConntrollers = new List<TyreController>();
        foreach (WheelSetup wheel in ((CarView)view).wheels)
        {
            TyreController tyreController = new TyreController(wheel.tyre, wheel.wheelPosition, ((CarView)view).CarConfig.steerResponseDelay, this);
            SuspensionController suspensionController = new SuspensionController(wheel.suspension, ((CarView)view).suspensionConfig, this, tyreController);
            tyreController.SetSuspension(suspensionController);
            carTyreConntrollers.Add(tyreController);
            carSuspensionConntrollers.Add(suspensionController);
        }
    }
}
