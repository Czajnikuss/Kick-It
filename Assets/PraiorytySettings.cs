using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PraiorytySettings : MonoBehaviour
{
    public CinemachineVirtualCamera ballCam, tartgetCam, pizzaCam, kickSignCam
    ;
    // Start is called before the first frame update
public void SetBallPrioryty(int prioryty)
{
    ballCam.Priority = prioryty;
}
public void SetTargetPrioryty(int prioryty)
{
    tartgetCam.Priority = prioryty;
}
public void SetPizzaPrioryty(int prioryty)
{
    pizzaCam.Priority = prioryty;
}
public void SetKickSignPrioryty(int prioryty)
{
    kickSignCam.Priority = prioryty;
}

}
