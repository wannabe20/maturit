using UnityEngine;
using UnityEngine.AI;

public class KidnapperAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing = false;

    public float chaseRange = 10f; // Vzd�lenost, na kterou detekuje hr��e
    public float lostSightTime = 5f; // Po jak dlouh� dob� p�estane hledat hr��e
    public float catchDistance = 1.5f; // Vzd�lenost pro chycen� hr��e
    public float footstepHearDistance = 15f; // Vzd�lenost, do kter� jsou sly�et kroky

    private float timeSinceLastSeenPlayer = 0f;

    [Header("Audio Settings")]
    public AudioSource footstepAudio;

    private bool playerCaught = false; // Kontrola, zda u� byl hr�� chycen

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (playerCaught) return; // Pokud byl hr�� chycen, AI u� nic ned�l�

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        CheckFootstepAudio();

        // Kontrola vzd�lenosti k hr��i
        if (Vector3.Distance(transform.position, player.position) < catchDistance)
        {
            CatchPlayer();
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }

        if (Vector3.Distance(transform.position, player.position) < chaseRange)
        {
            isChasing = true;
            Debug.Log("Un�e� spat�il hr��e!");
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) > chaseRange)
        {
            timeSinceLastSeenPlayer += Time.deltaTime;

            if (timeSinceLastSeenPlayer > lostSightTime)
            {
                isChasing = false;
                Debug.Log("Un�e� ztratil hr��e. Pokra�uje v patrolov�n�.");
                GoToNextPatrolPoint();
            }
        }
        else
        {
            timeSinceLastSeenPlayer = 0f;
        }
    }

    void CatchPlayer()
    {
        if (playerCaught) return; // Ochrana proti opakovan�mu spu�t�n�

        playerCaught = true;
        Debug.Log("Hr�� byl chycen! Game Over.");

        // Deaktivace pohybu hr��e
        CharacterController playerController = player.GetComponent<CharacterController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Zavol�n� Game Over obrazovky
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
        else
        {
            Debug.LogError("GameOverManager nebyl nalezen ve sc�n�!");
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void CheckFootstepAudio()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= footstepHearDistance)
        {
            float volume = Mathf.Clamp01(1 - (distanceToPlayer / footstepHearDistance));
            footstepAudio.volume = volume;

            if (!footstepAudio.isPlaying)
                footstepAudio.Play();
        }
        else
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
        }
    }
}
