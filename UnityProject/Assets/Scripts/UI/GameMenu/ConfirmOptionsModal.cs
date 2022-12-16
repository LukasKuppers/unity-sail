using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConfirmOptionsModal : MonoBehaviour
{
    private static readonly float TIMEOUT_TIME = 5.0f;

    [SerializeField]
    private GameObject applyChangesButtonObject;
    [SerializeField]
    private GameObject timerTextObect;

    private Button applyChangesButton;
    private TextMeshProUGUI timerText;

    private OptionsPage optionsPage;

    private void Awake()
    {
        applyChangesButton = applyChangesButtonObject.GetComponent<Button>();
        applyChangesButton.onClick.AddListener(ApplyChanges);

        StartCoroutine(RevertChangesTimer(TIMEOUT_TIME, () =>
        {
            optionsPage.RevertDisplaySettings();
            Destroy(gameObject);
        }));
    }

    public void InitParameters(GameObject optionsPageObject)
    {
        optionsPage = optionsPageObject.GetComponent<OptionsPage>();
    }

    private void ApplyChanges()
    {
        optionsPage.CloseOptions();
        Destroy(gameObject);
    }

    private IEnumerator RevertChangesTimer(float time, UnityAction onTimeoutCall)
    {
        float timer = 0f;
        while (timer < time)
        {
            int displayTime = (int)Mathf.Ceil(time - timer);
            timerText.text = $"{displayTime} seconds";

            timer += Time.deltaTime;
            yield return null;
        }

        onTimeoutCall.Invoke();
    }
}
