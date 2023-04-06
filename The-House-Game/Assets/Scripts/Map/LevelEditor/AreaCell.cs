using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCell : MonoBehaviour
{
    [SerializeField] private GameObject DefaultCell;
    [SerializeField] private string RoomName;

    public void FillArea(GameObject MapObject = null) 
    {
        if (Mathf.Abs(transform.rotation.eulerAngles.z - 90) < 15 || Mathf.Abs(transform.rotation.eulerAngles.z - 270) < 15) {
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.x, transform.localScale.z);
        }

        GameObject RoomObject = new GameObject();
        RoomObject.transform.position = transform.position;
        RoomObject.AddComponent<Room>();
        RoomObject.name = RoomName;
        RoomObject.GetComponent<Room>().color = gameObject.GetComponent<Renderer>().material;

        if (MapObject) {
            RoomObject.transform.SetParent(MapObject.transform);
        }

        float startXCord  = Mathf.Round(transform.position.x - Mathf.Abs(transform.localScale.x) / 2 + 0.5f);
        float finishXCord = Mathf.Round(transform.position.x + Mathf.Abs(transform.localScale.x) / 2 - 0.5f);
        float startYCord  = Mathf.Round(transform.position.y - Mathf.Abs(transform.localScale.y) / 2 + 0.5f);
        float finishYCord = Mathf.Round(transform.position.y + Mathf.Abs(transform.localScale.y) / 2 - 0.5f);
        GameObject newCell;
        for (float xCord = startXCord; xCord <= finishXCord; ++xCord) {
            for (float yCord = startYCord; yCord <= finishYCord; ++yCord) {
                newCell = Instantiate(DefaultCell, new Vector3(xCord, yCord, 0), Quaternion.identity);
                newCell.transform.SetParent(RoomObject.transform);    
            }   
        }
        GameObject.Destroy(gameObject);
    }
}
