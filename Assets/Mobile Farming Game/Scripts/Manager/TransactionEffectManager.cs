using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionEffectManager : MonoBehaviour
{
    public static TransactionEffectManager instance;

    [Header("Elements")]
    [SerializeField] private ParticleSystem coinPS;
    [SerializeField] private RectTransform coinRectTransform;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
     private int coinsAmount;
    private Camera camera;

    private void Awake()
    {
        #region SingleTon
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    private void Start()
    {
        camera = Camera.main;
    }

    [NaughtyAttributes.Button]
    private void PlayCoinParticleTest() 
    {
        PlayCoinParticles(100);
    }

    public void PlayCoinParticles(int amount) 
    {
        if (coinPS.isPlaying) return;

        ParticleSystem.Burst burst = coinPS.emission.GetBurst(0);
        burst.count = amount;
        coinPS.emission.SetBurst(0, burst);

        ParticleSystem.MainModule main = coinPS.main;
        main.gravityModifier = 2;

        coinPS.Play();

        coinsAmount = amount;

        StartCoroutine(PlayCoinParticlesCoroutine());
    }

    IEnumerator PlayCoinParticlesCoroutine() 
    {
        yield return new WaitForSeconds(1f);

        //disable the gravity of the coin particles
        ParticleSystem.MainModule main = coinPS.main;
        main.gravityModifier = 0;

        //get and set the particles amount for coin particles
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[coinsAmount];
        coinPS.GetParticles(particles);

        //determine the target position for the coins
        Vector3 direction = (coinRectTransform.position - camera.transform.position).normalized;
        Vector3 targetPosition = camera.transform.position + direction * (Vector3.Distance(camera.transform.position, coinPS.transform.position));

        while (coinPS.isPlaying) 
        {
            coinPS.GetParticles(particles);

            for (int i = 0; i < particles.Length; i++)
            {
                //move all coin particles to the icon
                particles[i].position = Vector3.MoveTowards(particles[i].position, targetPosition, moveSpeed * Time.deltaTime);
                
                if(Vector3.Distance(particles[i].position, targetPosition) < 0.01f) 
                {
                    //destroy all coins when reached target position
                    //particles[i].remainingLifetime = 0;
                    particles[i].position += Vector3.up * 10000;
                    CashManager.instance.AddCoins(1);
                }
            }

            coinPS.SetParticles(particles);
            yield return null;
        }
        
    }
}
