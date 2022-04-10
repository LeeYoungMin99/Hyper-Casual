using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    [Serializable]
    public class SlotImage
    {
        public List<Image> SlotImages = new List<Image>();
    }

    [SerializeField] private GameObject _slotMachineCanvas;
    [SerializeField] private List<Button> _slotButton;
    [SerializeField] private List<GameObject> _slots;
    [SerializeField] private List<SlotImage> _slotImages;
    [SerializeField] private List<Text> _text;
    [SerializeField] private List<Sprite> _sprites;

    public event EventHandler<AbilityEventArgs> AbilityGainEvent;
    public AbilityEventArgs _abilityEventArgs = new AbilityEventArgs();
    [HideInInspector] public List<int> Result = new List<int>();

    private List<int> _rewardsEarned = new List<int>();
    private Dictionary<EAbilityTag, Sprite> _abilityTagToSprite = new Dictionary<EAbilityTag, Sprite>(new AbilityTagEnumComparer());
    private Dictionary<EAbilityTag, string> _nameOfAbilities = new Dictionary<EAbilityTag, string>(new AbilityTagEnumComparer());
    private Dictionary<EAbilityTag, string> _descriptionOfAbilities = new Dictionary<EAbilityTag, string>(new AbilityTagEnumComparer());
    private Dictionary<EAbilityTag, Ability> _abilities = new Dictionary<EAbilityTag, Ability>(new AbilityTagEnumComparer());

    private void Awake()
    {
        int count = _slotButton.Count;
        for (int i = 0; i < count; ++i)
        {
            _slotButton[i].interactable = false;
        }

        count = _sprites.Count;
        for (int i = 0; i < count; ++i)
        {
            _abilityTagToSprite[(EAbilityTag)i] = _sprites[i];
        }

        _abilities[EAbilityTag.AttackDamageUp] = new AttackDamageUp();
        _abilities[EAbilityTag.AttackSppedUp] = new AttackSpeedUp();
        _abilities[EAbilityTag.CriticalUp] = new CriticalUp();
        _abilities[EAbilityTag.MaxHealthUp] = new MaxHealthUp();
        _abilities[EAbilityTag.FrontArrow] = new FrontArrow();
        _abilities[EAbilityTag.DiagonalArrows] = new DiagonalArrows();
        _abilities[EAbilityTag.SideArrows] = new SideArrows();
        _abilities[EAbilityTag.RearArrow] = new RearArrow();
        _abilities[EAbilityTag.MultiShot] = new MultiShot();
        _abilities[EAbilityTag.Piercing] = new Piercing();
        _abilities[EAbilityTag.Ricochet] = new Ricochet();
        _abilities[EAbilityTag.BouncyWall] = new BouncyWall();
        _abilities[EAbilityTag.Blaze] = new Blaze();
        _abilities[EAbilityTag.Freeze] = new Freeze();
        _abilities[EAbilityTag.Poison] = new Poison();

        _nameOfAbilities[EAbilityTag.AttackDamageUp] = "공격력 증가";
        _nameOfAbilities[EAbilityTag.AttackSppedUp] = "공격속도 증가";
        _nameOfAbilities[EAbilityTag.CriticalUp] = "크리티컬 증가";
        _nameOfAbilities[EAbilityTag.MaxHealthUp] = "최대 체력 증가";
        _nameOfAbilities[EAbilityTag.FrontArrow] = "전방 투사체 +1";
        _nameOfAbilities[EAbilityTag.DiagonalArrows] = "사선 투사체 +1";
        _nameOfAbilities[EAbilityTag.SideArrows] = "측면 투사체 +1";
        _nameOfAbilities[EAbilityTag.RearArrow] = "후방 투사체 +1";
        _nameOfAbilities[EAbilityTag.MultiShot] = "멀티 샷";
        _nameOfAbilities[EAbilityTag.Piercing] = "관통";
        _nameOfAbilities[EAbilityTag.Ricochet] = "쓰리쿠션";
        _nameOfAbilities[EAbilityTag.BouncyWall] = "벽 반사";
        _nameOfAbilities[EAbilityTag.Blaze] = "화염";
        _nameOfAbilities[EAbilityTag.Freeze] = "얼음";
        _nameOfAbilities[EAbilityTag.Poison] = "독";

        _descriptionOfAbilities[EAbilityTag.AttackDamageUp] = "공격력이 20% 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.AttackSppedUp] = "공격속도가 25% 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.CriticalUp] = "크리티컬 확률이 20% 증가합니다.\n크리티컬 데미지 40% 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.MaxHealthUp] = "최대 체력이 20% 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.FrontArrow] = "전방 투사체가 1개 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.DiagonalArrows] = "전방 양쪽 대각선으로 투사체가 1개씩 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.SideArrows] = "양쪽으로 투사체가 1개씩 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.RearArrow] = "후방 투사체가 1개 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.MultiShot] = "한번 더 공격합니다.";
        _descriptionOfAbilities[EAbilityTag.Piercing] = "공격이 몬스터를 관통합니다.";
        _descriptionOfAbilities[EAbilityTag.Ricochet] = "명중한 투사체가 주변 몬스터를 향합니다.";
        _descriptionOfAbilities[EAbilityTag.BouncyWall] = "화살이 벽에 맞으면 튕겨납니다.";
        _descriptionOfAbilities[EAbilityTag.Blaze] = "공격이 적을 불태웁니다.";
        _descriptionOfAbilities[EAbilityTag.Freeze] = "공격이 적을 얼립니다.";
        _descriptionOfAbilities[EAbilityTag.Poison] = "공격이 적을 중독시킵니다.";

        ExperienceBar experienceBar = GameObject.Find("Canvas").transform.
                                      Find("Experience Bar").GetComponent<ExperienceBar>();

        experienceBar.LevelUpEvent -= EnableSlotMachine;
        experienceBar.LevelUpEvent += EnableSlotMachine;
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        Result.Clear();

        int count = _slotButton.Count;
        for (int i = 0; i < count; ++i)
        {
            _slotButton[i].interactable = false;
        }

        int imageCount = _abilityTagToSprite.Count;
        for (int i = 0; i < _slotImages.Count; ++i)
        {
            int randomIndex = UnityEngine.Random.Range(0, imageCount);

            count = _slotImages[i].SlotImages.Count - 1;
            for (int j = 0; j < count; ++j)
            {
                randomIndex = UnityEngine.Random.Range(0, imageCount);

                if (0 == j)
                {
                    bool overlap = false;
                    int indexCount = Result.Count;
                    int rewardsEarnedCount = _rewardsEarned.Count;

                    do
                    {
                        randomIndex = UnityEngine.Random.Range(0, imageCount);

                        overlap = false;

                        for (int k = 0; k < indexCount; ++k)
                        {
                            if (randomIndex == Result[k])
                            {
                                overlap = true;

                                break;
                            }
                        }

                        for (int k = 0; k < rewardsEarnedCount; ++k)
                        {
                            if (randomIndex == _rewardsEarned[k])
                            {
                                overlap = true;

                                break;
                            }
                        }
                    }
                    while (true == overlap);

                    Result.Add(randomIndex);
                }

                _slotImages[i].SlotImages[j].sprite = _abilityTagToSprite[(EAbilityTag)randomIndex];
            }

            _slotImages[i].SlotImages[_slotImages[i].SlotImages.Count - 1].sprite = _slotImages[i].SlotImages[0].sprite;
        }

        count = _slots.Count;
        for (int i = 0; i < count; ++i)
        {
            StartCoroutine(StartRotateSlotMachine(i));
        }
    }

    public void OnClickSlotButton(int index)
    {
        _abilityEventArgs.Ability = _abilities[(EAbilityTag)Result[index]];

        if (Result[index] >= (int)EAbilityTag.MultiShot)
        {
            _rewardsEarned.Add(Result[index]);
        }

        AbilityGainEvent?.Invoke(this, _abilityEventArgs);

        _slotMachineCanvas.SetActive(false);

        Time.timeScale = 1f;

        int count = _slots.Count;
        for (int i = 0; i < count; ++i)
        {
            _text[i].text = null;
        }
    }

    private IEnumerator StartRotateSlotMachine(int slotIndex)
    {
        int rotateCount = 120 + slotIndex * 60;

        for (int i = 0; i < rotateCount; ++i)
        {
            _slots[slotIndex].transform.localPosition -= new Vector3(0f, 40f, 0f);

            if (0 >= _slots[slotIndex].transform.localPosition.y)
            {
                _slots[slotIndex].transform.localPosition += new Vector3(0f, 300f, 0f);
            }

            yield return null;
        }

        int count = _slots.Count;
        if (slotIndex == count - 1)
        {
            for (int i = 0; i < count; ++i)
            {
                _slotButton[i].interactable = true;
                _text[i].text = _nameOfAbilities[(EAbilityTag)Result[i]];
                _text[i].gameObject.SetActive(true);
            }
        }
    }

    private void EnableSlotMachine(object sender, EventArgs args)
    {
        _slotMachineCanvas.SetActive(true);

        _abilityEventArgs.Ability = _abilities[EAbilityTag.MaxHealthUp];
        AbilityGainEvent?.Invoke(this, _abilityEventArgs);
    }
}
