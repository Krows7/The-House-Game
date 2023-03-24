using UnityEngine;

public class UnitPickFieldAssetContainer : MonoBehaviour
{
    public static UnitPickFieldAssetContainer instance;

    public Sprite baseMovementStradegyIcon;
    public Sprite followEnemyStradegyIcon;

    void Start()
    {
        instance = this;
    }
}
