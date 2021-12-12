using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
	[SerializeField]
	private float _speed = 0;


	private void FixedUpdate()
	{
		var renderer = GetComponent<Renderer>();
		renderer.material.mainTextureOffset = new Vector2(0f, Time.time * _speed);
	}
}
