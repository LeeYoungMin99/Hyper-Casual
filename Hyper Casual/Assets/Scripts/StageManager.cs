using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject ClearMessage;
    private int _stage = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerValue.WALL_LAYER || other.gameObject.layer == LayerValue.MAP_LAYER) return;

        ++_stage;

        Vector3 newPosition = transform.position;

        newPosition.x -= 40f;
        if (0 == _stage % 4)
        {
            newPosition.z = 0f;
        }
        else
        {
            newPosition.z = Random.Range(0, 3) * -50f;
        }

        transform.position = newPosition;
        newPosition.z -= 27f;
        other.transform.position = newPosition;

        if (9 == _stage)
        {
            ClearMessage.SetActive(true);
        }
    }
}