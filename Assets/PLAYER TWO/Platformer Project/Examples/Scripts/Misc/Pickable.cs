using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Pickable : MonoBehaviour, IEntityContact
{
    [Header("General Settings")]
    public Vector3 offset;
    public float releaseOffset = 0.5f;

    [Header("Attack Settings")] 
    public bool attackEnemies = true;
    public int damage = 1;
    public float minDamageSpeed = 5f;

    [Header("Respawn Settings")] 
    public bool autoReswapn;
    public bool respawnOnHitHazards;
    public float respawnHeightLimit = -100;

    public UnityEvent onPicked;
    public UnityEvent onReleased;
    public UnityEvent onRespawn;

    protected Collider m_collider;
    protected AudioSource m_audioSource;
    protected Rigidbody m_rigidBody;

    protected Vector3 m_initialPosition;
    protected Quaternion m_initialRotation;
    protected Transform m_initialParent;

    protected RigidbodyInterpolation m_interpolation;
    public bool beingHold {get; private set; }

    protected void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        m_initialPosition = transform.localPosition;
        m_initialRotation = transform.localRotation;
        m_initialParent = transform.parent;

    }
    public virtual void PickUp(Transform slot)
    {
        if (!beingHold)
        {
            beingHold = true;
            transform.parent = slot;
            transform.localPosition = Vector3.zero + offset;
            transform.localRotation = Quaternion.identity;
            m_rigidBody.isKinematic = true;
            m_collider.isTrigger = true;
            m_interpolation = m_rigidBody.interpolation;
            m_rigidBody.interpolation = RigidbodyInterpolation.None;
            onPicked?.Invoke();
        }
    }

    public virtual void Release(Vector3 direction, float force)
    {
        if (beingHold)
        {
            transform.parent = null;
            transform.position += direction * releaseOffset;
            m_collider.isTrigger = m_rigidBody.isKinematic = beingHold = false;
            m_rigidBody.interpolation = m_interpolation;
            m_rigidBody.velocity = direction * force;
            onReleased?.Invoke();
        }
    }
    public void OnEntityContact(Entity entity)
    {
        if (attackEnemies && entity is Enemy && m_rigidBody.velocity.magnitude > minDamageSpeed)
        {
            entity.ApplyDamage(damage, transform.position);
        }
    }

    public virtual void Respawn()
    {
        m_rigidBody.velocity = Vector3.zero;
        transform.parent = m_initialParent;
        transform.SetPositionAndRotation(m_initialPosition, m_initialRotation);
        m_rigidBody.isKinematic = m_collider.isTrigger = beingHold = false;
    }

    protected virtual void EvaluateHazardRespawn(Collider other)
    {
        if (autoReswapn && respawnOnHitHazards && other.CompareTag(GameTags.Hazard))
        {
            Respawn();
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        EvaluateHazardRespawn(other);
    }

    protected void OnCollisionEnter(Collision other)
    {
        EvaluateHazardRespawn(other.collider);
    }
}