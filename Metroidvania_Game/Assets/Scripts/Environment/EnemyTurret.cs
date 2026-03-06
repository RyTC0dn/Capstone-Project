using System.Collections;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Turret Setup")]
    public GameObject bullet;

    public Transform firingPoint;
    public float rateOfFire = 1f;
    private float firingRate;

    private float firingCooldown = 0;

    [Space(20)]
    [Header("Audio")]
    private AudioPlayer player;

    private AudioSource source;
    [SerializeField] private int minAudio;
    [SerializeField] private int maxAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        firingRate = rateOfFire;
        player = GetComponentInChildren<AudioPlayer>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (firingCooldown <= 0)
        {
            Instantiate(bullet, firingPoint.transform.position, transform.rotation);
            firingCooldown = 1f / firingRate;

            player.PlayRandomClip(source, minAudio, maxAudio);
        }
        firingCooldown -= Time.deltaTime;
    }
}