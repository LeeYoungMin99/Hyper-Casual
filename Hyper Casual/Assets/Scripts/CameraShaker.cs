using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraShaker
{
    public static readonly CameraShaker Instance = new CameraShaker();

    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private CameraShaker()
    {
        _cinemachineBasicMultiChannelPerlin = GameObject.Find("Virtual Camera")
                                                        .GetComponent<CinemachineVirtualCamera>()
                                                        .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator ShakeCamera(float amplitueGain)
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitueGain;

        yield return new WaitForSeconds(1f);

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
    }
}
