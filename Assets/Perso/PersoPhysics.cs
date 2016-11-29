using UnityEngine;
using System.Collections;

// Known bug : Jackhammer : if no time spent on ground (ie jump as soon as on ground), min jump ensues
// Can't figure it out, put hack in place

[RequireComponent(typeof(CharacterController))]
public class PersoPhysics : MonoBehaviour
{
	private Log log = new Log ("P-PHYSICS"/*, LogLevel.Debug/**/);


	public static PersoPhysics i;
	public PersoInput pinput, pinputPrec;

	public PersoListener pl;

	public float runAccel, runMaxSpeed, runBrake, runAirAccel, runAirBrake;
	public float jumpSpeed, jumpMinTime, jumpMaxTime;
	public float fallGravity, fallMax;
	public int dashsPerJump;
	public float dashSpeed, dashMinTime, dashMaxTime;

	[HideInInspector]
	public CharacterController charC;

	[HideInInspector]
	public Vector2 velocity = Vector2.zero;

	[HideInInspector]
	public bool faceRight = true, grounded = true, groundedOld = true;

	private bool jumpInHeld = false;
	private float jumpTimeSince = 0.0f;

	[HideInInspector]
	public bool dashInHeld = false, dashJump = false;
	[HideInInspector]
	public int dashsRemaining = 0;

	private float dashTimeSince = 0.0f;

	private float systGroundCheckDist = 0.14f;
	private bool hackJackhammer = false;

	public void Awake()
	{
		charC = GetComponent<CharacterController> ();
		pinput = new PersoInput ();
		pinputPrec = new PersoInput ();
		i = this;
	}

	public void Update()
	{
		pinputPrec = pinput;
		pinput = new PersoInput (pinputPrec, Input.GetAxis("Horizontal"), Input.GetButton("Jump"), Input.GetButton("Dash"));
	}

	public void FixedUpdate()
	{
		Vector2 accel = Vector2.zero;

		accel += FURun (accel);
		accel += FUDash (accel);
		accel += FUAir (accel);


		velocity += accel * Time.fixedDeltaTime;
		if (!dashInHeld && velocity.x != 0.0f)
			faceRight = velocity.x > 0.0f;

		charC.Move (new Vector3(velocity.x, velocity.y, 0.0f)*Time.fixedDeltaTime);

		groundedOld = grounded;
		grounded = Physics.Raycast (transform.position, Vector3.down, systGroundCheckDist)
			|| Physics.Raycast (transform.position + Vector3.right*0.2f, Vector3.down, systGroundCheckDist)
			|| Physics.Raycast (transform.position + Vector3.left*0.2f, Vector3.down, systGroundCheckDist);//charC.isGrounded;

		if (!groundedOld && grounded)
			Landing ();

		log.Debug ("Velocity : " + velocity + " | Accel : " + accel + " | Grounded : "+ grounded);
	}

	public Vector2 FURun(Vector2 curAccel)
	{
		Vector2 accel = Vector2.zero;
		float runTarget = pinput.inputX * runMaxSpeed;

		float rA = (grounded ? runAccel : runAirAccel), rB = (grounded ? runBrake : runAirBrake);

		if (dashJump)
			runTarget *= dashSpeed / runMaxSpeed;
			//runTarget = (faceRight ? 1f : -1f) * dashSpeed;

		float runForce = rA;
		if (Mathf.Abs (runTarget) > Mathf.Abs (velocity.x))
			runForce = rB;
		else if (Mathf.Sign (runTarget) != Mathf.Sign (velocity.x))
			runForce = rB;
		float runTargetDiff = runTarget - velocity.x;
		if (Mathf.Abs (runTargetDiff) > runForce * Time.fixedDeltaTime)
			accel += Vector2.right * Mathf.Sign (runTargetDiff) * runForce;
		else
			accel += Vector2.right * runTargetDiff / Time.fixedDeltaTime;
		return accel;
	}

	public Vector2 FUDash(Vector2 curAccel)
	{
		Vector2 accel = Vector2.zero;

		if (dashInHeld) {
			accel -= velocity / Time.fixedDeltaTime;
			accel -= curAccel;
			accel += (faceRight ? Vector2.right : Vector2.left) * dashSpeed/Time.fixedDeltaTime;

			dashTimeSince += Time.fixedDeltaTime;
			if (dashTimeSince > dashMaxTime || (dashTimeSince > dashMinTime && !pinput.inputDash))
				DashEnd ();
		} else {
			if (pinput.DashPressed() && dashsRemaining > 0)
				DashStart ();
				
			if (grounded)
				dashsRemaining = dashsPerJump;
		}

		return accel;
	}

