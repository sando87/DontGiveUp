using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMainLogic : MonoBehaviour
{
    Animator anim = null;

    public float MoveSpeed;
    Vector3 DirForward = new Vector3(1, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim != null)
            transform.Translate(DirForward * MoveSpeed * Time.deltaTime);

        //if (Input.GetKey(KeyCode.A))
        //    anim.SetBool("Walk", true);
        //else
        //    anim.SetBool("Walk", false);
        //
        //if (Input.GetKeyDown(KeyCode.S))
        //    anim.SetTrigger("Attack");
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 dir = other.transform.position - transform.position;
        dir.y = 0;
        dir.Normalize();
        Vector3 pos = transform.position;
        pos.y = other.transform.position.y;
        Ray ray = new Ray(pos, dir);
        float pushPower = 0;
        if (other.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            pushPower = hit.distance;

        pushPower = pushPower < 0.1 ? 0.1f : pushPower;
        pushPower = (1 / (pushPower * 5)) + 1.0f; //0.1f~ 1.0f 스케일을 3.0f~1.25f스케일로 변환
        Vector3 Force = -dir * pushPower;
        transform.Translate(Force * MoveSpeed * Time.deltaTime);
    }


}
