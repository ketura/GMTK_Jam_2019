using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Life))]
public class Splitting : MonoBehaviour
{
    
	private Life lifePool;
	public double scaleFactor = 2.0;
    public double SpeedScaleFactor = 1.5;

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

		var target = unit.GetComponent<MoveableUnit>()?.Waypoint;

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

	public static void ShuffleAndCombine(IEnumerable<Splitting> blobs, float distanceThreshold, int healthPerUnitLeeway)
	{
		Dictionary<Splitting, Splitting> pairs = new Dictionary<Splitting, Splitting>();
		Dictionary<Splitting, HashSet<Splitting>> rejects = new Dictionary<Splitting, HashSet<Splitting>>();

		var XBlobs = blobs.ToList();
		var ZBlobs = blobs.ToList();
		XBlobs.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
		ZBlobs.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));

		for(int i = 0; i < XBlobs.Count; i++)
		{
			var blob = XBlobs[i];
			if (pairs.ContainsKey(blob))
				continue;

			for(int j = i + 1; j < XBlobs.Count; j++)
			{
				var otherblob = XBlobs[j];

				if ((rejects.ContainsKey(blob) && rejects[blob].Contains(otherblob)) || (rejects.ContainsKey(otherblob) && rejects[otherblob].Contains(blob)))
					continue;

				float leeway = (float)(blob.lifePool.HP + otherblob.lifePool.HP) / healthPerUnitLeeway;

				if(Vector3.Distance(blob.transform.position, otherblob.transform.position) < distanceThreshold + leeway)
				{
					pairs[blob] = otherblob;
					pairs[otherblob] = blob;
					break;
				}
				else
				{
					if(!rejects.ContainsKey(blob))
					{
						rejects[blob] = new HashSet<Splitting>();
					}
					if(!rejects.ContainsKey(otherblob))
					{
						rejects[otherblob] = new HashSet<Splitting>();
					}
					rejects[blob].Add(otherblob);
					rejects[otherblob].Add(blob);
				}
			}
		}

		List<Splitting> completed = new List<Splitting>();

		bool played = false;

		foreach(var pair in pairs)
		{
			if (completed.Contains(pair.Key) || completed.Contains(pair.Value))
				continue;

			completed.Add(pair.Key);
			completed.Add(pair.Value);
			Combine(new List<Splitting>() { pair.Key, pair.Value });

			if(!played)
			{
				AudioManager.Instance.PlayClip(SplitController.Instance.MergeSound);
				played = true;
			}
			
		}
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


		var target = blobs.Select(x => x.GetComponent<MoveableUnit>().Waypoint).FirstOrDefault();
		if (target != null)
		{
			combinedUnit.GetComponent<MoveableUnit>().SetNewDestination(target);
		}

		// clean up
		foreach (var blob in blobs.ToList())
		{
			var unit = blob.GetComponent<Unit>();
			unit.GetComponent<MoveableUnit>()?.Waypoint?.RemoveTargetingUnit(unit);
			UController.RemoveUnitFromSelection(unit);
			UController.RemoveUnit(unit);
			Destroy(blob.gameObject);
		}

		UController.AddUnitToSelection(combinedUnit);

	}

	private void ScaleVolumeAndMass()
	{
		Transform transform = GetComponent<Transform>();
		double volFactor = (double)lifePool.HP * scaleFactor;
        double speedFactor = Mathf.Pow(lifePool.HP, (float)SpeedScaleFactor);
        //scaleFactor = Math.Pow(volFactor, (double)1 / 3);
        transform.localScale = Vector3.one * (float)Math.Pow(volFactor, (double)1 / 2);

		Rigidbody body = GetComponent<Rigidbody>();
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        float baseSpeed = GetComponent<MoveableUnit>().baseSpeed;
        if (body != null)
		{
			body.mass = (float)volFactor;
            

        }
        if (nav != null)
        {
            nav.speed = (float) baseSpeed/ (float)speedFactor;


        }


    }
}
