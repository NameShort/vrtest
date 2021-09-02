using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHP : MonoBehaviour
{
    public int hp = 3;
    public List<GameObject> trees;

    
    Rigidbody rig;
    GameObject tree;
    Animator ani;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
        ani = GetComponent<Animator>();
        rig.freezeRotation = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.CompareTag("axe"))
        {
            int power = collision.gameObject.GetComponent<BreakTreeToAxe>().power;
            hp -= power;
            ani.SetBool("isChop", true);
            Debug.Log(collision.gameObject.tag + ", " + hp);

            if (hp <= 0)
            {
                rig.useGravity = true;
                rig.freezeRotation = false;
                Debug.Log("hp : 0 ");
                StartCoroutine(createTree());
                hp = 15;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        ani.SetBool("isChop", false);
    }

    IEnumerator createTree()
    {
        
        yield return new WaitForSeconds(5f);

        gameObject.SetActive(false);
        for (int i = 0; i < trees.Count; i++)
        {
            tree = Instantiate(trees[i], transform.position, transform.rotation);
        }
    }
}
