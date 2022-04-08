using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public List<Ability> Abilities;

    public float CriticalMultiplier = 2f;
    public float CriticalRate = 0f;
    public float Damage = 0f;
    public float MoveSpeed = 20f;

    public int WallBounceCount = 0;
    public int MonsterBounceCount = 0;

    protected Rigidbody _rigidbody;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        _rigidbody.MovePosition(transform.position + (transform.forward * (MoveSpeed * Time.deltaTime)));
    }

    protected virtual void OnDisable()
    {
        WallBounceCount = 0;
        MonsterBounceCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isActive = false;

        int count = Abilities.Count;
        for (int i = 0; i < count; ++i)
        {
            if (true == Abilities[i].InvokeAbility(this, other))
            {
                isActive = true;
            }
        }

        gameObject.SetActive(isActive);

        int layer = other.gameObject.layer;
        if (LayerValue.WALL_LAYER != layer && LayerValue.MAP_LAYER != layer)
        {
            other.GetComponent<IDamageable>().TakeDamage(Damage,
                                                         CriticalMultiplier,
                                                         CriticalRate);
        }
    }

    public void Init(float damage, float criticalMultiplier, float criticalRate, List<Ability> abilities, Vector3 position, float angle)
    {
        Damage = damage;
        CriticalMultiplier = criticalMultiplier;
        CriticalRate = criticalRate;
        Abilities = abilities;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0f, angle, 0f));
        gameObject.SetActive(true);
    }
}
