using UnityEngine;
using UnityEngine.AI;
namespace WarriorAnimsFREE
{
	public class WarriorMovementController:SuperStateMachine
	{
		[Header("Components")]
		private WarriorController warriorController;

		[Header("Movement")]
		public float movementAcceleration = 90.0f;
		public float runSpeed = 6f;
		private readonly float rotationSpeed = 40f;
		public float groundFriction = 50f;
		[HideInInspector] public Vector3 currentVelocity;

		[Header("Jumping")]
		public float gravity = 25.0f;
		public float jumpAcceleration = 5.0f;
		public float jumpHeight = 3.0f;
		public float inAirSpeed = 6f;

		[HideInInspector] public Vector3 lookDirection { get; private set; }

		//////////Add Data///////////
		private Camera mainCamera;
		private Vector3 targetPos;
		private bool isRun = false;
		private Rigidbody rigid;
		private NavMeshAgent agent;
		//////////Add Data///////////


		private void Start()
		{
			warriorController = GetComponent<WarriorController>();
			
			// Set currentState to idle on startup.
			currentState = WarriorState.Idle;

			rigid = GetComponent<Rigidbody>();
			mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
			agent = GetComponent<NavMeshAgent>();

			this.Hp = 400;
			this.Mp = 100;
			this.MoveSpeed = 6;
			this.AttackSpeed = 6;
			this.Range = 10;
			this.Ammor = 10;
		}
		private void Update()
		{
			if (Input.GetMouseButtonUp(0))
			{
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit, 10000f))
				{
					targetPos = hit.point;
					agent.SetDestination(targetPos);
				}
			}
			if (Run(targetPos))
			{
				isRun = true;
				Turn(targetPos);
			}
			else
			{
				isRun = false;
			}
		}
		private bool Run(Vector3 targetPos)
		{

			float dis = Vector3.Distance(transform.position, targetPos);
			if (dis >= 0.3f)
			{
				print("==");
				return true;
			}
			return false;
		}
		private void Turn(Vector3 targetPos)
		{
			Vector3 dir = targetPos - transform.position;
			Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);
			Quaternion targetRot = Quaternion.LookRotation(dirXZ);
			rigid.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 550.0f * Time.deltaTime);
		}
		#region Updates

		/*void Update () {
		 * Update is normally run once on every frame update. We won't be using it in this case, since the SuperCharacterController component sends a callback Update called SuperUpdate. 
		 * SuperUpdate is recieved by the SuperStateMachine, and then fires further callbacks depending on the state.
		}*/

		// Put any code in here you want to run BEFORE the state's update function. 
		// This is run regardless of what state you're in.
		protected override void EarlyGlobalSuperUpdate()
		{
		}

		// Put any code in here you want to run AFTER the state's update function.  
		// This is run regardless of what state you're in.
		protected override void LateGlobalSuperUpdate()
		{
			// Move the player by our velocity every frame.
			//transform.position += currentVelocity * warriorController.superCharacterController.deltaTime;

			// If alive and is moving, set animator.
			if (warriorController.canMove) {
				if (isRun) {
					warriorController.SetAnimatorBool("Moving", true);
					warriorController.SetAnimatorFloat("Velocity", agent.speed);
				} 
				else {
					warriorController.SetAnimatorBool("Moving", false);
					warriorController.SetAnimatorFloat("Velocity", 0);
				}
			}
		}

		#endregion

		#region Gravity / Jumping

		public void RotateGravity(Vector3 up)
		{
			lookDirection = Quaternion.FromToRotation(transform.up, up) * lookDirection;
		}

		// Calculate the initial velocity of a jump based off gravity and desired maximum height attained.
		private float CalculateJumpSpeed(float jumpHeight, float gravity)
		{
			return Mathf.Sqrt(2 * jumpHeight * gravity);
		}

		#endregion

		#region States

		// Below are the state functions. 
		// Each one is called based on the name of the state, so when currentState = Idle, we call Idle_EnterState. 
		// If currentState = Jump, we call Jump_SuperUpdate()
		private void Idle_EnterState()
		{
			warriorController.superCharacterController.EnableSlopeLimit();
			warriorController.superCharacterController.EnableClamping();
			warriorController.LockJump(false);
			warriorController.SetAnimatorInt("Jumping", 0);
			warriorController.SetAnimatorBool("Moving", false);
		}

		// Run every frame we are in the idle state.
		private void Idle_SuperUpdate()
		{
			// If Jump.
			if (warriorController.canJump && warriorController.inputJump) {
				currentState = WarriorState.Jump;
				return;
			}
			// In air.
			if (!warriorController.MaintainingGround()) {
				currentState = WarriorState.Fall;
				return;
			}
			if (warriorController.HasMoveInput() && warriorController.canMove) {
				currentState = WarriorState.Move;
				return;
			}
			// Apply friction to slow to a halt.
			currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, groundFriction 
				* warriorController.superCharacterController.deltaTime);
		}

		// Run once when exit the idle state.
		private void Idle_ExitState()
		{
		}

		// Run once when exit the idle state.
		private void Idle_MoveState()
		{
			warriorController.SetAnimatorBool("Moving", true);
		}

		private void Move_SuperUpdate()
		{
			// If Jump.
			if (warriorController.canJump && warriorController.inputJump) {
				currentState = WarriorState.Jump;
				return;
			}
			// Fallling.
			if (!warriorController.MaintainingGround()) {
				currentState = WarriorState.Fall;
				return;
			}
			// Set speed determined by movement type.
			if (warriorController.HasMoveInput() && warriorController.canMove) {
				currentVelocity = Vector3.MoveTowards(currentVelocity, warriorController.moveInput 
					* agent.speed, movementAcceleration 
					* warriorController.superCharacterController.deltaTime);
			} else {
				currentState = WarriorState.Idle;
			}
		}

		#endregion

	}
}