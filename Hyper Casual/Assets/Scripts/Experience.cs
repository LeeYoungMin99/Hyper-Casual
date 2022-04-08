using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    [HideInInspector] public float ExperienceAmount = 0f;

    private Transform _gainer = null;

    private void Start()
    {
        ExperienceManager.Instance.ExperienceGoToGainerEvent -= GoToGainer;
        ExperienceManager.Instance.ExperienceGoToGainerEvent += GoToGainer;
    }

    public void SetExperience(Transform gainer, float amount, Vector3 position)
    {
        _gainer = gainer;

        ExperienceAmount = amount;

        transform.position = position;

        gameObject.SetActive(true);
        StartCoroutine(GoToRandomPosition());
    }

    private void GoToGainer(object sender, EventArgs args)
    {
        if (false == gameObject.activeSelf) return;

        StartCoroutine(GoToTargetPosition(_gainer));
    }

    private IEnumerator GoToTargetPosition(Transform target)
    {
        while (true)
        {
            Vector3 dir = (target.position - _rigidbody.position).normalized;

            _rigidbody.MovePosition(transform.position + dir * (Time.deltaTime * 30f));

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator GoToRandomPosition()
    {
        float y = UnityEngine.Random.Range(0f, 360f);

        transform.rotation = Quaternion.Euler(0f, y, 0f);

        float t = 0f;
        float randomTime = UnityEngine.Random.Range(0.3f, 1f);

        while (randomTime > t)
        {
            t += Time.deltaTime;

            _rigidbody.MovePosition(_rigidbody.position + transform.forward * (Time.deltaTime * 3f));

            yield return new WaitForFixedUpdate();
        }
    }
}
