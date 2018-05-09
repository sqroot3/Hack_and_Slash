using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

    /*
     * For AI: whenever deciding what to go to,
     * evaluate distance to closest tree 
     * 
     *
     */

    private NavMeshAgent agent;
    [SerializeField] private GameObject player;
    [SerializeField] private bool isKinematic;
    [SerializeField] private Transform[] stations;
    [SerializeField] private float dirtMultiplier;
    [SerializeField] private float snowMultiplier;
    private int currentStation = 0;
    private PlayerMovement playerMovement;
    private EnemyAttack attack;

    /*
     *  Anim-specific imports 
     * 
     */


    public float defaultSpeed;
    public float sprintBoost;
    public float deathY = -1f;

    private Rigidbody rigidbody;
    private float moveVertical;
    private float moveHorizontal;
    private float vertical;
    private float horizontal;

    [HideInInspector] public Animator animator;
    private readonly int hashHorizontal = Animator.StringToHash("horizontal");
    private readonly int hashVertical = Animator.StringToHash("vertical");
    private readonly int hashIdle = Animator.StringToHash("idle");
    private readonly int hashSprint = Animator.StringToHash("sprint");

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    private void Start()
    {
        animator = GetComponent<Animator>();
        attack = GetComponent<EnemyAttack>();
        playerMovement = player.GetComponent<PlayerMovement>();
        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.speed = defaultSpeed;
        agent.updatePosition = false;
    }

    private void Update()
    {
        //Check if on death height
        if (transform.position.y <= deathY && !Manager.playerDied)
        {
            EnemyHealth health = GetComponent<EnemyHealth>();
            health.OnDeath(3);
            Debug.Log("Died from a fall");
        }

        //BOTH IF: enemy is facing player && the player is in "detection range"
        if (!IsPlayerBehind() && IsPlayerInRange())
        {
            agent.angularSpeed = 120;
            //player is now detected, AI should now get closer to him
            attack.State = 1;
            agent.SetDestination(player.transform.position);
            //onAttack now evaluates type of attacked based on detection/attack state
            attack.OnAttack();

        }
        //IF no player in sight to go and there's at least one tree burning
        else if (Manager.numOfBurningTrees > 0)
        {
            agent.angularSpeed = 120;
            //go to closest burning tree
            Tree t = getClosestBurningTree();
            agent.SetDestination(t.transform.position);
            attack.State = 4;

        }
        //ELSE, default to your calm/patrol state depending on enemy type
        else
        {
            if (isKinematic)
            {
                //Debug.Log(agent.destination);
                //if kinematic, go to next station and set patrolling state
                if (!agent.pathPending && agent.remainingDistance < 1f)
                    GoToNextStation();
                attack.State = 3;
            }
            else
            {
                //if static, stay in place
                agent.SetDestination(transform.position);
                agent.angularSpeed = 0;
                attack.State = 0;
            }
        }

        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;


        //Send data to animation
        bool isSprint = false;
        //bool isSprint = (attack.State == 1);
        bool isIdle = !(velocity.magnitude > 0.1f && agent.remainingDistance > agent.radius);

        //Send to animator
        animator.SetFloat(hashHorizontal, velocity.x);
        animator.SetFloat(hashVertical, velocity.y);
        animator.SetBool(hashIdle, isIdle);
        animator.SetBool(hashSprint, isSprint);


        //
        //Debug.Log(velocity + " - " + velocity.magnitude);
        Debug.DrawRay(transform.position, transform.right, Color.red);
        Debug.DrawRay(transform.position, transform.up, Color.green);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
    }

    void OnAnimatorMove()
    {
        //Update position to agent position
        transform.position = agent.nextPosition;
    }

    void GoToNextStation()
    {
        if (stations.Length == 0) return;

        agent.destination = stations[currentStation].position;
        currentStation = (currentStation + 1) % stations.Length;

    }

    public bool IsPlayerBehind()
    {
        //If DP b/w enemy's lookat and the vector from enemy to player is negative ( < -0.5f)
        //Then player is behind enemy
        Vector3 vecLookat = transform.forward;
        Vector3 vecEP = player.transform.position - transform.position;

        return (Vector3.Dot(vecEP, vecLookat) < -0.5f);
    }

    public bool IsPlayerInRange()
    {
        return (Mathf.Abs(Vector3.Distance(transform.position, playerMovement.transform.position)) <= attack.DetectRadius);
    }

    //determine closest tree
    public Tree getClosestTree()
    {
        float smallestDistance = 999999f;
        Tree closest = null;
        foreach (Tree t in Manager.trees)
        {
            if (t.distance(transform.position) < smallestDistance)
            {
                closest = t;
                smallestDistance = t.distance(transform.position);
            }
        }
        return closest;
    }

    //determine closest burning tree
    public Tree getClosestBurningTree()
    {
        float smallestDistance = 999999f;
        Tree closest = null;
        foreach (Tree t in Manager.trees)
        {
            if (t.distance(transform.position) < smallestDistance && t.OnFire)
            {
                closest = t;
                smallestDistance = t.distance(transform.position);
            }
        }
        return closest;
    }


    private void OnTriggerEnter(Collider other)
    {
        //handle entering in snow/dirt area triggers
        if (other.tag == "Snow_Ground")
        {
            //enemies will be faster on snow
            agent.speed += defaultSpeed * snowMultiplier;
        }
        else if (other.tag == "Dirt_Ground")
        {
            //enemies will be slower on dirt
            agent.speed -= defaultSpeed * dirtMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // as soon as area is left, reset speed to normal speed
        if (other.tag == "Snow_Ground" || other.tag == "Dirt_Ground")
            agent.speed = defaultSpeed;
    }


    /*
    [SerializeField] private GameObject player;
    [SerializeField] private bool isKinematic;
    [SerializeField] private Transform[] stations;
    [SerializeField] private float speed;
    [SerializeField] private float dirtMultiplier;
    [SerializeField] private float snowMultiplier;
    private int currentStation = 0;
    private PlayerMovement playerMovement;
    private EnemyAttack attack;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        attack = GetComponent<EnemyAttack>();
        playerMovement = player.GetComponent<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update () {

        //BOTH IF: enemy is facing player && the player is in "detection range"
        if (!IsPlayerBehind() && IsPlayerInRange())
        {
            //player is now detected, AI should now get closer to him
            attack.State = 1;
            agent.SetDestination(player.transform.position);
            if(attack.IsInAttackRadius())
            {
                //attack only if in attack radius
                attack.OnAttack();
            }
            
        }
        //IF no player in sight to go and there's at least one tree burning
        else if(Manager.numOfBurningTrees > 0)
        {
            //go to closest burning tree
            Tree t = getClosestBurningTree();
            agent.SetDestination(t.transform.position);
            attack.State = 4;

        }
        //ELSE, default to your calm/patrol state depending on enemy type
        else
        {
            if(isKinematic)
            {
                //if kinematic, go to next station and set patrolling state
                if(!agent.pathPending && agent.remainingDistance < 0.5f)
                    GoToNextStation();
                attack.State = 3;
            }
            else
            {
                //if static, stay in place
                agent.SetDestination(transform.position);
                attack.State = 0;
            }
            
            
        }
    }

    void GoToNextStation()
    {
        if (stations.Length == 0) return;

        agent.destination = stations[currentStation].position;
        currentStation = (currentStation + 1) % stations.Length;

    }

    public bool IsPlayerBehind()
    {
        //If DP b/w enemy's lookat and the vector from enemy to player is negative ( < -0.5f)
        //Then player is behind enemy
        Vector3 vecLookat = transform.forward;
        Vector3 vecEP = player.transform.position - transform.position;

        return (Vector3.Dot(vecEP, vecLookat) < -0.5f);
    }

    public bool IsPlayerInRange()
    {
        return (Mathf.Abs(Vector3.Distance(transform.position, playerMovement.transform.position)) <= attack.DetectRadius);
    }

    //determine closest tree
    public Tree getClosestTree()
    {
        float smallestDistance = 999999f;
        Tree closest = null;
        foreach(Tree t in Manager.trees)
        {
            if(t.distance(transform.position) < smallestDistance)
            {
                closest = t;
                smallestDistance = t.distance(transform.position);
            }
        }
        return closest;
    }

    //determine closest burning tree
    public Tree getClosestBurningTree()
    {
        float smallestDistance = 999999f;
        Tree closest = null;
        foreach (Tree t in Manager.trees)
        {
            if (t.distance(transform.position) < smallestDistance && t.OnFire)
            {
                closest = t;
                smallestDistance = t.distance(transform.position);
            }
        }
        return closest;
    }

    //demo - get color for closest tree ray
    public Color getTreeRayColor(int a)
    {
        switch(a)
        {
            case 0:     return Color.black;
            case 1:     return Color.white;
            case 2:     return Color.red;
            case 3:     return Color.green;
            default:    return Color.white;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //handle entering in snow/dirt area triggers
        if(other.tag == "Snow_Ground")
        {
            //enemies will be faster on snow
            agent.speed += speed * snowMultiplier;
        }
        else if(other.tag == "Dirt_Ground")
        {
            //enemies will be slower on dirt
            agent.speed -= speed * dirtMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // as soon as area is left, reset speed to normal speed
        if (other.tag == "Snow_Ground" || other.tag == "Dirt_Ground")
            agent.speed = speed;
    }
    */

}
