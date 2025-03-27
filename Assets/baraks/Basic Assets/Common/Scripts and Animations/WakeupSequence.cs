using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WakeupSequence : MonoBehaviour
{
    public Image fadeScreen;
    public TMP_Text dialogueText;
    public TMP_Text tutorialPopUp;
    public Canvas wakeupCanvas; // Keep the canvas active

    private bool isUsingPC = false;

    void Start()
    {
        // Make sure the wakeup canvas is always active
        wakeupCanvas.gameObject.SetActive(true);

        // Start fully black
        fadeScreen.color = new Color(0, 0, 0, 1);
        dialogueText.text = "Ugh... I will find out who did it if it's the last thing i do";

        // Start fading in
        StartCoroutine(FadeScreen());

        // Start wake-up sequence
        StartCoroutine(WakeupRoutine());
    }

    IEnumerator FadeScreen()
    {
        float fadeDuration = 20f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            fadeScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Instead of deactivating the screen, just hide the black fade
        fadeScreen.gameObject.SetActive(false);
    }

    IEnumerator WakeupRoutine()
    {
        yield return new WaitForSeconds(5f);
        dialogueText.text = "I should probably check my PC...";

        yield return new WaitForSeconds(2f);
        tutorialPopUp.text = "Open doors with <b>[LMB]<b>, special objects interact with <b>[E]</b>";
        tutorialPopUp.gameObject.SetActive(true);
        yield return new WaitForSeconds(6f);
        tutorialPopUp.gameObject.SetActive(false);
        tutorialPopUp.gameObject.SetActive(true);
        tutorialPopUp.text = "Press <b>[E]</b> to interact with the PC. <b>[E]</b> to also exit PC";
        yield return new WaitForSeconds(7f);

    }
}
