using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public float runSpeed = 5f;
	public float fastRunSpeed = 10f;
	public float horizontalSpeed;
	public float jumpForce = 5f;
	public float flySpeed = 5f;
	public float fallingSpeed;
	public bool isStopControler;


	private Vector2 touchStartPosition;

	[Space(15)]
	public float motionCameraSpeed = 1f;
	public CinemachineRecomposer cameraRecomposer;
	private float originDistanceCamera;
	private float effectDistanceMaximumCamera = 1.4f;
	public ParticleSystem speedParticlesEffect;



	[Space(15)]
	public float GroundedOffset = -0.14f;
	public float GroundedRadius = 0.28f;
	public LayerMask GroundLayers;

	private Rigidbody rb;
	private Animator animator;
	private float speed;
	private float flySpeedStarted;
	private float jumpForceStarted;
	private float horizontalLimet = 3.5f;
	private bool isGrounded;
	private bool isFlying;
	private bool isOnSpeedUpPlatform;
	

	private GameState gameState;

	public float angleRotation;
	void Start()
	{
		
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		gameState = FindAnyObjectByType<GameState>();
		flySpeedStarted = flySpeed;
		jumpForceStarted = jumpForce;
		originDistanceCamera = cameraRecomposer.m_ZoomScale;
	}

	
	void FixedUpdate()
	{
		
		Movement();
		RunFast();
		GroundedCheck();
		SmoothFalling();
	}
	void Update()
	{
		Jump();
		
	}
	private void LateUpdate()
	{
		CameraEffectSpeed(); 
	}
	void Movement()
	{
		if (isStopControler)
			return;

		speed = isOnSpeedUpPlatform ? fastRunSpeed : runSpeed;

		float horizontal = Input.GetAxisRaw("Horizontal");
		Vector3 dirX = new Vector3(horizontal, 0f, 0f);
		Vector3 moveHorizontal = dirX * horizontalSpeed * Time.fixedDeltaTime;
		float horizontalClamp = Mathf.Clamp(transform.position.x, -horizontalLimet, horizontalLimet);

		Vector3 forwardMovement = transform.forward * speed * Time.fixedDeltaTime;

		rb.position = new Vector3(horizontalClamp, rb.position.y, rb.position.z);
		rb.MovePosition(rb.position + forwardMovement + moveHorizontal);
		
		
	}

	void RunFast()
	{
		
		if (isOnSpeedUpPlatform)
		{
			animator.SetBool("RunFast", true);
		}
		else
			animator.SetBool("RunFast", false);
	}

	void Jump()
	{
		if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || CheckJumpTouch()))
		{
			rb.AddRelativeForce(Vector3.up * jumpForce,ForceMode.Impulse);
			
			animator.SetTrigger("Jump");
		}
	}

	

	public void JumpFromPlatform( int _throwingJump ,float _jumpPower )
	{
		rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
		rb.AddRelativeForce(Vector3.forward * _throwingJump, ForceMode.Impulse);
		flySpeed = _throwingJump;
		Debug.Log("throwing");
	}

	void SmoothFalling()
	{
		if (isGrounded)
			return;
		if (rb.velocity.y < 0f)
		{
			rb.velocity += Vector3.down * fallingSpeed * Time.deltaTime;

		}
	}
	//void TouchController()
	//{
	//	// Handle touch input for movement
	//	if (Input.touchCount > 0)
	//	{
	//		Touch touch = Input.GetTouch(0);

	//		switch (touch.phase)
	//		{
	//			case TouchPhase.Began:
	//				touchStartPosition = touch.position;
	//				break;
	//			case TouchPhase.Moved:
	//			case TouchPhase.Stationary:
	//				float horizontalDelta = (touch.position.x - touchStartPosition.x) / Screen.width;
	//				float horizontal = Mathf.Clamp(horizontalDelta, -1f, 1f);
	//				Vector3 dirX = new Vector3(horizontal, 0f, 0f);
	//				Vector3 moveHorizontal = dirX * horizontalSpeed * Time.deltaTime;
	//				rb.MovePosition(rb.position + moveHorizontal);
	//				break;
	//		}
	//	}
	//}
	bool CheckJumpTouch()
	{
		// Check for touch input for jumping
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				// Implement logic to determine if touch is for jumping
				// For example, check if touch is in a specific UI button area or screen region
				// You can use RectTransforms or screen coordinates for this check
				Vector2 touchPosition = touch.position;
				Rect jumpButtonRect = new Rect(0, 0, Screen.width / 4, Screen.height / 4); // Example rect for jump button

				if (jumpButtonRect.Contains(touchPosition))
				{
					return true;
				}
			}
		}
		
		return false;
	}


	private void OnTriggerEnter(Collider other)
	{	
		if (other.CompareTag("coin"))
		{
			Destroy(other.gameObject);
			var coinEffect = gameState.CoinDestroyedEffect;

			Instantiate(coinEffect,other.transform.position,Quaternion.identity);
		}
	}
	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.CompareTag("speed up") && isGrounded)
			isOnSpeedUpPlatform = true;
	}
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.CompareTag("speed up"))
			isOnSpeedUpPlatform = false;
	}
	private void GroundedCheck()
	{
		Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
			transform.position.z);
		if (Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore))
		{
			isGrounded = true;
			isFlying = false;
		}
		else
		{
			isGrounded = false;
			isFlying = true;
		}
			

		animator.SetBool("IsGrounded", isGrounded);
	}

	// called to "flyingByPlatform" script (State Machine Behavior)
	public void ResetParameters()
	{
		flySpeed = flySpeedStarted;
		jumpForce = jumpForceStarted;
	}
	
	void CameraEffectSpeed()
	{

		float normalizedSpeed = Mathf.InverseLerp(runSpeed, fastRunSpeed, speed);
		var jumpFactor = Mathf.InverseLerp(0, jumpForce, rb.velocity.y);

		float targetZoom = Mathf.Lerp(originDistanceCamera, effectDistanceMaximumCamera, normalizedSpeed + jumpFactor);

		cameraRecomposer.m_ZoomScale = Mathf.Lerp(cameraRecomposer.m_ZoomScale, targetZoom, motionCameraSpeed * Time.deltaTime);

		SpeedEffectParticles();
	}

	void SpeedEffectParticles()
	{
		if (speed >= fastRunSpeed)
		{
			if (!speedParticlesEffect.isPlaying)
				speedParticlesEffect.Play();
		}
		else 
		if (speedParticlesEffect.isPlaying)
			speedParticlesEffect.Stop();

	}

	public void ActiveWinnerDance()
	{
		animator.SetBool("winnerDance",true);
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawSphere(
			new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
			GroundedRadius);
	}
}
