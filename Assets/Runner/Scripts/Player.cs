using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public AudioClip JumpS;
	public AudioClip HeartS;
	public AudioClip KeyS;
	public AudioClip OpenS;
	public AudioClip FallS;
	public AudioClip CoinS;
	public AudioClip CompleteS;
	AudioSource audioSource;
	const float jumpCheckPreventionTime = 0.5f;
	public delegate void CollectCoinCallback();
	public CollectCoinCallback onCollectCoin;
	public GameObject player;
	[Header("Physic Setting")]
	public LayerMask groundLayerMask;
	[Header("Move & Jump Setting")]
	public float moveSpeed = 10;
	public float fallWeight = 5.0f;
	public float jumpWeight = 0.5f;
	public float jumpVelocity = 100.0f;
	protected bool jumping = false;			
	protected Vector3 moveVec = Vector3.zero; 
	protected float jumpTimestamp;		
	protected Animator animator;				
	protected Rigidbody rigidbody;         
	public GameObject KeyImage;
	bool IsPlayerHasKey;
	[SerializeField] GameController gamecontroller;
	private void Awake()
	{
		this.audioSource = GetComponent<AudioSource>();
		IsPlayerHasKey = false;
		animator = GetComponentInChildren<Animator>();
		rigidbody = GetComponent<Rigidbody>();
		
	}

	void PlaySound(string action)
	{
		switch (action)
		{
			case "JUMP":
				audioSource.clip = JumpS;
				break;
			case "COIN":
				audioSource.clip = CoinS;
				break;
			case "HEART":
				audioSource.clip = HeartS;
				break;
			case "KEY":
				audioSource.clip = KeyS;
				break;
			case "OPEN":
				audioSource.clip = OpenS;
				break;
			case "FALL":
				audioSource.clip = FallS;
				break;
			case "COMPLETE":
				audioSource.clip = CompleteS;
				break;
		}
		audioSource.Play();
	}

	void UpdateWhenJumping()
	{
		bool isFalling = rigidbody.velocity.y <= 0;
		float weight = isFalling ? fallWeight : jumpWeight;
		rigidbody.velocity = new Vector3(moveVec.x * moveSpeed, rigidbody.velocity.y, moveVec.z * moveSpeed);
		rigidbody.velocity += Vector3.up * Physics.gravity.y * weight * Time.deltaTime;
		GroundCheck();
	}

	void UpdateWhenGrounded()
	{
		rigidbody.velocity = moveVec * moveSpeed;
		if (moveVec != Vector3.zero)
		{
			transform.LookAt(this.transform.position + moveVec.normalized);
		}
		CheckShouldFall();
	}

	private void FixedUpdate()
	{
		if (jumping == false)
		{
			UpdateWhenGrounded();
		}
		else
		{
			UpdateWhenJumping();
		}
	}

	void Update()
	{
		UpdateAnimation();
	}

	public void OnJump()
    {
		HandleJump();
    }

	public void OnMove(InputValue input)
    {
		Vector2 inputVec = input.Get<Vector2>();

		moveVec = new Vector3(inputVec.x, 0, inputVec.y);
    }

	protected bool HandleJump()
	{
		if (jumping)
		{
			return false;
		}
		PlaySound("JUMP");
		jumping = true;
		jumpTimestamp = Time.time;
		rigidbody.velocity = new Vector3(0, jumpVelocity, 0);

		return true;
	}

	void CheckShouldFall()
	{
		if(jumping)
		{
			return;	
		}

		bool hasHit = Physics.CheckSphere(transform.position, 0.1f, groundLayerMask);

		if (hasHit == false)
		{
			jumping = true;
		}
	}
	void GroundCheck()
	{
		if(jumping == false)
		{
			return;
		}

		if (Time.time < jumpTimestamp + jumpCheckPreventionTime)
		{
			return;
		}

		bool hasHit = Physics.CheckSphere(transform.position, 0.1f, groundLayerMask);
		
		if(hasHit)
		{
			jumping = false;
		}
	}
	void UpdateAnimation()
	{
		if (animator == null)
		{
			return;
		}

		animator.SetBool("jumping", jumping);
		animator.SetFloat("moveSpeed", moveVec.magnitude);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Coin")
		{
			HandleCoinCollect(other);
			PlaySound("COIN");
		}
		if(IsPlayerHasKey == false)
        {
			if (other.transform.tag == "Key")
			{
				PlaySound("KEY");
				other.gameObject.SetActive(false);
				KeyImage.SetActive(true);
				IsPlayerHasKey = true;
			}
		}
		
		if (other.transform.tag == "TimeBoost")
		{
			PlaySound("HEART");
			other.gameObject.SetActive(false);
			gamecontroller.time += 10;
		}
		if (other.transform.tag == "Goal")
		{
			PlaySound("COMPLETE");
			gamecontroller.GameResult();
		}
		if (other.transform.tag == "DeathPlane")
		{
			PlaySound("FALL");
			player.transform.position = new Vector3(-14, 11, -21);
		}
	}
	private void OnCollisionEnter(Collision other)
	{
		if(IsPlayerHasKey == true)
        {
			if (other.transform.tag == "Obstacle")
			{
				PlaySound("OPEN");
				Debug.Log("Door Opened");
				other.gameObject.SetActive(false);
				KeyImage.SetActive(false);
				IsPlayerHasKey = false;
			}
		}
		
	}
	void HandleCoinCollect(Collider collision)
	{
		Coin coin = collision.transform.GetComponent<Coin>();
		if(coin == null)
		{
			return;
		}
		coin.Collect();

		if(onCollectCoin != null)
		{
			onCollectCoin();
		}
	}
}
