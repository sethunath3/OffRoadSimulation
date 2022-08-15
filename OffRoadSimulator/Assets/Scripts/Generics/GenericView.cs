using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericView : MonoBehaviour
{
    protected IController controller = null;

    public void SetController(IController _controller)
    {
        controller = _controller;
    }

    protected void Update()
    {
        if(controller != null)
        {
            controller.Update(Time.deltaTime);
        }
    }
}
