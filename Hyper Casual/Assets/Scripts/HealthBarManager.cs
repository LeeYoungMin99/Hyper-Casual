using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EHealthBarType { Monster, Boss, Player }

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private Transform _healthBarCanvas;
    [SerializeField] private Transform _bossHealthBarCanvas;
    [Header("Health Bar Prefabs")]
    [SerializeField] private HealthBar _playerHealthBar;
    [SerializeField] private HealthBar _monsterHealthBar;
    [SerializeField] private HealthBar _bossHealthBar;

    public static HealthBarManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateHealthBar(Character character, EHealthBarType type)
    {
        HealthBar healthBar;

        switch (type)
        {
            case EHealthBarType.Monster:
                healthBar = Instantiate(_monsterHealthBar, _healthBarCanvas).GetComponent<HealthBar>();
                healthBar.Owner = character.transform;
                character.HealthChangeEvent -= healthBar.SetHealthBar;
                character.HealthChangeEvent += healthBar.SetHealthBar;
                break;
            case EHealthBarType.Boss:
                healthBar = Instantiate(_bossHealthBar, _bossHealthBarCanvas).GetComponent<HealthBar>();
                character.HealthChangeEvent -= healthBar.SetHealthBar;
                character.HealthChangeEvent += healthBar.SetHealthBar;
                break;
            case EHealthBarType.Player:
                healthBar = Instantiate(_playerHealthBar, _healthBarCanvas).GetComponent<HealthBar>();
                healthBar.Owner = character.transform;
                character.HealthChangeEvent -= healthBar.SetHealthBar;
                character.HealthChangeEvent += healthBar.SetHealthBar;
                break;
        }
    }
}
