using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform Owner;
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _text;

    private static readonly Vector3 CORRECT_POSITION_VECTER = new Vector3(0f, 2f, 0f);

    private void LateUpdate()
    {
        transform.position = Owner.transform.position + CORRECT_POSITION_VECTER;
    }

    public void SetHealthBar(object sender, HealthChangeEventArgs args)
    {
        _slider.value = args.CurHealth / args.MaxHealth;
        _text.text = $"{args.CurHealth}";
    }
}
