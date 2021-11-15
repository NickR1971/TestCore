using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraController : MonoBehaviour
{
    private ICamera iCamera;
    private IInputController iInputController;
    private bool isLock = true;
    private float speed = 10.0f;

    private   void Start()
    {
        iCamera = AllServices.Container.Get<ICamera>();
        iInputController = AllServices.Container.Get<IInputController>();
        iCamera.SetPosition(EMapDirection.south);
    }

    private void Update()
    {
        if (iCamera.IsBusy()) return;
        if (iInputController.IsPressed(MyButton.Rstick))
        {
            isLock = !isLock;
            iCamera.SetViewLock(!isLock);
            if (isLock)
            {
                iCamera.SetViewPointInstant(transform.position + new Vector3(0, -5, 5));
            }
        }

        iInputController.GetRightStick(out float h, out float v);

        float offsetH = h * speed * Time.deltaTime;
        float offsetV = v * speed * Time.deltaTime;
        Vector3 pos = new Vector3(offsetH, 0, offsetV);
        iCamera.SetPositionInstant(transform.position + pos);
    }
}
