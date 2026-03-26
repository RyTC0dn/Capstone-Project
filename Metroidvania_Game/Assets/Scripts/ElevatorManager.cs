using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.Cinemachine;

public class ElevatorSaveData
{
    public List<string> registeredElevators = new List<string>();
}

public class ElevatorManager : MonoBehaviour
{
    public static ElevatorManager instance { get; private set; }

    public Dictionary<string, Elevator> elevators = new Dictionary<string, Elevator>();
    private Elevator currentElevator;
    public bool isNearElevator = false;
    public bool isActive = false;

    public GameObject parentPanel;

    public ElevatorSaveData saveData = new ElevatorSaveData();

    public GameObject elevatorFirst;

    [Space(20)]
    [Header("Camera")]
    [SerializeField]
    private float cameraSpeed;

    public Camera elevatorCam;
    [HideInInspector] public bool transitionReady = false;

    [Space(10)]
    [Header("Audio")]
    public AudioPlayer elevatorAudioPlayer;

    public AudioSource elevatorAudioSource;

    [SerializeField]
    private int minAudioValue, maxAudioValue;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("ElevatorRegistered"))
        {
            string json = PlayerPrefs.GetString("ElevatorRegistered");
            saveData = JsonUtility.FromJson<ElevatorSaveData>(json);
        }
        elevatorCam.gameObject.SetActive(false);
    }

    public void CloseUI()//Close UI on button click
    {
        //Disable event sytem controls
        EventSystem.current.SetSelectedGameObject(null);

        parentPanel.SetActive(false);
        Invoke(nameof(EnableComponent), 0.3f);
    }

    public void RegisterElevator(Elevator elevator)
    {
        if (!elevators.ContainsKey(elevator.elevatorLocationName))
        {
            elevators.Add(elevator.elevatorLocationName, elevator);

            saveData.registeredElevators.Add(elevator.elevatorLocationName);

            GameManager.instance.nextSpawnPointName = elevator.elevatorLocationName;

            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString("ElevatorRegistered", json);
            PlayerPrefs.Save();

            Debug.Log($"Registered elevator: {elevator.elevatorLocationName}");
        }
    }

    public void SetElevator(Elevator elevator)
    {
        currentElevator = elevator;
    }

    public void TeleportPlayer(string destinationName, Transform player)
    {
        if (elevators.ContainsKey(destinationName))
        {
            //Disable event system controls
            EventSystem.current.SetSelectedGameObject(null);

            Vector3 targetPos = elevators[destinationName].transform.position;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.position = targetPos;
            else
                player.position = targetPos;
            Debug.Log("Has Teleported");

            //Add transition logic here
            StopAllCoroutines();
            StartCoroutine(routine: CameraTransition(targetPos, destinationName,
                player.gameObject.GetComponent<SpriteRenderer>()));

            Invoke(nameof(EnableComponent), 0.3f);
        }
        else
        {
            Debug.LogWarning($"Destination {destinationName} not found");
        }
    }

    private IEnumerator CameraTransition(Vector3 targetPos, string destinationName, SpriteRenderer playerSp)
    {
        transitionReady = true;
        elevatorCam.gameObject.SetActive(true);

        Transform cam = elevatorCam.transform;
        Vector3 startPos = cam.position;

        float duration = cameraSpeed; //Total transition time
        float elapsed = 0f;

        Vector3 current = new Vector3(currentElevator.transform.position.x,
            currentElevator.transform.position.y,
            currentElevator.transform.position.z - 10);

        Vector3 target = new Vector3(targetPos.x, targetPos.y, targetPos.z - 10);

        playerSp.enabled = false;

        cam.position = current;

        DependentAudio(currentElevator.transform.position, targetPos);

        GameManager.instance.StateSwitch(GameStates.Pause);

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / duration;

            float smoothT = Mathf.SmoothStep(0, 1, t);

            cam.position = Vector3.Lerp(startPos, target, smoothT);

            yield return null;
        }

        //Snap to target position
        cam.position = target;

        transitionReady = false;
        yield return new WaitForEndOfFrame();
        GameManager.instance.StateSwitch(GameStates.Play);
        elevatorCam.gameObject.SetActive(transitionReady);
        playerSp.enabled = true;
        AudioTrigger(destinationName);
    }

    private void EnableComponent()
    {
        var playerAttack = FindAnyObjectByType<Player_Knight_Attack>();
        playerAttack.EnableAttack();
    }

    private void AudioTrigger(string destinationName)
    {
        //Have each audio clip equal to different floor
        if (destinationName == "Elevator_Entrance")
        {
            elevatorAudioPlayer.PlayAudio(4, elevatorAudioSource);
        }
        else if (destinationName == "Elevator_A2")
        {
            elevatorAudioPlayer.PlayAudio(0, elevatorAudioSource);
        }
        else if (destinationName == "Elevator_A3")
        {
            elevatorAudioPlayer.PlayAudio(1, elevatorAudioSource);
        }
        else if (destinationName == "Elevator_Runup")
        {
            elevatorAudioPlayer.PlayAudio(2, elevatorAudioSource);
        }
        else if (destinationName == "Elevator_Boss")
        {
            elevatorAudioPlayer.PlayAudio(3, elevatorAudioSource);
        }
    }

    private void DependentAudio(Vector3 startPos, Vector3 targetPos)
    {
        //This is to play audio depending on whether the player
        //is going up or down

        //If the start position is lower than target position
        if (startPos.y < targetPos.y)
        {
            //Play the elevator going up audio clip
            elevatorAudioPlayer.PlayAudio(7, elevatorAudioSource);
        }
        //If the start position is greater than the target position
        else if (startPos.y > targetPos.y)
        {
            //Play the elevator going down audio clip
            elevatorAudioPlayer.PlayAudio(6, elevatorAudioSource);
        }
    }
}