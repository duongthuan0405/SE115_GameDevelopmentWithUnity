using TMPro;
using UnityEngine;

public class HeathDisplay : UIelement
{
    [Tooltip("The text UI to use for display")]
    public TextMeshProUGUI displayText = null;
    private Health health;

    void Start()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }
    public override void UpdateUI()
    {
        base.UpdateUI();

        int iHealth = 0;
        if (health != null)
        {
            iHealth = (int)(health.currentHealth * 100f / health.maximumHealth);
        }

        if (displayText != null)
        {
            displayText.text = "Health: " + iHealth + "%";
        }
    }
}
