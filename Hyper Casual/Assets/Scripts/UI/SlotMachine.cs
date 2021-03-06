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
    [SerializeField] private ExperienceBar _experienceBar;

    public event EventHandler<AbilityEventArgs> AbilityGainEvent;
    public AbilityEventArgs _abilityEventArgs = new AbilityEventArgs();
    [HideInInspector] public List<int> Result = new List<int>();

    private List<int> _rewardsEarned = new List<int>();
    private Dictionary<EAbilityTag, Sprite> _abilityTagToSprite = new Dictionary<EAbilityTag, Sprite>(new AbilityTagEnumComparer());
    private Dictionary<EAbilityTag, string> _nameOfAbilities = new Dictionary<EAbilityTag, string>(new AbilityTagEnumComparer());
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
            _abilityTagToSprite[(EAbilityTag)i + 2] = _sprites[i];
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

        _nameOfAbilities[EAbilityTag.AttackDamageUp] = "?????? ????";
        _nameOfAbilities[EAbilityTag.AttackSppedUp] = "???????? ????";
        _nameOfAbilities[EAbilityTag.CriticalUp] = "???????? ????";
        _nameOfAbilities[EAbilityTag.MaxHealthUp] = "???? ???? ????";
        _nameOfAbilities[EAbilityTag.FrontArrow] = "???? ?????? +1";
        _nameOfAbilities[EAbilityTag.DiagonalArrows] = "???? ?????? +1";
        _nameOfAbilities[EAbilityTag.SideArrows] = "???? ?????? +1";
        _nameOfAbilities[EAbilityTag.RearArrow] = "???? ?????? +1";
        _nameOfAbilities[EAbilityTag.MultiShot] = "???? ??";
        _nameOfAbilities[EAbilityTag.Piercing] = "????";
        _nameOfAbilities[EAbilityTag.Ricochet] = "????????";
        _nameOfAbilities[EAbilityTag.BouncyWall] = "?? ????";
        _nameOfAbilities[EAbilityTag.Blaze] = "????";
        _nameOfAbilities[EAbilityTag.Freeze] = "????";
        _nameOfAbilities[EAbilityTag.Poison] = "??";

        _experienceBar.LevelUpEvent -= EnableSlotMachine;
        _experienceBar.LevelUpEvent += EnableSlotMachine;
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
            int randomIndex;

            count = _slotImages[i].SlotImages.Count - 1;
            for (int j = 0; j < count; ++j)
            {
                randomIndex = UnityEngine.Random.Range(2, imageCount + 2);

                if (0 == j)
                {
                    bool overlap;
                    int indexCount = Result.Count;
                    int rewardsEarnedCount = _rewardsEarned.Count;

                    do
                    {
                        randomIndex = UnityEngine.Random.Range(2, imageCount + 2);

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

        if (Result[index] <= (int)EAbilityTag.MultiShot)
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
