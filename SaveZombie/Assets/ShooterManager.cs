using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform start;
    public Transform end;
    public GameObject bullet;

    public void Begin()
    {
        StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {
        GameObject bull = Instantiate(bullet);
        bull.transform.position = start.position;
        bull.GetComponent<Rigidbody>().velocity = (end.position - start.position).normalized * 0.2f;
        yield return new WaitForSeconds(1f);
        StartCoroutine(ShootingRoutine());
    }
}
