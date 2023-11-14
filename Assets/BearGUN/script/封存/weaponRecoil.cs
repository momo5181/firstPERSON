using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponRecoil : MonoBehaviour
{
   [Header("位置:")]
   public Transform recoilPosition;
   public Transform rotationPoint;
   [Space(10)]

   [Header("1向後設置,2槍枝抖動 :")]
   public float positionalRecoilSpeed = 8f;
   public float rotationalRecoilSpeed = 8f;
   [Space(10)]

   public float positionalReturnSpeed = 18f;
   public float rotationalReturnSpeed = 38f;
   [Space(10)]
   

   [Header("Amount 設置:")]
   public Vector3 RecoilRotation = new Vector3(10 , 5, 7);
   public Vector3 RecoilKickBack = new Vector3(0.015f , 0f, -0.2f);
   [Space(10)]

   Vector3 rotationalRecoil;
   Vector3 PositionalRecoil;
   Vector3 Rot;
 

    // Update is called once per frame
    void FixedUpdate()
    {
        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed*Time.deltaTime);
        PositionalRecoil = Vector3.Lerp(PositionalRecoil, Vector3.zero, positionalReturnSpeed*Time.deltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, PositionalRecoil, positionalRecoilSpeed*Time.fixedDeltaTime);
        Rot = Vector3.Slerp(Rot, rotationalRecoil, rotationalRecoilSpeed*Time.fixedDeltaTime);
        rotationPoint.localRotation = Quaternion.Euler(Rot);        
    }
    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            Fire();
        }
    }
    void Fire()
    {
        rotationalRecoil += new Vector3(-RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
        PositionalRecoil += new Vector3 (Random.Range(-RecoilKickBack.x, RecoilKickBack.x), Random.Range(-RecoilKickBack.y, RecoilKickBack.y), RecoilKickBack.z);
    }
}
