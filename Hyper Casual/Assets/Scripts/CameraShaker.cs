using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    public static CameraShaker Instance;

    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    private Coroutine _cameraShakeCoroutine;
    private float _duration = 0f;

    private void Awake()
    {
        Instance = this;

        _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float amplitueGain, float duration)
    {
        if (amplitueGain >= _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain)
        {
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitueGain;
            _duration = duration;
        }

        if (null != _cameraShakeCoroutine) return;

        StartCoroutine(ShakeCamera());
    }

    public IEnumerator ShakeCamera()
    {
        while (0 < _duration)
        {
            _duration -= Time.deltaTime;

            yield return null;
        }

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        _duration = 0f;
    }
}
