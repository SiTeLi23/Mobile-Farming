using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform cropRenderer;
    [SerializeField] private ParticleSystem harvestedParticles;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScaleUp()
    {
        cropRenderer.gameObject.LeanScale(Vector3.one,1).setEase(LeanTweenType.easeOutBack);
    }

    public void ScaleDown()
    {
        cropRenderer.gameObject.LeanScale(Vector3.zero, 1).
            setEase(LeanTweenType.easeOutBack).setOnComplete(() => Destroy(gameObject));

        harvestedParticles.transform.parent = null;
        harvestedParticles.gameObject.SetActive(true);
        harvestedParticles.Play();
    }

}
