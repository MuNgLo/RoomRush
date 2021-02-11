using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunSwarmPatterns : MonoBehaviour
{
    private Transform[] _swarmPoints = null;
    private Transform _swarmMid = null;
    private List<Transform[]> _patterns = null;
    public int _lastPatternUsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        _swarmPoints = new Transform[transform.childCount - 1];
        _patterns = new List<Transform[]>();
        int i = 0;
        foreach(Transform child in transform)
        {
            if(child.name == "SwarmMid")
            {
                _swarmMid = child;
            }
            else
            {
                _swarmPoints[i] = child;
                i++;
            }
        }
        for (int x = 0; x < 20; x++)
        {
            _patterns.Add(MakePattern());
        }
    }

    private Transform[] MakePattern()
    {
        List<int> pulled = new List<int>();
        int nbToPull = 20;
        Transform[] pattern = new Transform[nbToPull];
        pattern[0] = _swarmMid;
        for (int i = 1; i < nbToPull;)
        {
            int pull = UnityEngine.Random.Range(0, _swarmPoints.Length);
            if (!pulled.Contains(pull))
            {
                pulled.Add(pull);
                pattern[i] = _swarmPoints[pull];
                i++;
            }
        }
        return pattern;
    }

    public Transform[] GetPattern()
    {
        if (_lastPatternUsed > _patterns.Count - 1)
        {
            _lastPatternUsed = 0;
        }
        _lastPatternUsed++;
        return _patterns[_lastPatternUsed - 1];
    }
}
