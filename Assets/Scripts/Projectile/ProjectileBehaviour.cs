using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [Header("Must be setted")]
    [SerializeField] private float Speed;
    [SerializeField] private float DurationTime;
    [Header("Not necessary")]
    [SerializeField] private AudioClip ProjectileSound;

    private int DamageAmount;
    private Rigidbody2D ProjectileRB;

    private void Awake()
    {
        ProjectileRB = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        ProjectileRB.velocity = transform.up * Speed;
        AudioManager.Instance.PlayOneClip(ProjectileSound);
        Invoke("DestroyThisObject", DurationTime);
    }
    private void DestroyThisObject()
    {
        Destroy(gameObject);
    }
    public void SetDamageAmount(int amount)
    {
        DamageAmount = amount;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var isDamageable = collision.gameObject.GetComponent<IDamageable>();
        if(isDamageable != null)
        {
            isDamageable.TakeDamageOrHeal(DamageAmount);
        }
        DestroyThisObject();
    }
}
