using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BreakTreeToAxe : MonoBehaviour
{
    public int power=1;

    public ParticleSystem particle;
    RaycastHit hit;

    IEnumerator smokeEff(RaycastHit x)
    {
        ParticleSystem par = Instantiate(particle, x.point, transform.rotation);
        par.Play();
        yield return new WaitForSeconds(.1f);
        Destroy(par);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 4f, Color.red);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("tree"))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
            {

                StartCoroutine(smokeEff(hit));
            }
        }

    }
}
