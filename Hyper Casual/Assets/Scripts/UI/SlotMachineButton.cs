using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineButton : MonoBehaviour
{
    private int Index;
    private SlotMachine _slotMachine;
    private Button _button;

    private void Awake()
    {
        Index = transform.GetSiblingIndex();
        _slotMachine = transform.parent.GetComponent<SlotMachine>();
        _button = GetComponent<Button>();

        _button.onClick.RemoveListener(OnClick);
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _slotMachine.OnClickSlotButton(Index);
    }
}
