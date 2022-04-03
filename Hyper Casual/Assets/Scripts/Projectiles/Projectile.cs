using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Ability[] Abilities;
    public Rigidbody Rigidbody;

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

    private void FixedUpdate()
    {
        Rigidbody.MovePosition(transform.position + (transform.forward * (MoveSpeed * Time.deltaTime)));
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isActive = false;
        bool isFreeze = false;
        bool isBruning = false;
        bool isPoisonous = false;

        int count = Abilities.Length;
        for (int i = 0; i < count; ++i)
        {
            Abilities[i].InvokeAbility(transform,
                                       other,
                                       CriticalMultiplier,
                                       CriticalRate,
                                   ref Damage,
                                   ref WallBounceCount,
                                   ref MonsterBounceCount,
                                   ref isActive,
                                   ref isFreeze,
                                   ref isBruning,
                                   ref isPoisonous);
        }

        if (LayerValue.MAP_OBJECT_LAYER != other.gameObject.layer)
        {
            other.GetComponent<IDamageable>().TakeDamage(Damage,
                                                         CriticalMultiplier,
                                                         CriticalRate,
                                                         isFreeze,
                                                         isBruning,
                                                         isPoisonous);
        }

        gameObject.SetActive(isActive);
    }
}
