using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    public void Update(float deltaTime);
}

public abstract class GenericController<T> : IController
{
    protected GenericView view;

    public GenericController(GenericView _view)
    {
        view = _view;
        view.SetController(this);
    }

    public abstract void Update(float deltaTime);
}
