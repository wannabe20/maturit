using UnityEngine;
using UnityEngine.AI;

public class KidnapperAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing = false;

    public float chaseRange = 10f; // Vzdálenost, na kterou detekuje hráèe
    public float lostSightTime = 5f; // Po jak dlouhé dobì pøestane hledat hráèe
    public float catchDistance = 1.5f; // Vzdálenost pro chycení hráèe
    public float footstepHearDistance = 15f; // Vzdálenost, do které jsou slyšet kroky

    private float timeSinceLastSeenPlayer = 0f;

    [Header("Audio Settings")]
    public AudioSource footstepAudio;

    private bool playerCaught = false; // Kontrola, zda už byl hráè chycen

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (playerCaught) return; // Pokud byl hráè chycen, AI už nic nedìlá

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        CheckFootstepAudio();

        // Kontrola vzdálenosti k hráèi
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
            Debug.Log("Unášeè spatøil hráèe!");
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
                Debug.Log("Unášeè ztratil hráèe. Pokraèuje v patrolování.");
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
        if (playerCaught) return; // Ochrana proti opakovanému spuštìní

        playerCaught = true;
        Debug.Log("Hráè byl chycen! Game Over.");

        // Deaktivace pohybu hráèe
        CharacterController playerController = player.GetComponent<CharacterController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Zavolání Game Over obrazovky
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
        else
        {
            Debug.LogError("GameOverManager nebyl nalezen ve scénì!");
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
