# Dungeon Escape

![Dungeon Escape](https://user-images.githubusercontent.com/62818241/201535161-03ef0b6c-cc18-43f4-906f-48a174062109.png)

 ![image](https://user-images.githubusercontent.com/62818241/201535277-f36e6baf-0305-48a2-9de6-63e25c54bec0.png)

## 📌 Introduction
**Dungeon Escape** is an action-packed 2D platformer where the player navigates through various dungeon-themed levels. The game includes enemy patrols, item collection, health management, and a dynamic combat system. Players must overcome obstacles, defeat enemies, and collect gems while avoiding traps. The game features complex enemy behaviors, including patrolling, attacking, and idle states.

## 🔥 Features
- 🎮 **Enemy Patrols**: Enemies move back and forth between patrol points and change direction after reaching the edges.
- ⚔️ **Combat System**: Enemies attack players when in range, and players can take damage or defeat enemies.
- 💎 **Item Collection**: Players collect gems scattered throughout the level.
- 🏆 **Health Management**: Players and enemies have health systems with invulnerability frames and damage states.
- 🚪 **Checkpoints**: Checkpoints are activated when the player reaches specific areas, allowing for easier respawn after death.
- 🏃 **Player Movement**: Smooth movement, including ladder climbing and platform interaction.

---

## 🏗️ How It Works

The game is structured around enemy patrol points, health management, and interactive platforms. Here's how various systems are implemented:

---

### 📌 **Enemy Patrol**

Enemies patrol back and forth between designated points. When they reach the patrol edge, they change direction after a short idle period.

```csharp
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private Transform enemy;
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;
    [SerializeField] private float idleDuration;
    private float idleTimer;
    [SerializeField] private Animator anim;

    void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Update()
    {
        if (movingLeft) { if (enemy.position.x >= leftEdge.position.x) MoveInDirection(-1); else DirectionChange(); }
        else { if (enemy.position.x <= rightEdge.position.x) MoveInDirection(1); else DirectionChange(); }
    }

    private void DirectionChange()
    {
        anim.SetBool("Moving", false);
        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration) movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("Moving", true);
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) *- _direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }
}
```

---

### 📌 **Health System**

The health system allows both the player and enemies to take damage. When health reaches zero, the entity dies, and all associated components are disabled.

```csharp
public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    public void Damage(int amount)
    {
        this.health -= amount;
        if(health <= 0) Die();
    }

    private void Die()
    {
        EnemyDeath.Play();
        Destroy(gameObject);
    }

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0) { anim.SetTrigger("hurt"); StartCoroutine(Invunerability()); }
        else { if (!dead) { anim.SetTrigger("die"); foreach (Behaviour component in components) component.enabled = false; dead = true; } }
    }

    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }
}
```

---

### 📌 **Melee Enemy Combat**

Melee enemies attack the player when in range. The attack is triggered when the cooldown timer expires, and a collision with the player is detected.

```csharp
public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;
    private Health playerHealth;

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            anim.SetTrigger("meleeAttack");
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x*range, boxCollider.bounds.size.y, boxCollider.bounds.size.y),
            0, Vector2.left, 0, playerLayer);
        if (hit.collider != null) playerHealth = hit.transform.GetComponent<Health>();
        return hit.collider != null;
    }
}
```

---

### 📌 **Item Collection**

Players can collect gems in the game by colliding with gem objects. Each time a gem is collected, the total gem count increases.

```csharp
public class ItemCollector : MonoBehaviour
{
    [SerializeField] private AudioSource CollectionSoundEffect;
    private int Gems = 0;
    [SerializeField] private Text GemsText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Gems")
        {
            Destroy(collision.gameObject);
            CollectionSoundEffect.Play();
            Gems++;
            GemsText.text = "Gems: " + Gems;
        }
    }
}
```

---

### 📌 **Checkpoint System**

The checkpoint system allows players to save their progress at certain points in the dungeon. Upon reaching a checkpoint, the checkpoint flag changes color to indicate activation.

```csharp
public class CheckpointController : MonoBehaviour
{
    public Sprite redFlag;
    public Sprite greenFlag;
    private SpriteRenderer checkpointSpriteRenderer;
    public bool checkpointReached;

    void Start()
    {
        checkpointSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            checkpointSpriteRenderer.sprite = greenFlag;
            checkpointReached = true;
        }
    }
}
```

---

### 📌 **Player Ladder Movement**

This feature allows the player to climb ladders in the game using the `W` and `S` keys.

```csharp
public class LadderMovement : MonoBehaviour
{
    public Animator anim;
    public float climbingspeed = 6;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.W)) { other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, climbingspeed); }
        else if (other.tag == "Player" && Input.GetKey(KeyCode.S)) { other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -climbingspeed); }
        else { other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1); }
    }
}
```

---

## 🎯 Conclusion

**Dungeon Escape** combines elements of exploration, combat, and puzzle-solving in a dynamic dungeon environment. With its engaging mechanics like patrolling enemies, combat interactions, item collection, and checkpoints, it provides a challenging yet rewarding gameplay experience.
