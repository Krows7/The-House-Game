using UnityEngine;

public class UnitPickFieldAssetContainer : MonoBehaviour
{
    public static UnitPickFieldAssetContainer instance;

    public Sprite baseMovementStradegyIcon;
    public Sprite followEnemyStradegyIcon;
    public Sprite preventAutoAttackIcon;
    public Sprite CycleMovementModeIcon;

    void Start()
    {
        instance = this;
    }
}
