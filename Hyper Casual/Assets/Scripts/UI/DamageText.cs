using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField] private Text _text;

    public void SetDamageText(Vector3 position, int damage, bool isCritical)
    {
        gameObject.SetActive(true);

        transform.position = position + GetRandomPosition();

        _text.text = $"-{damage}";

        if (true == isCritical)
        {
            _text.color = Color.red;
        }
        else
        {
            _text.color = Color.white;
        }
    }

    private void DisableDamageText()
    {
        gameObject.SetActive(false);
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(4.5f, 5.5f);

        return new Vector3(x, y, 0f);
    }
}
