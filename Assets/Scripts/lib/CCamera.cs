using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMapDirection
{
    center = 0, north = 1, northeast = 2, east = 3, southeast = 4, south = 5, southwest = 6, west = 7, northwest = 8
}

public interface ICamera : IService
{
    void CorrectViewPoint(Vector3 _viewpoint);
    void SetViewPointInstant(Vector3 _viewpoint);
    void SetViewPoint(Vector3 _viewpoint);
    void SetPositionInstant(Vector3 _position);
    void SetPosition(Vector3 _position);
    void SetPosition(EMapDirection _dir);
    void SetRelativePosition(float _height, float _distance);
    void SetViewLock(bool _isViewLock);
    bool IsBusy();
}

public class CCamera : MonoBehaviour, ICamera
{
    private CMove move;
    private CMove view;
    private const float changeTime = 0.7f; // час в секундах за який змінюється положення та точка зору камери
    private Vector3 viewpoint;
    private Vector3[] positionList;
    private Vector3 currentPosition;
    private float height = 15.0f;
    private float distance = 15.0f;
    private bool isViewLock = true;

    private void InitPositions()
    {
        positionList[0] = new Vector3(0, height, 0); // center
        positionList[1] = new Vector3(0, height, distance); // north
        positionList[2] = new Vector3(distance * 0.7071f, height, distance * 0.7071f); // northeast
        positionList[3] = new Vector3(distance, height, 0); // east
        positionList[4] = new Vector3(distance * 0.7071f, height, -distance * 0.7071f); // southeast
        positionList[5] = new Vector3(0, height, -distance); // south
        positionList[6] = new Vector3(-distance * 0.7071f, height, -distance * 0.7071f); // southwest
        positionList[7] = new Vector3(-distance, height, 0); // west
        positionList[8] = new Vector3(-distance * 0.7071f, height, distance * 0.7071f); // northwest
    }

    private void Awake()
    {
        AllServices.Container.Register<ICamera>(this);
        move = new CMove();
        view = new CMove();
        move.SetActionTime(changeTime);
        view.SetActionTime(changeTime);
        positionList = new Vector3[9];
        InitPositions();
        currentPosition = positionList[1];
    }

    private void Start()
    {
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
            transform.LookAt(viewpoint);
        }
        if (isViewLock) transform.LookAt(viewpoint);
    }

    private void OnDestroy()
    {
        AllServices.Container.UnRegister<ICamera>();
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
        transform.LookAt(viewpoint);
    }
    public void SetViewPoint(Vector3 _viewpoint)
    {
        if (_viewpoint != viewpoint)
        {
            view.SetPositions(viewpoint, _viewpoint);
            view.StartAction();
            SetPosition(_viewpoint + currentPosition);
        }
    }

    public void SetPositionInstant(Vector3 _position)
    {
        transform.position = _position;
    }

    public void SetPosition(Vector3 _position)
    {
        if (transform.position != _position)
        {
            move.SetPositions(transform.position, _position);
            move.StartAction();
        }
    }
    public void SetPosition(EMapDirection _dir)
    {
        currentPosition = positionList[(int)_dir];
        SetPosition(viewpoint + currentPosition);
        SetViewPointInstant(viewpoint);
    }

    public void SetRelativePosition(float _height, float _distance)
    {
        if (_height > 0) height = _height;
        if (distance > 0) distance = _distance;
        InitPositions();
    }

    public void SetViewLock(bool _isViewLock)
    {
        isViewLock = _isViewLock;
    }

    public bool IsBusy()
    {
        return move.IsActive() || view.IsActive();
    }
    //--------------------------
}
