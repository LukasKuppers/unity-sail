using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenOptionsButton : MonoBehaviour
{
    [SerializeField]
    private GameObject menuModalPrefab;
    [SerializeField]
    private GameObject hudRoot;
    [SerializeField]
    private int optionsPageIndex;

    private void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OpenOptionsModal);
    }

    private void OpenOptionsModal()
    {
        GameObject modal = Instantiate(menuModalPrefab, hudRoot.transform);
        modal.GetComponent<GameMenuModal>().InitAsSinglePage(optionsPageIndex);
    }
}