	public Vector2 FUAir(Vector2 curAccel)
	{
		Vector2 accel = Vector2.zero;
		float downMaxSpeed = fallMax;

		if (grounded) {
			jumpInHeld = false;
			jumpTimeSince = 0.0f;
			if (pinput.JumpPressed() && hackJackhammer) {
				// JUMP START
				accel += JumpStart();
			} else {
				// Cancel downwards velocity
				downMaxSpeed = 0.0f;
			}
			hackJackhammer = true;
		} else {
			jumpTimeSince += Time.fixedDeltaTime;
			hackJackhammer = false;
			bool oldJIH = jumpInHeld;
			// Allows for variable jump height
			// In order -> Did we already release the button ? (no repushing midair)
			// Did we achieve max jump height/time ?
			// Did we achieve min jump height/time ? (if so stay pressed)
			// Then if we are in control, do we still want to jump ?
			jumpInHeld = jumpInHeld && (jumpTimeSince <= jumpMaxTime) && ((jumpTimeSince <= jumpMinTime) || (pinput.inputJump));
			log.Debug ("JIH " + jumpInHeld+ " | " + jumpTimeSince);
			if (oldJIH && !jumpInHeld)
				accel += JumpEnd ();

			if (!jumpInHeld && !dashInHeld)
				accel += Vector2.down * fallGravity;
		}

		if (velocity.y < -downMaxSpeed)
			accel += Vector2.down * (velocity.y+downMaxSpeed) / Time.fixedDeltaTime;
		
		return accel;
	}

	public Vector2 JumpStart()
	{
		pl.OnJumpStart ();
		log.Exec ("Jump Start !");
		//grounded = false;
		jumpInHeld = true;
		jumpTimeSince = 0.0f;
		pinput.JumpRelease ();

		if (dashInHeld)
		{
			DashEnd ();
			dashJump = true;
		}

		return Vector2.up * jumpSpeed / Time.fixedDeltaTime;
	}

	public Vector2 JumpEnd()
	{
		pl.OnJumpEnd ();
		// Allows for faster response and descent
		return Vector2.down * jumpSpeed / Time.fixedDeltaTime;
	}

	public void Landing()
	{
		dashJump = false;
		pl.OnLanding ();
	}

	public void DashStart()
	{
		dashInHeld = true;

		if (Mathf.Abs(pinput.inputX) > 0.0f)
			faceRight = pinput.inputX > 0.0f;

		log.Exec ("Dash Start ! (Dir = " + (faceRight ? "Right" : "Left") + " ; Pos = "+transform.position+")");

		dashTimeSince = 0.0f;

		dashsRemaining--;
		pinput.DashRelease ();

		pl.OnDashStart ();
	}

	public void DashEnd()
	{
		dashInHeld = false;
		dashJump = false;
		pl.OnDashEnd ();
	}
}

public class PersoInput
{
	public float inputX = 0.0f;
	public bool inputJump = false;
	public bool inputDash = false;
	public float inputJumpTime = 0.0f;
	public float inputDashTime = 0.0f;

	public PersoInput()
	{}

	public PersoInput(PersoInput o, float x, bool j, bool d)
	{
		inputX = (Mathf.Abs(x) >= GameOptions.stickDeadzone ? x : 0.0f);
		inputJump = j;
		inputDash = d;

		if (inputJump)
			inputJumpTime = o.inputJumpTime + Time.deltaTime;

		if (inputDash)
			inputDashTime = o.inputDashTime + Time.deltaTime;
	}

	public bool JumpPressed()
	{
		return inputJump && inputJumpTime <= GameOptions.buttonPressBuffer;
	}

	public bool DashPressed()
	{
		return inputDash && inputDashTime <= GameOptions.buttonPressBuffer;
	}

	public void JumpRelease()
	{
		inputJumpTime += GameOptions.buttonPressBuffer;
	}

	public void DashRelease()
	{
		inputDashTime += GameOptions.buttonPressBuffer;
	}
}