using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectScrambler: MonoBehaviour {
    public int _skip = 0;
	public Transform[] scrambledObjects;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    public void Start() {
        _skip = Mathf.Clamp(_skip, 0, scrambledObjects.Length - 2);
        for (int i = 0; i < scrambledObjects.Length; i++)
        {
            scrambledObjects[i].gameObject.SetActive(false);
        }
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
		for (int i = 0; i < scrambledObjects.Length - _skip; i++) {
			scrambledObjects[i].SetPositionAndRotation(result[i].position, result[i].rotation);
            scrambledObjects[i].gameObject.SetActive(true);

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
