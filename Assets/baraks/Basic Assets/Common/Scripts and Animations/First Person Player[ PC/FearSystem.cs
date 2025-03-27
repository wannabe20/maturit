using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FearSystem : MonoBehaviour
{
    [Header("Fear Settings")]
    public float fearLevel = 0f;
    public float fearIncreaseRate = 2f;
    public float fearDecreaseRate = 1f;
    public float maxFear = 100f;

    private bool isMoving = false;
    private float lastMoveTime;

    [Header("Audio Effects")]
    public AudioSource whisperSource;
    public AudioClip whisperSound;
    private bool isAfraid = false;

    [Header("Visual Effects")]
    public PostProcessVolume postProcessing;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;

    void Start()
    {
        if (postProcessing != null)
        {
            postProcessing.profile.TryGetSettings(out vignette);
            postProcessing.profile.TryGetSettings(out chromaticAberration);
        }

        lastMoveTime = Time.time;
    }

    void Update()
    {
        HandleFear();
        UpdateEffects();
    }

    void HandleFear()
    {
        // Detekce pohybu hr��e
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            isMoving = true;
            lastMoveTime = Time.time;
        }
        else
        {
            isMoving = false;
        }

        // Pokud se hr�� neh�be d�le ne� 10 sekund, strach za�ne r�st
        if (!isMoving && Time.time - lastMoveTime > 10f)
        {
            fearLevel += fearIncreaseRate * Time.deltaTime;
        }
        else
        {
            fearLevel -= fearDecreaseRate * Time.deltaTime;
        }

        // Omez�me hodnotu strachu mezi 0 a maxFear
        fearLevel = Mathf.Clamp(fearLevel, 0, maxFear);

        // Pokud strach dos�hne maxima, hra kon��
        if (fearLevel >= maxFear)
        {
            Collapse();
        }
    }

    void UpdateEffects()
    {
        float effectIntensity = fearLevel / maxFear;

        if (vignette != null)
            vignette.intensity.value = effectIntensity;

        if (chromaticAberration != null)
            chromaticAberration.intensity.value = effectIntensity;

        // Spu�t�n� whisper zvuku p�i vysok�m strachu
        if (fearLevel > 30f && !isAfraid)
        {
            isAfraid = true;
            whisperSource.clip = whisperSound;
            whisperSource.loop = true;
            whisperSource.Play();
        }
        else if (fearLevel < 30f && isAfraid)
        {
            isAfraid = false;
            whisperSource.Stop();
        }
    }

    void Collapse()
    {
        Debug.Log("Hr�� zkolaboval ze strachu! Game Over.");
        FindObjectOfType<GameOverManager>().ShowGameOver();
    }
}
