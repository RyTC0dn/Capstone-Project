using UnityEngine;

public class PrototypePlayerAttack : MonoBehaviour
{
    private Transform spawnPos; //Storing the position of the spawnpoint of weapon
    public GameObject weapon; //Variable storing weapon object
    [SerializeField]
    private float activeTimer = 1.0f;

    private bool isUnsheathed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPos = GetComponent<Transform>();
        spawnPos.position = transform.position;

        weapon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        WeaponUnsheath();
    }

    private void WeaponUnsheath()
    {
        if (Input.GetMouseButton(0))
        {
            weapon.SetActive(true);
            isUnsheathed = true;
        }
        else if(isUnsheathed)
        {
            activeTimer -= Time.deltaTime;
        }
        if(activeTimer <= 0)
        {
            weapon.SetActive(false );
            isUnsheathed=false;
            activeTimer = 2;    
        }

    }
}
