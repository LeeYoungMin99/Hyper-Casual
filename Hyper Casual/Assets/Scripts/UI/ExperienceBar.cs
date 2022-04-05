using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _levelText;

    public event EventHandler<EventArgs> LevelUpEvent;

    private Coroutine _setValueCoroutine;
    private float _maxExperience = 50f;
    private float _curExperience = 0f;
    private float _experienceGained = 0f;
    private int _level = 1;

    private void Start()
    {
        ExperienceManager.Instance.ExperienceGainer.GainExperienceEvent -= GainExperience;
        ExperienceManager.Instance.ExperienceGainer.GainExperienceEvent += GainExperience;
        LevelUpEvent?.Invoke(this, EventArgs.Empty);
    }

    public void GainExperience(object sender, ExperienceEventArgs args)
    {
        _experienceGained += args.ExperienceAmount;

        if (null != _setValueCoroutine) return;

        _setValueCoroutine = StartCoroutine(SetExperienceBar());
    }

    private IEnumerator SetExperienceBar()
    {
        while (0 != _experienceGained)
        {
            float curExperienceToGain;

            curExperienceToGain = (1f > _experienceGained) ? _experienceGained : Mathf.Lerp(0, _experienceGained, Time.deltaTime * 10f);

            _experienceGained -= curExperienceToGain;
            _curExperience += curExperienceToGain;

            if (_curExperience >= _maxExperience)
            {
                _curExperience -= _maxExperience;
                _maxExperience *= 2f;

                ++_level;
                _levelText.text = $"Lv.{_level}";

                LevelUpEvent?.Invoke(this, EventArgs.Empty);
            }

            float value = _curExperience / _maxExperience;

            _slider.value = value;

            yield return null;
        }

        _setValueCoroutine = null;
    }
}
