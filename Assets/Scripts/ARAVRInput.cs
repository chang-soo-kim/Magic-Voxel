//#define PC
#define Oculus
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ARAVRInput
{
#if Oculus
    static Transform rootTransform;
#endif
#if Oculus
    static Transform GetTransform()
    {
        if(rootTransform == null)
        {
            rootTransform = GameObject.Find("TrackingSpace").transform;
        }
        return rootTransform;
    }
#endif
#if PC
    public enum ButtonTarget
    {
        Fire1,
        Fire2,
        Fire3,
        Jump,
    }
#endif
    public enum Button
    {
#if PC
        One = ButtonTarget.Fire1,
        Two = ButtonTarget.Jump,
        Thumbstick = ButtonTarget.Fire1,
        IndexTrigger = ButtonTarget.Fire3,
        HandTrigger = ButtonTarget.Fire2
#elif Oculus
        One = OVRInput.Button.One,
        Two = OVRInput.Button.Two,
        Thumbstick = OVRInput.Button.PrimaryThumbstick,
        IndexTrigger = OVRInput.Button.PrimaryIndexTrigger,
        HandTrigger = OVRInput.Button.PrimaryHandTrigger
#endif
    }
    public enum Controller
    {
#if PC
       LTouch,
       RTouch
#elif Oculus
        LTouch = OVRInput.Controller.LTouch,
        RTouch = OVRInput.Controller.RTouch
#endif
    }
    public static bool Get(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButton(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.Get((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif

    }
    public static bool GetDown(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonDown(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetDown((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif

    }
    public static bool GetUp(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonUp(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetUp((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif

    }

    public static float GetAxis(string axis, Controller hand = Controller.LTouch)
    {
#if PC
        return Input.GetAxis(axis);
#elif Oculus
        if (axis == "Horizontal")
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)hand).x;
        }
        else 
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)hand).y;
        }
#endif
    }

    public static void PlayVibraation(float duration, float frequency,float amplitude, Controller hand)
    {
#if Oculus
        if (CoroutineInstance.coroutineInstance ==null)
        {
            GameObject coroutineObj = new GameObject("CoroutineInstance");
            coroutineObj.AddComponent<CoroutineInstance>();
        }
        CoroutineInstance.coroutineInstance.StopAllCoroutines();
        CoroutineInstance.coroutineInstance.StartCoroutine(VibrationCoroutine(duration, frequency, amplitude, hand));
#endif
    }
    public static void PlayVibration(Controller hand)
    {
#if Oculus
        PlayVibraation(0.06f,1,1,hand);
#endif
    }
#if Oculus
    static IEnumerator VibrationCoroutine(float duration, float frequency, float amplitude, Controller hand)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            OVRInput.SetControllerVibration(frequency, amplitude, (OVRInput.Controller)hand);
            yield return null;
        }
        OVRInput.SetControllerVibration(0, 0, (OVRInput.Controller)hand);
    }
#endif
    public static void Recenter(Transform target, Vector3 direction)
    {
        target.forward = target.rotation * direction;
    }
    public static void Recenter()
    {
#if Oculus
        OVRManager.display.RecenterPose();
#endif
    }

#if PC
    static Vector3 originScale = Vector3.one * 0.02f;
#else
    static Vector3 originScale = Vector3.one * 0.005f;
#endif


    public static void DrawCrosshair(Transform crosshair, bool isHand = true, Controller hand = Controller.RTouch)
    {
        Ray ray;
        if(isHand)
        {
#if PC
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
            if (hand == Controller.RTouch)
            {
                ray = new Ray(RHandPosition, RHandDirection);
            }
            else
            {
                ray = new Ray(LHandPosition, LHandDirection);
            }
#endif
        }
        else
        {
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }
        Plane plane = new Plane(Vector3.up, 0);
        float distance = 0;
        if(plane.Raycast(ray,out distance))
        {
            crosshair.position = ray.GetPoint(distance);
            crosshair.forward = -Camera.main.transform.forward;
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }
        else
        {
            crosshair.position = ray.origin + ray.direction * 100;
            crosshair.forward = -Camera.main.transform.forward;
            distance = (crosshair.position - ray.origin).magnitude;
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }

    }
    //?????? ?????????? ???? ????????
    public static Vector3 RHandPosition
    {
        get
        {

#if PC
            Vector3 pos = Input.mousePosition;
            pos.z = 0.7f;
            pos = Camera.main.ScreenToWorldPoint(pos);
            RHand.position = pos;
            return pos;
#elif Oculus
        Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            pos = GetTransform().TransformPoint(pos);
            return pos;               
#endif

        }
    }
//?????? ?????????? ???? ????????
    public static Vector3 RHandDirection
    {
        get
        {

#if PC
            Vector3 direction = RHandPosition-Camera.main.transform.position;
            RHand.forward = direction;
            return direction;
#elif Oculus
            Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;
            direction = GetTransform().TransformPoint(direction);
            return direction;
#endif
        }
    }
//?????? ?????????? ???? ????????
    public static Vector3 LHandPosition
    {
        get
        {

#if PC
            Vector3 pos = Input.mousePosition;
            pos.z = 0.7f;
            pos = Camera.main.ScreenToWorldPoint(pos);
            LHand.position = pos;
            return pos;
#elif Oculus
            Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            pos = GetTransform().TransformPoint(pos);
            return pos;
#endif

        }
    }
    //?????? ?????????? ???? ????????
    public static Vector3 LHandDirection
    {
        get
        {

#if PC
            Vector3 direction = LHandPosition - Camera.main.transform.position;
            LHand.forward = direction;
            return direction;
#elif Oculus
            Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;
            direction = GetTransform().TransformPoint(direction);
            return direction;
#endif
        }
    }
    //?????? ?????????? ???? ????????

    static Transform lHand;
    public static Transform LHand
    {
        get
        {
            if (lHand == null)
            {
#if PC
                GameObject handObj = new GameObject("LHand");
                lHand = handObj.transform;
                lHand.parent = Camera.main.transform;
#elif Oculus
                lHand = GameObject.Find("LeftControllerAnchor").transform;
#endif
            }
            return lHand;
        }
    }
    static Transform rHand;

    public static Transform RHand
    {
        get
        {
            if (rHand==null)
            {
#if PC
                GameObject handObj = new GameObject("RHand");
                rHand = handObj.transform;
                rHand.parent = Camera.main.transform;
#elif Oculus
                lHand = GameObject.Find("RightControllerAnchor").transform;
#endif
            }
            return rHand;
        }
    }



}
class CoroutineInstance : MonoBehaviour
{
    public static CoroutineInstance coroutineInstance = null;
    private void Awake()
    {
        if (coroutineInstance == null)
        {
            coroutineInstance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}



