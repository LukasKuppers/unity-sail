using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject healthText;

    private ShipPrefabManager shipManager;
    private IDamageable playerHealth;

    private Slider healthSlider;
    private TextMeshProUGUI text;

    private void Start()
    {
        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        shipManager.AddSpawnListener(UpdatePlayerHealth);

        healthSlider = gameObject.GetComponent<Slider>();
        text = healthText.GetComponent<TextMeshProUGUI>();
    }

    private void UpdatePlayerHealth()
    {
        GameObject currentShip = shipManager.GetCurrentShip();
        playerHealth = currentShip.GetComponent<IDamageable>();
    }

    private void Update()
    {
        if (playerHealth != null)
        {
            float health = playerHealth.GetHealth();
            float maxHealth = playerHealth.GetMaxHealth();

            healthSlider.value = health / maxHealth;

            string healthStr = ((int)health).ToString();
            string maxHealthStr = ((int)maxHealth).ToString();
            text.SetText(healthStr + "/" + maxHealthStr);
        }
    }
}
