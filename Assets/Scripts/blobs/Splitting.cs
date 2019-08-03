using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Life))]
public class Splitting : MonoBehaviour
{

	private Life lifePool;
	private double scaleFactor;

	private UnitController UController;

	private int lastFrameHP = 0;

	// Start is called before the first frame update
	void Awake()
	{
		lifePool = GetComponent<Life>();
		ScaleVolumeAndMass();
		UController = UnitController.Instance;
	}

	// Update is called once per frame
	void Update()
	{
		if (lastFrameHP != lifePool.HP)
		{
			ScaleVolumeAndMass();
			lastFrameHP = lifePool.HP;
		}
	}

	// Split the attached GameObject into two. Life is split evenly,
	// everything else is copied.
	public void Split()
	{
		if (lifePool.HP <= 1)
			return;

		Vector3 offset = VectorUtils.RandomHorizontalUnitVector() * (float)scaleFactor / 2;

		GameObject child1 = Instantiate(gameObject);
		child1.GetComponent<Life>().HP = lifePool.HP / 2;
		child1.GetComponent<Transform>().position += offset;

		GameObject child2 = Instantiate(gameObject);
		child2.GetComponent<Life>().HP = (lifePool.HP + 1) / 2;
		child2.GetComponent<Transform>().position -= offset;

		Unit unit = GetComponent<Unit>();
		UController.RemoveUnitFromSelection(unit);
		UController.RemoveUnit(unit);

		var target = unit.GetComponent<MoveableUnit>()?.Target;

		unit = child1.GetComponent<Unit>();
		UController.AddUnitToSelection(unit);
		if (target != null)
		{
			target.AddTargetingUnit(unit);
		}


		unit = child2.GetComponent<Unit>();
		UController.AddUnitToSelection(unit);
		if (target != null)
		{
			target.AddTargetingUnit(unit);
		}


		Destroy(gameObject);
	}

	// Combine the attached GameObject with another one. Life and momenta are combined.
	public static void Combine(IEnumerable<Splitting> blobs)
	{
		if (blobs.Count() <= 1)
			return;
		// Actually just "eats" the other object


		GameObject combined = Instantiate(blobs.First().gameObject);

		// Combine life totals
		Life combinedLife = combined.GetComponent<Life>();
		combinedLife.HP = blobs.Sum(x => x.lifePool.HP);
		//combined.GetComponent<Splitting>().Start();

		// Combine momenta
		IEnumerable<Rigidbody> bodies = blobs.Select(x => x.GetComponent<Rigidbody>()).Where(x => x != null);

		IEnumerable<Vector3> forces = bodies.Select(x => x.mass * x.velocity);
		Vector3 totalMomentum = Vector3.zero;
		foreach (var force in forces)
		{
			totalMomentum += force;
		}
		Rigidbody combinedBody = combined.GetComponent<Rigidbody>();
		combinedBody.velocity = totalMomentum / combinedBody.mass;

		UnitController UController = UnitController.Instance;

		Unit combinedUnit = combined.GetComponent<Unit>();


		var target = blobs.Select(x => x.GetComponent<MoveableUnit>().Target).FirstOrDefault();
		if (target != null)
		{
			combinedUnit.GetComponent<MoveableUnit>().SetNewDestination(target);
		}

		// clean up
		foreach (var blob in blobs.ToList())
		{
			var unit = blob.GetComponent<Unit>();
			unit.GetComponent<MoveableUnit>()?.Target?.RemoveTargetingUnit(unit);
			UController.RemoveUnitFromSelection(unit);
			UController.RemoveUnit(unit);
			Destroy(blob.gameObject);
		}

		UController.AddUnitToSelection(combinedUnit);

	}

	private void ScaleVolumeAndMass()
	{
		Transform transform = GetComponent<Transform>();
		double volFactor = (double)lifePool.HP / lifePool.StartingHP;
		scaleFactor = Math.Pow(volFactor, (double)1 / 3);
		transform.localScale = Vector3.one * (float)scaleFactor;

		Rigidbody body = GetComponent<Rigidbody>();
		if (body != null)
		{
			body.mass = (float)volFactor;
		}
	}
}
