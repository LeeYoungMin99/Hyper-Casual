using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Ability[] Abilities;
    public Rigidbody Rigidbody;
    public LayerMask LayerMask;

    public float CriticalMultiplier = 2f;
    public float CriticalRate = 0f;
    public float Damage = 0f;
    public float MoveSpeed = 100f;

    public int WallBounceCount = 0;
    public int MonsterBounceCount = 0;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        WallBounceCount = 0;
        MonsterBounceCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (6 != other.gameObject.layer)
        {
            other.GetComponent<IDamageable>().TakeDamage(Damage);
        }

        bool isActive = false;

        int count = Abilities.Length;

        for (int i = 0; i < count; ++i)
        {
            Abilities[i].InvokeAbility(other, CriticalMultiplier, CriticalRate, ref Damage, ref WallBounceCount, ref MonsterBounceCount, ref isActive);
        }

        gameObject.SetActive(isActive);
    }

    private void FixedUpdate()
    {
        Rigidbody.velocity = transform.forward * (MoveSpeed * Time.deltaTime);
    }
}
