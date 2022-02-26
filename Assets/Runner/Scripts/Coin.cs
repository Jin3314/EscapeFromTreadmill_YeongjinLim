using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 100;
    [SerializeField] GameObject effectParticle;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += rotateSpeed * new Vector3(0, 1, 0) * Time.deltaTime;
    }

	public void Collect()
	{
        SpawnVFXParticle();
        Destroy(gameObject);
	}

	void SpawnVFXParticle()
	{
		if(effectParticle == null)
		{
            return;
		}
        GameObject newParticle = Instantiate(effectParticle);
        Transform tf = newParticle.transform;
        tf.position = transform.position + new Vector3(0, 2, 0);
	}
}
