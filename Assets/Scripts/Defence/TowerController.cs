using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerController : MonoBehaviour
{
    // Stat variables
    public int health = 2; // Initial health of the tower
    private int currentHealth;
    public int damage = 1; // Damage amount to be dealt to the enemy
    public float range = 2f; // Range within which the tower deals damage to enemies
    public float damageInterval = 1f; // Time interval between damage applications
    public int maxTargets = 1; // Maximum number of enemies the tower can damage at once
    private int currentLevel = 0;

    // Reference variables
    public bool isObstacle = false;
    public int cost = 5; // Cost to place the tower
    public int upgradeCost = 5; // Cost to upgrade the tower
    private int healCost = 1;
    public bool isPreview = false;
    public GameObject rangeIndicator;

    // Upgrade tab variables
    private Label towerName;
    private Label levelLabel;
    private Label healthValue;
    private Label damageValue;
    private Label upgradeCostLabel;
    private Label healCostLabel;


    private float damageCooldown;
    private PlayerController playerController;

    void Start()
    {
        damageCooldown = 0f;
        currentHealth = health;
        UpdateRangeIndicator();
    }

    void Update()
    {
        if (!isObstacle)
        {
            GameObject nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    rotation,
                    Time.deltaTime * 2
                );
            }

            damageCooldown -= Time.deltaTime;

            if (damageCooldown <= 0f)
            {
                DealDamageToEnemiesInRange();
                damageCooldown = damageInterval;
            }
        }
    }

    public bool TowerPlaced()
    {
        GameObject player = GameObject.Find("PlayerController");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();

            // Subtract cost from player currency when tower is placed
            if (playerController != null && !playerController.SubtractCurrency(cost))
            {
                Destroy(gameObject);
                return false;
            }
        }
        return true;
    }

    public void setIsPreview()
    {
        this.isPreview = true;
        this.gameObject.tag = "Preview";
        ShowRangeIndicator();
    }

    public void isSelected(VisualElement upgradeTab)
    {
        // Display tab
        upgradeTab.style.display = DisplayStyle.Flex;

        towerName = upgradeTab.Q<Label>("TowerName");
        levelLabel = upgradeTab.Q<Label>("LevelLabel");
        healthValue = upgradeTab.Q<Label>("HealthValue");
        damageValue = upgradeTab.Q<Label>("DamageValue");
        upgradeCostLabel = upgradeTab.Q<Label>("UpgradeCost");
        healCostLabel = upgradeTab.Q<Label>("HealCost");

        Button upgradeButton = upgradeTab.Q<Button>("UpgradeButton");
        upgradeButton.clicked += handleUpgrade;

        Button healButton = upgradeTab.Q<Button>("HealButton");
        healButton.clicked += handleHeal;

        refreshUpgradeTabValues();
    }

    private void handleUpgrade()
    {
        // Max level is 10 
        if (this.currentLevel == 9)
        {
            return;
        }

        if (playerController != null && !playerController.SubtractCurrency(upgradeCost))
        {
            Debug.Log("Not Enough");
            return;
        }

        ++this.currentLevel;

        // Increase health
        this.health = (int)(this.currentLevel * 15) + this.health;
        this.currentHealth = this.health;
        setHealthValue();

        // Increase damage
        this.damage = (int)(this.currentLevel * 10) + this.damage;

        // If level 5 increase max targets
        if (this.currentLevel == 4)
        {
            this.maxTargets = this.maxTargets * 2;
        }

        // If level 10 increase range
        if (this.currentLevel == 9)
        {
            this.range = this.range * 1.5f;
        }

        // Increase upgrade cost for next upgrade
        this.upgradeCost = 5 + (5 * this.currentLevel);

        refreshUpgradeTabValues();
    }

    private void handleHeal()
    {
        if (playerController != null && !playerController.SubtractCurrency(healCost))
        {
            Debug.Log("Not Enough");
            return;
        }

        this.currentHealth = this.health;
        setHealthValue();
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void setHealthValue()
    {
        if (healthValue != null)
        {
            healthValue.text = "Health: " + currentHealth.ToString() + " / " + health.ToString();
        }
    }

    private void refreshUpgradeTabValues()
    {
        towerName.text = "BOBOBO"; // TODO: Implement real tower names
        levelLabel.text = "Level " + (this.currentLevel + 1).ToString();
        setHealthValue();
        damageValue.text = "Damage: " + this.damage.ToString();
        upgradeCostLabel.text = "Cost: " + this.upgradeCost.ToString();
        healCostLabel.text = "Cost: " + this.healCost.ToString();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        this.setHealthValue();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void DealDamageToEnemiesInRange()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range);
        Animator animator = gameObject.GetComponent<Animator>();

        int targetsDamaged = 0;

        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy") && targetsDamaged < maxTargets)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
                EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.TakeDamage(damage);
                    targetsDamaged++;
                }
            }
        }

        if (animator != null && targetsDamaged == 0)
        {
            animator.ResetTrigger("Attack");
        }
    }

    void ShowRangeIndicator()
    {
        if (rangeIndicator != null)
        {
            rangeIndicator.SetActive(true);
        }
    }

    void UpdateRangeIndicator()
    {
        if (rangeIndicator != null)
        {
            rangeIndicator.transform.localScale = new Vector3(range * 2, 0, range * 2); // Adjust scale based on range
        }
    }
}
