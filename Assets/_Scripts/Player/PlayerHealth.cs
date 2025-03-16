using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public PlayerStats playerStats; // Gán ScriptableObject chứa thông tin nhân vật

    public Slider healthSlider;
    public Slider shieldSlider;

    private PlayerController playerController; // Tham chiếu đến PlayerController nhằm lôi cái ChangeAnimationState ra
    public bool isDead {  get; private set; }

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        playerStats.Health = playerStats.MaxHealth;
        playerStats.Shield = playerStats.MaxShield;

        UpdateHealthUI();
        UpdateShieldUI();

        isDead = false;
    }

    private void FixedUpdate()
    {
        if(playerStats.Health <= 0)
        {
            playerStats.Health = 0;
            isDead = true;
            StartCoroutine(Die());
        }
    }

    public void TakeDamage(float damage)
    {
        float remainingDamage = damage;

        // Nếu có Shield, trừ Shield trước
        if (playerStats.Shield > 0)
        {
            float shieldAbsorb = Mathf.Min(playerStats.Shield, remainingDamage);
            playerStats.Shield -= shieldAbsorb;
            remainingDamage -= shieldAbsorb;

            UpdateShieldUI();

            // Nếu Shield hấp thụ hết damage, thì không cần trừ máu
            if (remainingDamage <= 0) return;
        }

        // Nếu còn sát thương, trừ vào Health nhưng có Armor giảm sát thương
        if (remainingDamage > 0)
        {
            int damageReduction = Mathf.FloorToInt(remainingDamage * (playerStats.Armor / 100f)); // Armor giảm %
            remainingDamage -= damageReduction; // Giảm sát thương thực tế

            playerStats.Health -= remainingDamage;

            //Hết máu thì cook
            if (playerStats.Health <= 0)
            {
                playerStats.Health = 0;
                //Die();
            }
        }

        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        playerStats.Health = Mathf.Min(playerStats.Health + amount, playerStats.MaxHealth);
        UpdateHealthUI();
    }

    public void RechargeShield(int amount)
    {
        playerStats.Shield = Mathf.Min(playerStats.Shield + amount, playerStats.MaxShield);
        UpdateShieldUI();
    }

    void UpdateHealthUI()
    {
        healthSlider.maxValue = playerStats.MaxHealth;
        healthSlider.value = playerStats.Health;
    }

    void UpdateShieldUI()
    {
        if (playerStats.Shield > 0)
        {
            shieldSlider.gameObject.SetActive(true);
            shieldSlider.maxValue = playerStats.MaxShield;
            shieldSlider.value = playerStats.Shield;
        }
        else
        {
            shieldSlider.gameObject.SetActive(false);
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        Debug.Log("Player đã chết!");

        // Gọi animation chết từ PlayerController
        playerController.ChangeAnimationState(AnimationState.Die);

        // Chờ animation chạy xong
        yield return new WaitForSeconds(1f);

        // Hủy Player
        Destroy(gameObject);
    }
}
