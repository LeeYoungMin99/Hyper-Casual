using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform Owner;
    [SerializeField] private Slider _slider;
    [SerializeField] private Slider _effectSlider;
    [SerializeField] private Text _text;

    private const float DELAY = 0.2f;
    private static readonly Vector3 CORRECT_POSITION_VECTER = new Vector3(0f, 2f, 0f);

    private void OnEnable()
    {
        _slider.value = 1f;
        _effectSlider.value = 1f;
    }

    private void LateUpdate()
    {
        transform.position = Owner.transform.position + CORRECT_POSITION_VECTER;
    }

    public void SetHealthBar(object sender, HealthChangeEventArgs args)
    {
        if (0 >= args.CurHealth) Destroy(gameObject, 2f);

        _slider.value = args.CurHealth / args.MaxHealth;
        _text.text = $"{(int)(args.CurHealth)}";

        StartCoroutine(SetHealthBarEffect());
    }

    private IEnumerator SetHealthBarEffect()
    {
        yield return new WaitForSeconds(DELAY);

        while (true)
        {
            _effectSlider.value = Mathf.Lerp(_effectSlider.value, _slider.value, Time.deltaTime * 10f);

            if (0.01f >= _effectSlider.value - _slider.value) yield break;

            yield return null;
        }
    }
}
