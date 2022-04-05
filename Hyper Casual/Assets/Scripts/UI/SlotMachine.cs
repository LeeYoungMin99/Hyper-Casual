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

        _nameOfAbilities[EAbilityTag.AttackDamageUp] = "���ݷ� ����";
        _nameOfAbilities[EAbilityTag.AttackSppedUp] = "���ݼӵ� ����";
        _nameOfAbilities[EAbilityTag.CriticalUp]    = "ũ��Ƽ�� ����";
        _nameOfAbilities[EAbilityTag.MaxHealthUp]   = "�ִ� ü�� ����";
        _nameOfAbilities[EAbilityTag.MultiShot]     = "��Ƽ ��";
        _nameOfAbilities[EAbilityTag.FrontArrow]    = "���� ȭ�� +1";
        _nameOfAbilities[EAbilityTag.DiagonalArrows] = "�缱 ȭ�� +1";
        _nameOfAbilities[EAbilityTag.SideArrows]    = "���� ȭ�� +1";
        _nameOfAbilities[EAbilityTag.RearArrow]     = "�Ĺ� ȭ�� +1";
        _nameOfAbilities[EAbilityTag.Piercing]      = "����";
        _nameOfAbilities[EAbilityTag.Ricochet]      = "�������";
        _nameOfAbilities[EAbilityTag.BouncyWall]    = "�� �ݻ�";
        _nameOfAbilities[EAbilityTag.WallWalker]    = "�� ���� �ȴ� ��";
        _nameOfAbilities[EAbilityTag.WaterWalker]   = "���� �մ� ��";

        _descriptionOfAbilities[EAbilityTag.AttackDamageUp] = "���ݷ��� 20% �����մϴ�.";
        _descriptionOfAbilities[EAbilityTag.AttackSppedUp] = "���ݼӵ��� 25% �����մϴ�.";
        _descriptionOfAbilities[EAbilityTag.CriticalUp] = "ũ��Ƽ�� Ȯ���� 20% �����մϴ�.\nũ��Ƽ�� ������ 40% �����մϴ�.";
        _descriptionOfAbilities[EAbilityTag.MaxHealthUp] = "�ִ� ü���� 20% �����մϴ�.";
        _descriptionOfAbilities[EAbilityTag.MultiShot] = "�ѹ� �� �����մϴ�.";
        _descriptionOfAbilities[EAbilityTag.FrontArrow] = "���� ����ü�� 1�� �����մϴ�.";
        _descriptionOfAbilities[EAbilityTag.DiagonalArrows] = "���� ���� �밢������ ����ü�� 1���� �����մϴ�.";
        _descriptionOfAbilities[EAbilityTag.SideArrows] = "�������� ����ü�� 1���� �����մϴ�.";
        _descriptionOfAbilities[EAbilityTag.RearArrow] = "�Ĺ� ����ü�� 1�� �����մϴ�.";
        _descriptionOfAbilities[EAbilityTag.Piercing] = "������ ���͸� �����մϴ�.";
        _descriptionOfAbilities[EAbilityTag.Ricochet] = "������ ����ü�� �ֺ� ���͸� ���մϴ�.";
        _descriptionOfAbilities[EAbilityTag.BouncyWall] = "ȭ���� ���� ������ ƨ�ܳ��ϴ�.";
        _descriptionOfAbilities[EAbilityTag.WallWalker] = "�� ���� ���� �� �ֽ��ϴ�.";
        _descriptionOfAbilities[EAbilityTag.WaterWalker] = "���� ������ �� �ֽ��ϴ�.";

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
