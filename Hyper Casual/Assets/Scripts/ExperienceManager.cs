using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    [Header("Experience Prefab")]
    [SerializeField] private GameObject _experience;
    [Space(10f)]

    public ExperienceGainer ExperienceGainer;
    public static ExperienceManager Instance;
    public event EventHandler<EventArgs> ExperienceGoToGainerEvent;

    private ObjectPoolingManager<Experience> _objectPoolingManager;

    private void Awake()
    {
        Instance = this;

        _objectPoolingManager = new ObjectPoolingManager<Experience>(_experience);
    }

    public void CreateExperience(float exp, Vector3 position)
    {
        while (0 != exp)
        {
            Experience experience = _objectPoolingManager.GetObject();

            float experienceAmount = (6f > exp) ? exp : UnityEngine.Random.Range(4f, 6f);
            exp -= experienceAmount;

            experience.SetExperience(ExperienceGainer.transform, experienceAmount, position);
        }
    }

    public void ExperienceGoToGainer(object sender, EventArgs args)
    {
        ExperienceGoToGainerEvent?.Invoke(this, EventArgs.Empty);
    }
}
