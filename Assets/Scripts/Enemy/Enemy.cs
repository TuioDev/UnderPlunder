using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int MaxHealth;
    [SerializeField] private float FireProjectileCooldown;
    [SerializeField] private int ProjectileDamage;
    [SerializeField] private bool IsBoss = false;
    [Header("Audio")]
    [SerializeField] private AudioClip Explosion;
    [Header("References")]
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private ProjectileBehaviour Projectile;
    [SerializeField] private Transform AutoFireSpawnPoint;

    private Health EnemyHealth = new Health();
    private Animator EnemyAnim;
    private Collider2D EnemyCollider;
    private float FireTimer = 0f;

    private void Awake()
    {
        this.enabled = false;
        EnemyHealth.SetAmount(MaxHealth);
        PlayerTransform = Player.Instance.transform;
        EnemyAnim = GetComponent<Animator>();
        EnemyCollider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (PlayerTransform != null)
        {
            //Face the player
            transform.up = PlayerTransform.position - transform.position;

            //Shoot at the player
            ShootProjectile();
        }
    }
    private void ShootProjectile()
    {
        FireTimer += Time.deltaTime;
        if (FireTimer > FireProjectileCooldown)
        {
            FireTimer = 0f;
            ProjectileBehaviour projectile = Instantiate(Projectile, AutoFireSpawnPoint.position, transform.rotation);
            projectile.SetDamageAmount(ProjectileDamage);
            if (IsBoss)
            {
                projectile.transform.localScale = this.transform.localScale;
                Vector3 AutoFireSecondPoint = new Vector3(-AutoFireSpawnPoint.localPosition.x, AutoFireSpawnPoint.localPosition.y, 0);
                AutoFireSecondPoint = transform.TransformPoint(AutoFireSecondPoint);
                ProjectileBehaviour projectile2 = Instantiate(Projectile, AutoFireSecondPoint, transform.rotation);
                projectile2.SetDamageAmount(ProjectileDamage);
                projectile2.transform.localScale = this.transform.localScale;
            }
        }
    }
    private void OnBecameInvisible()
    {
        GetComponent<Enemy>().enabled = false;
    }

    private void OnBecameVisible()
    {
        GetComponent<Enemy>().enabled = true;
    }

    public void TakeDamageOrHeal(int damage)
    {
        EnemyHealth.TakeDamage(damage);
        if (EnemyHealth.Amount <= 0)
        {
            OnKill();
            return;
        }
        OnHit();
    }
    private void OnHit()
    {
        if (EnemyAnim != null && EnemyHealth.Amount >= 1)
        {
            EnemyAnim.SetTrigger("WasHit");
        }
    }
    private void OnKill()
    {
        EnemyAnim.SetTrigger("IsDead");
        EnemyCollider.enabled = false;
        AudioManager.Instance.PlayOneClip(Explosion);
        this.enabled = false;
    }
    //By the end of the animation and event calls this method
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}