using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
	public int HP;
	public int StartingHP;
	public bool IsDead => HP <= 0;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void Damage(int amount)
	{
		HP -= amount;
	}
}
