using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectScrambler: MonoBehaviour {
	public Transform[] scrambledObjects;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    public void Start() {
		scramble();
	}

	public void scramble() {
		List<Transform> deck = scrambledObjects.ToList();
		List<Location> result = new List<Location>(scrambledObjects.Length);
		//shuffle
		while (deck.Count > 0) {
			int index = Random.Range(0, deck.Count);
			result.Add(new Location(deck[index]));
			deck.RemoveAt(index);
		}
		//deal
		for (int i = 0; i < scrambledObjects.Length; i++) {
			scrambledObjects[i].SetPositionAndRotation(result[i].position, result[i].rotation);
		}
	}

	private struct Location {
		public Vector3 position;
		public Quaternion rotation;

		public Location(Transform t) {
			position = t.position;
			rotation = t.rotation;
		}
	}
}
