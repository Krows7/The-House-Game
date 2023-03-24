using UnityEngine;
using Units.Settings;

public class PostAnimationBehaviour : MonoBehaviour
{
    GameObject parent;

    void Start()
    {
        parent = transform.parent.gameObject;
    }

    public void PostAnimation()
    {
        parent.GetComponent<MovementComponent>().PostAnimation();
    }

    public void OnMovePrepared()
    {

    }

    public void OnUnitTransition()
    {
        Unit unit = parent.GetComponent<Unit>();
        parent.transform.position = SetXY(parent.transform.position, unit.Cell.transform.position);
    }

    public void OnUnitAttacked()
    {

    }

    private static Vector3 SetXY(Vector3 v, Vector3 a)
    {
        return new(a.x, a.y, v.z);
    }
}
