using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWall : MonoBehaviour
{
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private float WallWidth = 0.2f;

    public void FillArea(GameObject WallParent = null, GameObject anotherWallPrefab = null) 
    {
        float startXCord, startYCord, finishXCord, finishYCord;

        if (Mathf.Abs(transform.rotation.eulerAngles.z - 90) < 15 || Mathf.Abs(transform.rotation.eulerAngles.z - 270) < 15) {
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.x, transform.localScale.z);
        }

        // Vertical
        startXCord  = Mathf.Round(transform.position.x - Mathf.Abs(transform.localScale.x) / 2 + 1.0f) - 0.5f;
        finishXCord = Mathf.Round(transform.position.x + Mathf.Abs(transform.localScale.x) / 2 - 0.0f) - 0.5f;
        startYCord  = Mathf.Round(transform.position.y - Mathf.Abs(transform.localScale.y) / 2 + 0.5f);
        finishYCord = Mathf.Round(transform.position.y + Mathf.Abs(transform.localScale.y) / 2 - 0.5f);
        for (float xCord = startXCord; xCord <= finishXCord; ++xCord) {
            for (float yCord = startYCord; yCord <= finishYCord; ++yCord) {
                SetWall(new Vector3(xCord, yCord, 0), "vertical", WallParent);
            }   
        }

        // Horizontal
        startXCord  = Mathf.Round(transform.position.x - Mathf.Abs(transform.localScale.x) / 2 + 0.5f);
        finishXCord = Mathf.Round(transform.position.x + Mathf.Abs(transform.localScale.x) / 2 - 0.5f);
        startYCord  = Mathf.Round(transform.position.y - Mathf.Abs(transform.localScale.y) / 2 + 1.0f) - 0.5f;
        finishYCord = Mathf.Round(transform.position.y + Mathf.Abs(transform.localScale.y) / 2 - 0.0f);
        for (float xCord = startXCord; xCord <= finishXCord; ++xCord) {
            for (float yCord = startYCord; yCord <= finishYCord; ++yCord) {
                SetWall(new Vector3(xCord, yCord, 0), "horizontal", WallParent);
            }   
        }

        GameObject.Destroy(gameObject);
    }

    public void SetWall(Vector3 WallPosition, string type = "horizontal", GameObject WallParent = null, GameObject anotherWallPrefab = null) {
        GameObject newWall;
        if (anotherWallPrefab) {
            WallPrefab = anotherWallPrefab;
        }
        newWall = Instantiate(WallPrefab, WallPosition, Quaternion.identity);
        if (WallParent) {
            newWall.transform.SetParent(WallParent.transform);  
        }
        newWall.name = "Wall";
        if (type == "horizontal") {
            newWall.transform.localScale = new Vector3(1.0f + WallWidth, WallWidth, 0.5f);
        }
        else {
            newWall.transform.localScale = new Vector3(WallWidth, 1.0f + WallWidth, 0.5f);
        }
        newWall.transform.GetChild(0).GetComponent<Renderer>().material.mainTextureScale = new Vector2 (newWall.transform.localScale.x, newWall.transform.localScale.y);
    }
}
