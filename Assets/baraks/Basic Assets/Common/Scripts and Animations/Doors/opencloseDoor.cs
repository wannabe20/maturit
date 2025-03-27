using System.Collections;
using UnityEngine;

namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public bool open;
        public Transform Player;
        public Transform Kidnapper;
        private bool isAIInside = false;

        [Header("Auto Close Settings")]
        public bool closeBehindPlayer = true; // If true, the door will close behind the player
        public float autoCloseDelay = 2f; // Time before door closes after player enters

        void Start()
        {
            open = false;
        }

        void OnMouseOver()
        {
            if (Player)
            {
                float dist = Vector3.Distance(Player.position, transform.position);
                if (dist < 3f && Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Player clicked the door.");
                    if (open == false)
                        StartCoroutine(opening());
                    else
                        StartCoroutine(closing());
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Kidnapper"))
            {
                isAIInside = true;
                Debug.Log("AI entered the door trigger.");
                if (open == false)
                    StartCoroutine(opening());
            }
            else if (other.CompareTag("Player") && closeBehindPlayer)
            {
                Debug.Log("Player entered the door trigger.");
                if (open == false)
                {
                    StartCoroutine(AutoCloseAfterDelay());
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Kidnapper"))
            {
                isAIInside = false;
                Debug.Log("AI exited the door trigger.");
                StartCoroutine(closing());
            }
            else if (other.CompareTag("Player"))
            {
                Debug.Log("Player exited the door trigger.");
                if (closeBehindPlayer)
                    StartCoroutine(AutoCloseAfterDelay());
            }
        }

        IEnumerator opening()
        {
            Debug.Log("Opening door...");
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(.5f);
        }

        IEnumerator closing()
        {
            Debug.Log("Closing door...");
            if (isAIInside == false)
            {
                openandclose.Play("Closing");
                open = false;
                yield return new WaitForSeconds(.5f);
            }
        }

        IEnumerator AutoCloseAfterDelay()
        {
            Debug.Log("Waiting for auto close...");
            yield return new WaitForSeconds(autoCloseDelay);
            if (!isAIInside)
                StartCoroutine(closing());
            if (!Player)
                StartCoroutine(closing());
        }
    }
}
