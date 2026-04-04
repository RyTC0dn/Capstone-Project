using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.Cinemachine;
using TMPro;

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
    private float transitionDuration;

    public Camera elevatorCam;

    [HideInInspector]
    public bool transitionReady = false;

    [Space(10)]
    [Header("Audio")]
    public AudioPlayer elevatorAudioPlayer;

    public AudioSource elevatorAudioSource;

    [SerializeField]
    private int minAudioValue, maxAudioValue;

    [Tooltip("Select to have the audio clips overlap " +
        "when moving up or down in elevator")]
    public bool overlapAudioDependent = false;

    [Tooltip("Select to have the audio clips overlap as " +
        "the camera transitions to the next elevator")]
    public bool overlapAudioIndependent = false;

    public GameObject textPopup;
    public TextMeshProUGUI popupText;
    public string message;
    public int inputCount = 0;

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
        popupText.text = message;
        textPopup.SetActive(false);
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

        float duration = transitionDuration; //Total transition time
        float elapsed = 0f;

        Vector3 current = new Vector3(currentElevator.transform.position.x,
            currentElevator.transform.position.y,
            currentElevator.transform.position.z - 10);

        //Grab the target position elevator and offset camera position
        Vector3 target = new Vector3(targetPos.x, targetPos.y, targetPos.z - 10);

        playerSp.enabled = false;

        cam.position = current;

        DependentAudio(currentElevator.transform.position, targetPos);

        GameManager.instance.StateSwitch(GameStates.Pause);

        //While the elapsed time is less than the duration, keep transitioning the camera
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / duration;

            //Use SmoothStep for smoother transition
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
        elevatorAudioPlayer.PlayAudio(8, elevatorAudioSource, overlapAudioIndependent);

        //Have each audio clip equal to different floor
        if (destinationName == "Ground Floor")
        {
            elevatorAudioPlayer.PlayAudio(4, elevatorAudioSource, overlapAudioIndependent);
        }
        else if (destinationName == "Floor 1")
        {
            elevatorAudioPlayer.PlayAudio(0, elevatorAudioSource, overlapAudioIndependent);
        }
        else if (destinationName == "Floor 2")
        {
            elevatorAudioPlayer.PlayAudio(1, elevatorAudioSource, overlapAudioIndependent);
        }
        else if (destinationName == "Runup Floor")
        {
            elevatorAudioPlayer.PlayAudio(2, elevatorAudioSource, overlapAudioIndependent);
        }
        else if (destinationName == "Boss Floor")
        {
            elevatorAudioPlayer.PlayAudio(3, elevatorAudioSource, overlapAudioIndependent);
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
            elevatorAudioPlayer.PlayAudio(7, elevatorAudioSource, overlapAudioDependent);
        }
        //If the start position is greater than the target position
        else if (startPos.y > targetPos.y)
        {
            //Play the elevator going down audio clip
            elevatorAudioPlayer.PlayAudio(6, elevatorAudioSource, overlapAudioDependent);
        }
    }
}