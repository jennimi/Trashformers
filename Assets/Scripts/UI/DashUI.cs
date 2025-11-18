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
        if (cooldownImage != null)
            cooldownImage.fillAmount = 1f; // ready at start
    }

    // Called externally (UIManager.StartDashCooldown)
    public void StartCooldown(float duration)
    {
        if (duration <= 0f)
        {
            // instantly ready
            if (cooldownImage != null) cooldownImage.fillAmount = 1f;
            coolingDown = false;
            return;
        }

        cooldownTime = duration;
        cooldownRemaining = duration;
        coolingDown = true;

        if (cooldownImage != null)
            cooldownImage.fillAmount = 0f;
    }

    // Optional: call to forcibly reset UI to ready state
    public void ResetUI()
    {
        coolingDown = false;
        cooldownRemaining = 0f;
        cooldownTime = 0f;
        if (cooldownImage != null)
            cooldownImage.fillAmount = 1f;
    }

    void Update()
    {
        if (!coolingDown) return;

        cooldownRemaining -= Time.deltaTime;
        if (cooldownTime > 0f && cooldownImage != null)
            cooldownImage.fillAmount = 1f - (cooldownRemaining / cooldownTime);

        if (cooldownRemaining <= 0f)
        {
            coolingDown = false;
            if (cooldownImage != null)
                cooldownImage.fillAmount = 1f; // full when dash is ready
        }
    }
}
