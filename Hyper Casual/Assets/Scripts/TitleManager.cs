using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _equipmentViewButton;
    [SerializeField] private Button _titleViewButton;
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private Button[] _weaponButtons;
    [SerializeField] private GameObject _camera;
    public EWeaponType EquipmentWeapon;

    private static readonly Vector3 EQUIPMENT_VIEW_POSITION = new Vector3(0, 1f, -10f);
    private static readonly Quaternion EQUIPMENT_VIEW_ROTATION = Quaternion.Euler(0f, 0f, 0f);
    private static readonly Vector3 TITLE_VIEW_POSITION = new Vector3(-3f, 1f, -10f);
    private static readonly Quaternion TITLE_VIEW_ROTATION = Quaternion.Euler(0f, 90f, 0f);

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);

        DontDestroyOnLoad(gameObject);

        EquipmentWeapon = EWeaponType.Knife;

        _weapons[(int)EquipmentWeapon].SetActive(true);

        _equipmentViewButton.onClick.RemoveListener(SetEquimentView);
        _equipmentViewButton.onClick.AddListener(SetEquimentView);

        _titleViewButton.onClick.RemoveListener(SetTitleView);
        _titleViewButton.onClick.AddListener(SetTitleView);

        _gameStartButton.onClick.RemoveListener(GameStart);
        _gameStartButton.onClick.AddListener(GameStart);
    }

    public void ChangeWeapon(int index)
    {
        _weapons[(int)EquipmentWeapon].SetActive(false);
        _weapons[index].SetActive(true);

        EquipmentWeapon = (EWeaponType)index;
    }

    private void GameStart()
    {
        SceneManager.LoadScene(1);

        Screen.SetResolution(1080, 1920, true);
    }

    private void SetEquimentView()
    {
        StartCoroutine(SetViewHelper(TITLE_VIEW_POSITION, EQUIPMENT_VIEW_POSITION,
                                     TITLE_VIEW_ROTATION, EQUIPMENT_VIEW_ROTATION,
                                     _titleViewButton.gameObject));

        _equipmentViewButton.gameObject.SetActive(false);
    }

    private void SetTitleView()
    {
        StartCoroutine(SetViewHelper(EQUIPMENT_VIEW_POSITION, TITLE_VIEW_POSITION,
                                     EQUIPMENT_VIEW_ROTATION, TITLE_VIEW_ROTATION,
                                     _equipmentViewButton.gameObject));

        _titleViewButton.gameObject.SetActive(false);
    }

    private IEnumerator SetViewHelper(Vector3 av, Vector3 bv, Quaternion aq, Quaternion bq, GameObject button)
    {
        float elapseTime = 0f;

        while (1f > elapseTime)
        {
            elapseTime += Time.deltaTime;

            Vector3 newPosition = Vector3.Lerp(av, bv, Mathf.Clamp01(elapseTime));
            Quaternion newRotation = Quaternion.Lerp(aq, bq, Mathf.Clamp01(elapseTime));

            _camera.transform.position = newPosition;
            _camera.transform.rotation = newRotation;

            yield return null;
        }

        button.SetActive(true);
    }
}
