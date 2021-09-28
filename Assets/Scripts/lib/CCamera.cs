using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICamera : IService
{
    void CorrectViewPoint(Vector3 _viewpoint);
    void SetViewPointInstant(Vector3 _viewpoint);
    void SetViewPoint(Vector3 _viewpoint);
    void SetPosition(Vector3 _position);
}

public class CCamera : MonoBehaviour, ICamera
{
    private CMove move;
    private CMove view;
    private const float changeTime = 0.7f; // час в секундах за який змінюється положення та точка зору камери
    private Vector3 viewpoint;

    private void Awake()
    {
        AllServices.Container.Register<ICamera>(this);
    }

    private void Start()
    {
        move = new CMove();
        view = new CMove();
        move.SetActionTime(changeTime);
        view.SetActionTime(changeTime);

        SetViewPointInstant(Vector3.zero);
    }
    private void LateUpdate()
    {
        if (move.IsActive())
        {
            move.UpdatePosition();
            transform.position = move.GetCurrentPosition();
        }
        if (view.IsActive())
        {
            view.UpdatePosition();
            viewpoint = view.GetCurrentPosition();
        }
        transform.LookAt(viewpoint);
    }

    private void OnDestroy()
    {
        AllServices.Container.Register<ICamera>(null);
    }

    //--------------------------
    // ICamera interface
    //--------------------------
    public void CorrectViewPoint(Vector3 _viewpoint)
    {
        if (view.IsActive())
            view.CorrectTargetPosition(_viewpoint);
        else
            SetViewPointInstant(_viewpoint);
    }

    public void SetViewPointInstant(Vector3 _viewpoint)
    {
        viewpoint = _viewpoint;
    }
    public void SetViewPoint(Vector3 _viewpoint)
    {
        if (_viewpoint != viewpoint)
        {
            view.SetPositions(viewpoint, _viewpoint);
            view.StartAction();
        }
    }

    public void SetPosition(Vector3 _position)
    {
        if (transform.position != _position)
        {
            move.SetPositions(transform.position, _position);
            move.StartAction();
        }
    }
    //--------------------------
}
