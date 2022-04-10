using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private List<Monster> _monsters = new List<Monster>();
    [SerializeField] private GameObject _door;

    private int _monsterCount = 0;

    private event EventHandler<EventArgs> StageClearEvent;

    private const string STAGE_CLEAR = "StageClear";
    private void Awake()
    {
        _monsterCount = _monsters.Count;

        for (int i = 0; i < _monsterCount; ++i)
        {
            _monsters[i].DeathEvent -= DecreaseMonster;
            _monsters[i].DeathEvent += DecreaseMonster;
        }

        StageClearEvent -= ExperienceManager.Instance.ExperienceGoToGainer;
        StageClearEvent += ExperienceManager.Instance.ExperienceGoToGainer;
        StageClearEvent -= ExperienceManager.Instance.ExperienceGainer.EnableCollider;
        StageClearEvent += ExperienceManager.Instance.ExperienceGainer.EnableCollider;
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < _monsterCount; ++i)
        {
            _monsters[i].gameObject.SetActive(true);
        }
    }

    private void DecreaseMonster(object sender, EventArgs args)
    {
        --_monsterCount;

        if (0 >= _monsterCount)
        {
            Invoke(STAGE_CLEAR, 2f);
        }
    }

    private void StageClear()
    {

        _door.SetActive(false);
        StageClearEvent.Invoke(this, EventArgs.Empty);
    }
}
