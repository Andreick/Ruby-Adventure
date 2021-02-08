using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPersonCharacter : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private float displayTime;

    void Awake()
    {
        dialogBox.SetActive(false);
    }

    public void DisplayDialog()
    {
        StopAllCoroutines();

        dialogBox.SetActive(true);

        StartCoroutine(HideDialog());
    }

    IEnumerator HideDialog()
    {
        yield return new WaitForSeconds(displayTime);

        dialogBox.SetActive(false);
    }
}
