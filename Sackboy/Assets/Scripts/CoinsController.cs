using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinsController : MonoBehaviour
{
    public float numberOfCoins;
    public float radius;
    public float pickupSpeed;
    public float pickupDistance;
    private string coinsTag = "Coin";
    public List<Transform> coins;
    public GameObject coinPrefab;

    void Start()
    {
        coins = new List<Transform>();
        for (int i = 0; i < 10; i++)
        {
            var coin = Instantiate(coinPrefab, new Vector3(0, 0.805f, Random.Range(9, 20)), Quaternion.identity);
        }
    }

    void FixedUpdate()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag(coinsTag) && !coins.Contains(collider.transform))
            {
                coins.Add(collider.transform);
            }
        }

        foreach (var coin in coins)
        {
            if (Vector3.Distance(transform.position,coin.position) <= pickupDistance)
            {
                numberOfCoins++;
                coin.gameObject.SetActive(false);
                coins.Remove(coin);
            }
            else
            {
                float step = pickupSpeed * Time.deltaTime;
                coin.position = Vector3.MoveTowards(coin.position, transform.position, step);
            }
        }
    }
}
