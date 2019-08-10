using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
	public int HP;
	public int StartingHP;
	public bool DestroyOnDead = true;
	public bool IsDead => HP <= 0;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (IsDead && DestroyOnDead)
		{
			Destroy(gameObject);
			Unit unit = GetComponent<Unit>();
			if(unit != null)
			{
				UnitController.Instance.RemoveUnit(unit);
				UnitController.Instance.RemoveUnitFromSelection(unit);
			}
		}

	}

	public void Damage(int amount)
	{
		HP -= amount;
	}
}
