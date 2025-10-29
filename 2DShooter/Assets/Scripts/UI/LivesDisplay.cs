using TMPro;
using UnityEngine;

public class LivesDisplay : UIelement
{
    private Health health;
    public TextMeshProUGUI displayText;
    void Start()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UpdateUI()
    {
        base.UpdateUI();

        if(health != null && displayText != null)
        {
            displayText.text = "Lives: " + health.currentLives.ToString();
        }
    }
}
