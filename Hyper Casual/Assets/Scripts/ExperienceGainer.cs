using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGainer : MonoBehaviour
{
    public event EventHandler<ExperienceEventArgs> GainExperienceEvent;

    private ExperienceEventArgs _experienceEvent = new ExperienceEventArgs();
    private Collider _collider;

    private const string DISABLE_COLLIDER = "DisableCollider";

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _experienceEvent.ExperienceAmount = other.GetComponent<Experience>().ExperienceAmount;
        other.gameObject.SetActive(false);

        GainExperienceEvent?.Invoke(this, _experienceEvent);
    }

    public void EnableCollider(object sender, EventArgs args)
    {
        _collider.enabled = true;

        Invoke(DISABLE_COLLIDER, 5f);
    }

    private void DisableCollider()
    {
        _collider.enabled = false;
    }
}
