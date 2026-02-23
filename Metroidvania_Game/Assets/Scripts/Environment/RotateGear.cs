using UnityEngine;

public class RotateGear : MonoBehaviour
{
    [SerializeField] private Transform gear;
    [SerializeField] private float gearSpeed = 30f;
    void Update()
    {
        gear.transform.Rotate(0,0,gearSpeed * Time.deltaTime);
    }
}
