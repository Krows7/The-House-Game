using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWall : MonoBehaviour
{
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private float WallWidth;

    public void FillArea(GameObject WallParent = null) 
    {
        float startXCord, startYCord, finishXCord, finishYCord;
        GameObject newWall;

        // Vertical
        startXCord  = Mathf.Round(transform.position.x - Mathf.Abs(transform.localScale.x) / 2 + 1.0f) - 0.5f;
        finishXCord = Mathf.Round(transform.position.x + Mathf.Abs(transform.localScale.x) / 2 - 0.0f) - 0.5f;
        startYCord  = Mathf.Round(transform.position.y - Mathf.Abs(transform.localScale.y) / 2 + 0.5f);
        finishYCord = Mathf.Round(transform.position.y + Mathf.Abs(transform.localScale.y) / 2 - 0.5f);
        for (float xCord = startXCord; xCord <= finishXCord; ++xCord) {
            for (float yCord = startYCord; yCord <= finishYCord; ++yCord) {
                newWall = Instantiate(WallPrefab, new Vector3(xCord, yCord, 0), Quaternion.identity);
                if (WallParent) {
                    newWall.transform.SetParent(WallParent.transform);  
                }
                newWall.name = "Wall";
                newWall.transform.localScale = new Vector3(WallWidth, 1.0f + WallWidth, 0.5f);
                newWall.transform.GetChild(0).GetComponent<Renderer>().material.mainTextureScale = new Vector2 (newWall.transform.localScale.x, newWall.transform.localScale.y);
            }   
        }

        // Horizontal
        startXCord  = Mathf.Round(transform.position.x - Mathf.Abs(transform.localScale.x) / 2 + 0.5f);
        finishXCord = Mathf.Round(transform.position.x + Mathf.Abs(transform.localScale.x) / 2 - 0.5f);
        startYCord  = Mathf.Round(transform.position.y - Mathf.Abs(transform.localScale.y) / 2 + 1.0f) - 0.5f;
        finishYCord = Mathf.Round(transform.position.y + Mathf.Abs(transform.localScale.y) / 2 - 0.0f);
        for (float xCord = startXCord; xCord <= finishXCord; ++xCord) {
            for (float yCord = startYCord; yCord <= finishYCord; ++yCord) {
                newWall = Instantiate(WallPrefab, new Vector3(xCord, yCord, 0), Quaternion.identity);
                if (WallParent) {
                    newWall.transform.SetParent(WallParent.transform);  
                }
                newWall.name = "Wall";
                newWall.transform.localScale = new Vector3(1.0f + WallWidth, WallWidth, 0.5f);
                newWall.transform.GetChild(0).GetComponent<Renderer>().material.mainTextureScale = new Vector2 (newWall.transform.localScale.x, newWall.transform.localScale.y);
            }   
        }

        GameObject.Destroy(gameObject);
    }
}
