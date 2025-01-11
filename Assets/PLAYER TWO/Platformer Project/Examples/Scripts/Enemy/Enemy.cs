using UnityEngine;

public class Enemy : Entity<Enemy>
{
    public EnemyEvents enemyEvents;
    protected Collider[] m_sightOverlaps = new Collider[1024];
    protected Collider[] m_contackAttackOverlaps = new Collider[1024];
    public Player player {get; set;}
    public Health health {get; set;}
    public EnemyStatsManager stats {get; set;}
    public WaypointManager waypoints {get; set;}
    protected virtual void InitializeWaypointsManager() => waypoints = GetComponent<WaypointManager>();
    protected virtual void InitializeStatsManager() => stats = GetComponent<EnemyStatsManager>();
    protected virtual void InitializeHealth() => health = GetComponent<Health>(); 
    protected virtual void InitializeTag() => tag = GameTags.Enemy;



    public virtual void Accelerate(Vector3 direction, float acceleration, float topSpeed) =>
        Accelerate(direction, stats.current.turningDrag, acceleration, topSpeed);

    public virtual void Decelerate() => Decelerate(stats.current.deceleration);
    public virtual void Friction() => Decelerate(stats.current.friction);
    public virtual void Gravity() => Gravity(stats.current.gravity);
    public virtual void SnapToGround() => SnapToGround(stats.current.snapForce);

    public virtual void FaceDirectionSmooth(Vector3 direction) =>
        FaceDirection(direction, stats.current.rotationSpeed);
    protected virtual void HandleSight()
    {
        if (!player)
        {
            var overlaps = Physics.OverlapSphereNonAlloc(position, stats.current.spotRange, m_sightOverlaps);
            for (int i = 0; i < overlaps; ++i)
            {
                if (m_sightOverlaps[i].CompareTag(GameTags.Player))
                {
                    if (m_sightOverlaps[i].TryGetComponent<Player>(out var player))
                    {
                        this.player = player;
                        enemyEvents.OnPlayerSpotted?.Invoke();
                        return;
                    }
                }
            }
        }
        else
        {
            var distance = Vector3.Distance(position, player.position);
            if ((player.health.current == 0)||(distance > stats.current.viewRange))
            {
                player = null;
                enemyEvents.OnPlayerScaped?.Invoke();
            }
        }
    }

    protected virtual void ContactAttack()
    {
        if (stats.current.canAttackOnContact)
        {
            var overlaps = OverlapEntity(m_contackAttackOverlaps, stats.current.contactOffset);

            for (int i = 0; i < overlaps; ++i)
            {
                if (m_contackAttackOverlaps[i].CompareTag(GameTags.Player) &&
                    m_contackAttackOverlaps[i].TryGetComponent<Player>(out var player))
                {
                    var stepping = controller.bounds.max + Vector3.down * stats.current.contactSteppingTolerance;

                    if (!player.IsPointUnderStep(stepping))
                    {
                        if (stats.current.contactPushback)
                        {
                            lateralVelocity = -transform.forward * stats.current.contactPushBackForce;
                        }

                        player.ApplyDamage(stats.current.contactDamage, transform.position);
                        enemyEvents.OnPlayerContact?.Invoke();
                    }
                } 
            }
        }
    }


    public override void ApplyDamage(int amount, Vector3 origin)
    {
        if (!health.isEmpty && !health.recovering)
        {
            health.Damage(amount);
            enemyEvents.OnDamage?.Invoke();

            if (health.isEmpty)
            {
                controller.enabled = false;
                enemyEvents.OnDie?.Invoke();
            }
        }
    }


    protected override void Awake()
    {
        base.Awake();
        InitializeTag();
        InitializeStatsManager();
        InitializeWaypointsManager();
        InitializeHealth();
    } 

    protected override void OnUpdate()
    {
        HandleSight();
        ContactAttack();

    }
}