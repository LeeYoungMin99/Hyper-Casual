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

    private Dictionary<EAbilityTag, Sprite> _abilityTagToSprite = new Dictionary<EAbilityTag, Sprite>(new EnumComparer());
    private Dictionary<EAbilityTag, string> _nameOfAbilities = new Dictionary<EAbilityTag, string>(new EnumComparer());
    private Dictionary<EAbilityTag, string> _descriptionOfAbilities = new Dictionary<EAbilityTag, string>(new EnumComparer());
    private Dictionary<EAbilityTag, Ability> _abilities = new Dictionary<EAbilityTag, Ability>(new EnumComparer());

    public List<int> Result = new List<int>();

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

        //_abilities[EAbilityTag.AttackDamageUp]= new Ability()
        //_abilities[EAbilityTag.AttackSppedUp] = 
        //_abilities[EAbilityTag.CriticalUp]    = 
        //_abilities[EAbilityTag.MaxHealthUp]   = 
        //_abilities[EAbilityTag.MultiShot]     = 
        //_abilities[EAbilityTag.FrontArrow]    = 
        //_abilities[EAbilityTag.DiagonalArrows]= 
        //_abilities[EAbilityTag.SideArrows]    = 
        //_abilities[EAbilityTag.RearArrow]     = 
        //_abilities[EAbilityTag.Piercing]      = 
        //_abilities[EAbilityTag.Ricochet]      = 
        //_abilities[EAbilityTag.BouncyWall]    = 
        //_abilities[EAbilityTag.WallWalker]    = 
        //_abilities[EAbilityTag.WaterWalker]   = 

        _nameOfAbilities[EAbilityTag.AttackDamageUp] = "공격력 증가";
        _nameOfAbilities[EAbilityTag.AttackSppedUp] = "공격속도 증가";
        _nameOfAbilities[EAbilityTag.CriticalUp]    = "크리티컬 증가";
        _nameOfAbilities[EAbilityTag.MaxHealthUp]   = "최대 체력 증가";
        _nameOfAbilities[EAbilityTag.MultiShot]     = "멀티 샷";
        _nameOfAbilities[EAbilityTag.FrontArrow]    = "전방 화살 +1";
        _nameOfAbilities[EAbilityTag.DiagonalArrows] = "사선 화살 +1";
        _nameOfAbilities[EAbilityTag.SideArrows]    = "측면 화살 +1";
        _nameOfAbilities[EAbilityTag.RearArrow]     = "후방 화살 +1";
        _nameOfAbilities[EAbilityTag.Piercing]      = "관통";
        _nameOfAbilities[EAbilityTag.Ricochet]      = "쓰리쿠션";
        _nameOfAbilities[EAbilityTag.BouncyWall]    = "벽 반사";
        _nameOfAbilities[EAbilityTag.WallWalker]    = "물 위를 걷는 자";
        _nameOfAbilities[EAbilityTag.WaterWalker]   = "벽을 뚫는 자";

        _descriptionOfAbilities[EAbilityTag.AttackDamageUp] = "공격력이 20% 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.AttackSppedUp] = "공격속도가 25% 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.CriticalUp] = "크리티컬 확률이 20% 증가합니다.\n크리티컬 데미지 40% 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.MaxHealthUp] = "최대 체력이 20% 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.MultiShot] = "한번 더 공격합니다.";
        _descriptionOfAbilities[EAbilityTag.FrontArrow] = "전방 투사체가 1개 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.DiagonalArrows] = "전방 양쪽 대각선으로 투사체가 1개씩 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.SideArrows] = "양쪽으로 투사체가 1개씩 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.RearArrow] = "후방 투사체가 1개 증가합니다.";
        _descriptionOfAbilities[EAbilityTag.Piercing] = "공격이 몬스터를 관통합니다.";
        _descriptionOfAbilities[EAbilityTag.Ricochet] = "명중한 투사체가 주변 몬스터를 향합니다.";
        _descriptionOfAbilities[EAbilityTag.BouncyWall] = "화살이 벽에 맞으면 튕겨납니다.";
        _descriptionOfAbilities[EAbilityTag.WallWalker] = "물 위를 걸을 수 있습니다.";
        _descriptionOfAbilities[EAbilityTag.WaterWalker] = "벽을 관통할 수 있습니다.";

        ExperienceBar experienceBar = GameObject.Find("Canvas").transform.
                                      Find("Experience Bar").GetComponent<ExperienceBar>();

        experienceBar.LevelUpEvent -= EnableSlotMachine;
        experienceBar.LevelUpEvent += EnableSlotMachine;
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;

        int count = _slotButton.Count;
        for (int i = 0; i < count; ++i)
        {
            _slotButton[i].interactable = false;
        }

        int imageCount = _abilityTagToSprite.Count;
        for (int i = 0; i < _slotImages.Count; ++i)
        {
            int randomIndex;

            count = _slotImages[i].SlotImages.Count - 1;
            for (int j = 0; j < count; ++j)
            {
                randomIndex = UnityEngine.Random.Range(0, imageCount);

                int indexCount = Result.Count;
                if (0 == j)
                {
                    for (int k = 0; k < indexCount; ++k)
                    {
                        while (randomIndex == Result[k])
                        {
                            randomIndex = UnityEngine.Random.Range(0, imageCount);
                        }
                    }

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
        _abilityEventArgs.Ability = (EAbilityTag)Result[index];

        AbilityGainEvent?.Invoke(this, _abilityEventArgs);

        _slotMachineCanvas.SetActive(false);

        Time.timeScale = 1f;
    }

    private IEnumerator StartRotateSlotMachine(int slotIndex)
    {
        int rotateCount = 60 + slotIndex * 60;

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
    }
}
