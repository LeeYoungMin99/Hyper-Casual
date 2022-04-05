using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    [SerializeField] private Transform _damageTextCanvas;
    [Header("Damage Text Prefab")]
    [SerializeField] private GameObject _damageText;

    public static DamageTextManager Instance;

    private ObjectPoolingManager<DamageText> _objectPoolingManager;

    private void Awake()
    {
        Instance = this;

        _objectPoolingManager = new ObjectPoolingManager<DamageText>(_damageText, _damageTextCanvas);
        _objectPoolingManager.CreateObjectPool(32);
    }

    public void MarkDamageText(Character character, float damage, bool isCritical)
    {
        DamageText damageText = _objectPoolingManager.GetObject();
        damageText.SetDamageText(character.transform.position, (int)damage, isCritical);
    }
}
