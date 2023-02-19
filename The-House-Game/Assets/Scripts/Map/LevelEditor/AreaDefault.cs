using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDefault : MonoBehaviour
{
    [SerializeField] private GameObject DefaultCell;

    void Start()
    {
        float startXCord = Mathf.Round(transform.position.x - transform.localScale.x / 2 + 0.5f);
        float finishXCord = Mathf.Round(transform.position.x + transform.localScale.x / 2 - 0.5f);
        float startYCord = Mathf.Round(transform.position.y - transform.localScale.y / 2 + 0.5f);
        float finishYCord = Mathf.Round(transform.position.y + transform.localScale.y / 2 - 0.5f);
        for (float xCord = startXCord; xCord <= finishXCord; ++xCord) {
            for (float yCord = startYCord; yCord <= finishYCord; ++yCord) {
                Instantiate(DefaultCell, new Vector3(xCord, yCord, 0), Quaternion.identity);
            }   
        }
        Destroy(gameObject);
    }
}
