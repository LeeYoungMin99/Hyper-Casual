using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public List<Ability> Abilities;

    public Transform Owner;
    public float CriticalMultiplier = 2f;
    public float CriticalRate = 0f;
    public float Damage = 0f;
    public float MoveSpeed = 20f;

    public int WallBounceCount = 0;
    public int MonsterBounceCount = 0;
    public bool IsReturning = false;

    protected Rigidbody _rigidbody;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        if (true == IsReturning)
        {
            Vector3 targetDir = (Owner.position - transform.position).normalized;

            float angle = Utils.CalculateAngle(targetDir, transform.forward);

            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + angle, 0f);

            if (0.5f >= Vector3.Distance(Owner.position, transform.position))
            {
                gameObject.SetActive(false);
                IsReturning = false;
            }
        }

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
                break;
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

    public void Init(Transform owner, float damage, float criticalMultiplier, float criticalRate, List<Ability> abilities, Vector3 position, float angle)
    {
        Owner = owner;
        Damage = damage;
        CriticalMultiplier = criticalMultiplier;
        CriticalRate = criticalRate;
        Abilities = abilities;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0f, angle, 0f));
        gameObject.SetActive(true);
    }
}
