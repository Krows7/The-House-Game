using UnityEngine;
using Units.Settings;

public class PostAnimationBehaviour : MonoBehaviour
{
    public void PostAnimation()
    {
        Debug.LogWarning("Post Animation");
        GetComponentInParent<MovementComponent>().PostAnimation();
    }

    public void OnUnitTransition()
    {
        Unit unit = GetComponentInParent<Unit>();
        transform.parent.position = SetXY(transform.parent.position, unit.Cell.transform.position);
    }

    private static Vector3 SetXY(Vector3 v, Vector3 a)
    {
        return new(a.x, a.y, v.z);
    }
}
