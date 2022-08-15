using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyreController : GenericController<TyreController>
{
    public WheelPosition TyrePosition { get; private set; }

    float wheelAngle;
    float desiredWheelAngle;
    float steerResponseDelay;

    Vector3 wheelVelocityLS;

    CarController carController = null;
    SuspensionController suspensionController;

    public TyreController(TyreView _view, WheelPosition _tyrePosition, float _steerResponseDelay, CarController _carController) : base((GenericView)_view)
    {
        TyrePosition = _tyrePosition;
        steerResponseDelay = _steerResponseDelay;
        desiredWheelAngle = 0;
        wheelAngle = 0;
        carController = _carController;
    }

    public void SetSuspension(SuspensionController _suspensionController)
    {
        suspensionController = _suspensionController;
    }

    public float GetTyreRadius()
    {
        return ((TyreView)view).TyreRadius;
    }

    public void UpdateTyreSteerAngle(float _angle)
    {
        desiredWheelAngle = _angle;
    }

    public void UpdateTyrePosition(Vector3 _position)
    {
        view.transform.localPosition = _position;
    }

    public void ApplyAccelaration(float accelarationValue)
    {
        int layerMask = 1 << (int)6;
        if (Physics.Raycast(view.transform.position, -view.transform.up, out RaycastHit hit, GetTyreRadius()+2, layerMask))
        {
            wheelVelocityLS = view.transform.InverseTransformDirection(carController.GetPointVelocityFor(hit.point));
            float forceX = accelarationValue * suspensionController.SpringForce;
            float forceY = wheelVelocityLS.x * suspensionController.SpringForce;
            carController.ApplyAccelarationForceOnCarBodyAt((forceX * view.transform.forward) + (forceY * -view.transform.right), hit.point);
        }
    }

    public override void Update(float deltaTime)
    {
        wheelAngle = Mathf.Lerp(wheelAngle, desiredWheelAngle, steerResponseDelay* deltaTime);
        view.transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);
    }
}
