using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;

    private float cooldownTime;
    private float cooldownRemaining;
    private bool coolingDown;

    void Start()
    {
        // Dash is ready at start → image full
        cooldownImage.fillAmount = 1f;
    }

    public void StartCooldown(float duration)
    {
        cooldownTime = duration;
        cooldownRemaining = duration;
        coolingDown = true;
        cooldownImage.fillAmount = 0f;
    }

    void Update()
    {
        if (!coolingDown) return;

        cooldownRemaining -= Time.deltaTime;
        // Fill amount increases as cooldown progresses
        cooldownImage.fillAmount = 1f - (cooldownRemaining / cooldownTime);

        if (cooldownRemaining <= 0f)
        {
            coolingDown = false;
            cooldownImage.fillAmount = 1f; // full when dash is ready
        }
    }
}