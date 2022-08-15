using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspensionController : GenericController<SuspensionController>
{
    SuspensionSO suspensionConfig = null;
    TyreController tyreController = null;
    CarController carController = null;

    float minLength;
    float maxLength;
    float previousSpringLength;
    float springLength;
    public float SpringForce { get; private set; }
    float damperForce;
    float springSpeed;


    Vector3 suspensionForce;

    public SuspensionController(SuspensionView _view, SuspensionSO _suspensionConfig, CarController _carController, TyreController _tyreController) : base(_view)
    {
        carController = _carController;
        tyreController = _tyreController;
        SetupSpringValues(_suspensionConfig);
    }

    public override void Update(float deltaTime)
    {
        int layerMask = 1 << (int)6;
        if (Physics.Raycast(view.transform.position, -view.transform.up, out RaycastHit hit, maxLength + tyreController.GetTyreRadius(), layerMask))
        {
            previousSpringLength = springLength;
            springLength = hit.distance - tyreController.GetTyreRadius();
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springSpeed = (previousSpringLength - springLength) / deltaTime;
            SpringForce = suspensionConfig.SpringStiffness * (suspensionConfig.RestLength - springLength);
            damperForce = suspensionConfig.DamperStiffness * springSpeed;

            suspensionForce = (SpringForce + damperForce) * view.transform.up;

            carController.ApplySuspensionForceOnCarBodyAt(suspensionForce, tyreController.TyrePosition);
            tyreController.UpdateTyrePosition(view.transform.localPosition - (view.transform.up * springLength));
        }
    }

    private void SetupSpringValues(SuspensionSO _suspensionConfig)
    {
        suspensionConfig = _suspensionConfig;
        minLength = suspensionConfig.RestLength - suspensionConfig.SpringTravel;
        maxLength = suspensionConfig.RestLength + suspensionConfig.SpringTravel;
        springLength = suspensionConfig.RestLength;
    }
}
