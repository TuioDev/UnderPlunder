using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Assets.Scripts.States;

public class Player : MonoBehaviour, IDamageable
{
    private static Player _Instance;

    public static Player Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<Player>();
            }
            return _Instance;
        }
    }
    [Header("Movement Stats")]
    [SerializeField] private int MaxHealth;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float ImpulseSpeed;
    [SerializeField] private float ImpulseCooldown;
    [SerializeField] private float FireProjectileCooldown;
    [Header("Damage Stats")]
    [SerializeField] private int ProjectileDamage;
    [SerializeField] private int ContactDamage;
    [Header("Input configuration")]
    [SerializeField] private KeyCode MoveUp;
    [SerializeField] private KeyCode RotateRight;
    [SerializeField] private KeyCode RotateLeft;
    [SerializeField] private KeyCode ImpulseRight;
    [SerializeField] private KeyCode ImpulseLeft;
    [Header("References")]
    [SerializeField] private SpriteRenderer PlayerSR;
    [SerializeField] private ProjectileBehaviour ProjectilePrefab;
    [SerializeField] private Transform AutoFireSpawnPoint;
    [Header("Audio")]
    [SerializeField] private AudioClip Explosion;
    [Header("UI")]
    [SerializeField] private Image UI_ImpulseFill;
    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI UI_Health;
    [SerializeField] private TextMeshProUGUI UI_ImpulseTimer;
    [SerializeField] private TextMeshProUGUI UI_Crystals;

    public static event Action<bool> OnGameOver;
    public static event Action<bool> OnVictory;

    private VisualState CurrentState;
    private Health PlayerHealth = new Health();
    private Animator PlayerAnim;
    private Rigidbody2D PlayerRB;
    private CapsuleCollider2D PlayerCollider;
    private GameManager ReferenceGameManager;
    private int PlayerCrystals = 0;
    private float RotationSide;
    private float FireTimer = 0f;
    private float ImpulseTimer;
    private bool CanRotate = true;
    private bool CanBoost = true;
    private bool CanFire = false;
    private bool OnImpulseCD = false;
    private bool IsImpulseRight = false;
    private bool IsImpulseLeft = false;

    private void Awake()
    {
        PlayerSR = GetComponent<SpriteRenderer>();
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
        PlayerAnim = GetComponentInChildren<Animator>();
        ReferenceGameManager = FindObjectOfType<GameManager>();
        PlayerHealth.SetAmount(MaxHealth);
        ImpulseTimer = ImpulseCooldown;
    }
    private void Start()
    {
        UpdateUI();
        AutoFire();
    }
    private void Update()
    {
        //CurrentState.Execute();
        CheckInputs();
        ShootProjectile();
    }
    private void FixedUpdate()
    {
        RotateBody();
        MoveBody();
        LimitSpeed();
    }
    public void SetState(VisualState newState)
    {
        CurrentState = newState;
    }
    public void UpdateUI()
    {
        UI_Health.text = PlayerHealth.Amount.ToString();
        UI_Crystals.text = PlayerCrystals.ToString();
    }
    private void RotateBody()
    {
        if (CanRotate)
        {
            PlayerRB.rotation -= RotationSpeed * RotationSide * Time.deltaTime;
            CanRotate = false;
        }
    }
    private void MoveBody()
    {
        //Player move up
        if (CanBoost)
        {
            PlayerRB.AddForce(transform.up * MovementSpeed);
            CanBoost = false;
        }
        //Impulse Cooldown
        if (OnImpulseCD)
        {
            ApplyImpulseTimer();
        }
    }
    private void ImpulseBody()
    {
        if (OnImpulseCD)
        {
            //Display message/audio "Impulse is on cooldown"
            Debug.Log("Impulse is on cooldown!");
        }
        else
        {
            if (IsImpulseRight)
            {
                PlayerRB.AddForce(transform.right * ImpulseSpeed);
                IsImpulseRight = false;
            }
            else if (IsImpulseLeft)
            {
                PlayerRB.AddForce(-transform.right * ImpulseSpeed);
                IsImpulseLeft = false;
            }
            OnImpulseCD = true;
            UI_ImpulseTimer.gameObject.SetActive(true);
            UI_ImpulseFill.gameObject.SetActive(true);
            ImpulseTimer = ImpulseCooldown;
        }
    }
    private void ApplyImpulseTimer()
    {
        ImpulseTimer -= Time.deltaTime;

        if (ImpulseTimer < 0.0f)
        {
            //Set the objects back to normal
            OnImpulseCD = false;
            UI_ImpulseTimer.gameObject.SetActive(false);
            UI_ImpulseTimer.text = null;
            UI_ImpulseFill.fillAmount = 0.0f;
        }
        else
        {
            //Prints timer text
            if (ImpulseTimer > 1.0f)
            {
                UI_ImpulseTimer.text = Mathf.FloorToInt(ImpulseTimer).ToString();
            }
            else
            {
                UI_ImpulseTimer.text = (ImpulseTimer).ToString("F1", CultureInfo.InvariantCulture);
            }
            UI_ImpulseFill.fillAmount = ImpulseTimer / ImpulseCooldown;
        }
    }
    private void LimitSpeed()
    {
        if (PlayerRB.velocity.magnitude > MaxSpeed) PlayerRB.velocity = Vector2.ClampMagnitude(PlayerRB.velocity, MaxSpeed);
    }
    //Can use State Pattern to get rid of all these ifs
    private void CheckInputs()
    {
        RotationSide = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(RotateRight) || Input.GetKey(RotateLeft)) CanRotate = true;
        if (Input.GetKey(MoveUp)) CanBoost = true;
        if (Input.GetKeyDown(ImpulseRight)) { IsImpulseRight = true; ImpulseBody(); }
        if (Input.GetKeyDown(ImpulseLeft)) { IsImpulseLeft = true; ImpulseBody(); }
    }
    private void AutoFire()
    {
        CanFire = true;
    }
    private void ShootProjectile()
    {
        if (CanFire)
        {
            FireTimer += Time.deltaTime;
            if (FireTimer > FireProjectileCooldown)
            {
                FireTimer = 0f;
                ProjectileBehaviour projectile = Instantiate(ProjectilePrefab, AutoFireSpawnPoint.position, transform.rotation);
                projectile.SetDamageAmount(ProjectileDamage);
            }
        }
    }
    public void TakeDamageOrHeal(int damage)
    {
        //Some logic if there is more to the damage
        PlayerHealth.TakeDamage(damage);
        UpdateUI();
        CheckGameStatus();
    }
    public void PickingCrystal()
    {
        PlayerCrystals++;
        UpdateUI();
        CheckGameStatus();
    }
    private void CheckGameStatus()
    {
        if (PlayerHealth.Amount <= 0)
        {
            CallGameLost();
        }
        else if (PlayerCrystals >= ReferenceGameManager.AmountOfCrystals)
        {
            CallGameVictory();
        }
    }
    private void CallGameLost()
    {
        StartCoroutine(StartGameOverSequence());
    }
    private void CallGameVictory()
    {
        OnVictory?.Invoke(true);
    }
    private IEnumerator StartGameOverSequence()
    {
        PlayerAnim.SetTrigger("IsDead");
        PlayerCollider.enabled = false;
        this.enabled = false;
        yield return AudioManager.Instance.WaitForEndOfSong(Explosion);
        OnGameOver?.Invoke(true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            TakeDamageOrHeal(ContactDamage);
            enemy.TakeDamageOrHeal(ContactDamage);
        }
        else
        {
            //Its a wall, do something?
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<IPickable>(out IPickable pickable))
        {
            pickable.PickUpOnContact();
        }
    }
}
