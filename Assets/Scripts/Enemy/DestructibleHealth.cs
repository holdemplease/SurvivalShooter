using UnityEngine;

public class DestructibleHealth : MonoBehaviour	
{
	public int startingHealth = 100;
	public int currentHealth;
	public float sinkSpeed = 2.5f;
	public int scoreValue = 10;
	public AudioClip deathClip;
	public float thrust;
	public Rigidbody rigidBody;
	public Transform attackerTransform;
	public float explodeRadius = 5.0F;
	public float explodeForce = 10.0F;
	public int explosionDamage = 100;
	
	
	//	Animator anim;
	AudioSource enemyAudio;
	ParticleSystem hitParticles;
	CapsuleCollider capsuleCollider;
	bool isDead;
	bool isSinking;
	bool isTakeDamage;
	
	void Awake ()
	{
		//		anim = GetComponent <Animator> ();
		enemyAudio = GetComponent <AudioSource> ();
		hitParticles = GetComponentInChildren <ParticleSystem> ();
		capsuleCollider = GetComponent <CapsuleCollider> ();
		rigidBody = GetComponent<Rigidbody>();
		
		currentHealth = startingHealth;
	}
	
	
	void Update ()
	{
		if(isSinking)
		{
			//			transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		}
	}
	
	
	public void TakeDamage (int amount, Vector3 hitPoint)
	{
		if(isDead)
			return;
		
		enemyAudio.Play ();
		
		currentHealth -= amount;
		
		hitParticles.transform.position = hitPoint;
		hitParticles.Play();

		rigidBody.AddForce(attackerTransform.forward * thrust);
		rigidBody.AddForceAtPosition(attackerTransform.forward * thrust, hitPoint);

		Debug.Log("The direction is "+ attackerTransform.forward);

		if(currentHealth <= 0)
		{
			Explode ();
		}
	}
	
	
	void Explode ()
	{
		isDead = true;
		
		//		capsuleCollider.isTrigger = true;
		
		//		anim.SetTrigger ("Dead");

		enemyAudio.clip = deathClip;
		enemyAudio.Play ();

		inflictAreaDamage ();
//		transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
//		GetComponent <Rigidbody> ().isKinematic = true;
//		Destroy (gameObject, 2f);
//		Invoke ("RemoveObject", 1);
		// launch the can upward with the final explosion
		rigidBody.AddForce(Vector3.up * thrust*5);

		Destroy (gameObject, 2f);
	}
	
	public void inflictAreaDamage ()
	{
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere (explosionPos, explodeRadius);
		foreach (Collider hit in colliders)
		{
			if (hit && hit.GetComponent<Rigidbody>())
			{
				hit.attachedRigidbody.AddExplosionForce (explodeForce, explosionPos, explodeRadius);
				EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth> ();
				PlayerHealth playerHealth = hit.GetComponent<PlayerHealth> ();
				if (enemyHealth)
					enemyHealth.TakeDamage (explosionDamage, hit.attachedRigidbody.transform.position);
				else if (playerHealth)
					playerHealth.TakeDamage (explosionDamage);
			}
		}
	}
	
}
