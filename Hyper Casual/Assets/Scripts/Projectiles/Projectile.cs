using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private delegate void OnFixedUpdateAbility();
    private delegate void OnInvokeAbility(Collider other);
    private OnFixedUpdateAbility[] _onFixedUpdateAbilityFunction = new OnFixedUpdateAbility[2];
    private OnInvokeAbility[] _onInvokeAbilityFunction = new OnInvokeAbility[8];

    private List<Ability> _abilities;
    private Rigidbody _rigidbody;
    private Transform _owner;
    private float _criticalMultiplier = 2f;
    private float _criticalRate = 0f;
    private float _damage = 0f;
    private float _moveSpeed = 20f;
    private int _wallBounceCount = 0;
    private int _monsterBounceCount = 0;
    private bool _isActive = false;
    private bool _isReturning = false;
    private int _targetLayerMask;
    private float _homingElapsedTime;
    private const float HOMING_TIME = 0.2f;
    private const int MAX_BOUNCE_COUNT = 3;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _targetLayerMask = (LayerValue.FRIENDLY_PROJECTILE == gameObject.layer) ? LayerValue.ALL_ENEMY_LAYER_MASK : LayerValue.ALL_PLAYER_LAYER_MASK;

        _onInvokeAbilityFunction[(int)EAbilityTag.Homing] = null;
        _onInvokeAbilityFunction[(int)EAbilityTag.Boomerang] = Boomerang;
        _onInvokeAbilityFunction[(int)EAbilityTag.Piercing] = Piercing;
        _onInvokeAbilityFunction[(int)EAbilityTag.Ricochet] = Ricochet;
        _onInvokeAbilityFunction[(int)EAbilityTag.BouncyWall] = BouncyWall;
        _onInvokeAbilityFunction[(int)EAbilityTag.Blaze] = Blaze;
        _onInvokeAbilityFunction[(int)EAbilityTag.Freeze] = Freeze;
        _onInvokeAbilityFunction[(int)EAbilityTag.Poison] = Poison;

        _onFixedUpdateAbilityFunction[0] = FixedUpdateHoming;
        _onFixedUpdateAbilityFunction[1] = FixedUpdateBoomerang;
    }

    private void FixedUpdate()
    {
        int count = _abilities.Count;
        for (int i = 0; i < count; ++i)
        {
            _abilities[i].FixedUpdateAbility(this);
        }

        _rigidbody.MovePosition(transform.position + (transform.forward * (_moveSpeed * Time.deltaTime)));
    }

    private void OnDisable()
    {
        _wallBounceCount = 0;
        _monsterBounceCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        _isActive = false;

        int count = _abilities.Count;
        for (int i = 0; i < count; ++i)
        {
            _abilities[i].InvokeAbility(this, other);
        }

        gameObject.SetActive(_isActive);

        int layer = other.gameObject.layer;
        if (LayerValue.WALL_LAYER != layer && LayerValue.MAP_LAYER != layer)
        {
            other.GetComponent<IDamageable>().TakeDamage(_damage,
                                                         _criticalMultiplier,
                                                         _criticalRate);
        }
    }

    public void Init(Transform owner, float damage, float criticalMultiplier, float criticalRate, List<Ability> abilities, Vector3 position, float angle)
    {
        _owner = owner;
        _damage = damage;
        _criticalMultiplier = criticalMultiplier;
        _criticalRate = criticalRate;
        _abilities = abilities;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0f, angle, 0f));
        gameObject.SetActive(true);
    }

    public void InvokeAbility(EAbilityTag tag, Collider other)
    {
        _onInvokeAbilityFunction[(int)tag]?.Invoke(other);
    }

    public void FixedUpdateAbility(EAbilityTag tag)
    {
        if ((int)tag >= _onFixedUpdateAbilityFunction.Length) return;

        _onFixedUpdateAbilityFunction[(int)tag]?.Invoke();
    }

    private void FixedUpdateHoming()
    {
        int count = Physics.OverlapSphereNonAlloc(_rigidbody.position, 3f, Utils.Colliders, _targetLayerMask);

        if (0 != count && HOMING_TIME > _homingElapsedTime)
        {
            Collider target = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < count; ++i)
            {
                float distance = Vector3.Distance(Utils.Colliders[i].transform.position, transform.position);

                if (minDistance <= distance) continue;

                Vector3 dir = (Utils.Colliders[i].transform.position - transform.position).normalized;

                float dot = Mathf.Clamp(Vector3.Dot(transform.forward, dir), -1f, 1f);

                if (0f >= dot) continue;

                minDistance = distance;
                target = Utils.Colliders[i];
            }

            if (target != null)
            {
                _homingElapsedTime += Time.deltaTime;

                Vector3 dir = target.attachedRigidbody.position - _rigidbody.position;
                Quaternion dirQuat = Quaternion.LookRotation(dir);
                Quaternion moveQuat = Quaternion.Slerp(_rigidbody.rotation, dirQuat, Time.deltaTime * 20f);

                _rigidbody.MoveRotation(moveQuat);
            }
        }
    }

    private void Boomerang(Collider other)
    {
        _isActive = true;

        int layer = other.gameObject.layer;
        if (LayerValue.WALL_LAYER != layer && LayerValue.MAP_LAYER != layer) return;

        if (0 < _wallBounceCount && _wallBounceCount > 3) return;

        _isReturning = true;
    }

    private void FixedUpdateBoomerang()
    {
        if (true == _isReturning)
        {
            Vector3 targetDir = (_owner.position - transform.position).normalized;

            float angle = Utils.CalculateAngle(targetDir, transform.forward);

            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + angle, 0f);

            if (0.5f >= Vector3.Distance(_owner.position, transform.position))
            {
                gameObject.SetActive(false);
                _isReturning = false;
            }
        }
    }

    private void Ricochet(Collider other)
    {
        int layer = other.gameObject.layer;
        if (LayerValue.WALL_LAYER == layer || LayerValue.MAP_LAYER == layer) return;

        if (MAX_BOUNCE_COUNT <= _monsterBounceCount) return;

        int count = Physics.OverlapSphereNonAlloc(other.transform.position, 10f, Utils.Colliders, LayerValue.ALL_ENEMY_LAYER_MASK);

        if (count == 0) return;

        ++_monsterBounceCount;

        _damage *= 0.7f;

        _isActive = true;

        transform.position += transform.forward * (Time.deltaTime * 20f);

        for (int i = 0; i < count; ++i)
        {
            if (other == Utils.Colliders[i]) continue;

            Vector3 targetDir = (Utils.Colliders[i].transform.position - transform.position).normalized;

            float angle = Utils.CalculateAngle(targetDir, transform.forward);

            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + angle, 0f);

            break;
        }
    }

    private void Piercing(Collider other)
    {
        int layer = other.gameObject.layer;
        if (LayerValue.WALL_LAYER == layer || LayerValue.MAP_LAYER == layer) return;

        int bounceCount = _monsterBounceCount;
        if (0 < bounceCount && bounceCount < 3) return;

        _isActive = true;

        _damage *= 0.67f;
    }

    private void BouncyWall(Collider other)
    {
        int layer = other.gameObject.layer;
        if (LayerValue.WALL_LAYER != layer && LayerValue.MAP_LAYER != layer) return;

        if (MAX_BOUNCE_COUNT <= _wallBounceCount) return;

        _isActive = true;

        ++_wallBounceCount;

        _damage *= 0.5f;

        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, 2000f, LayerValue.WALL_AND_MAP_LAYER_MASK);

        Vector3 reflect = Vector3.Reflect(transform.forward, hit.normal).normalized;

        float angle = Utils.CalculateAngle(reflect, transform.forward);

        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + angle, 0f);
    }

    private void Blaze(Collider other)
    {
        int layer = other.gameObject.layer;
        if (LayerValue.WALL_LAYER == layer || LayerValue.MAP_LAYER == layer) return;

        Character hitCharacter = other.GetComponent<Character>();
        BlazeEffect statusEffet = hitCharacter.GetStatusEffect<BlazeEffect>();
        if (null == statusEffet)
        {
            statusEffet = new BlazeEffect();

            hitCharacter.AddStatusEffect(statusEffet);
        }

        statusEffet.Activate(hitCharacter,
                             2f,
                             _damage,
                             _criticalMultiplier,
                             _criticalRate);
    }

    private void Freeze(Collider other)
    {
        int layer = other.gameObject.layer;
        if (LayerValue.WALL_LAYER == layer || LayerValue.MAP_LAYER == layer) return;

        Character hitCharacter = other.GetComponent<Character>();
        FreezeEffect statusEffet = hitCharacter.GetStatusEffect<FreezeEffect>();
        if (null == statusEffet)
        {
            statusEffet = new FreezeEffect();

            hitCharacter.AddStatusEffect(statusEffet);
        }

        statusEffet.Activate(hitCharacter, 2f, 0f, 0f, 0f);
    }

    private void Poison(Collider other)
    {
        int layer = other.gameObject.layer;
        if (LayerValue.WALL_LAYER == layer || LayerValue.MAP_LAYER == layer) return;

        Character hitCharacter = other.GetComponent<Character>();
        PoisonEffect statusEffet = hitCharacter.GetStatusEffect<PoisonEffect>();
        if (null == statusEffet)
        {
            statusEffet = new PoisonEffect();

            hitCharacter.AddStatusEffect(statusEffet);
        }

        statusEffet.Activate(hitCharacter,
                             0f,
                             _damage,
                             _criticalMultiplier,
                             _criticalRate);
    }
}
