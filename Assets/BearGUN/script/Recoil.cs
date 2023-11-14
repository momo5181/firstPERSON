using UnityEngine;

public class Recoil : MonoBehaviour
{

    private bool isAiming;

    private Vector3 currentRotation;
    private Vector3 TargetRotation;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float aimrecoilX;
    [SerializeField] private float aimrecoilY;
    [SerializeField] private float aimrecoilZ;

    [SerializeField] private float snappiness;//設置相機抖動
    [SerializeField] private float returnSpeed;//返回速度
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetButton("Fire2"))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }
        TargetRotation = Vector3 .Lerp(TargetRotation , Vector3.zero , returnSpeed*Time.deltaTime);
        currentRotation= Vector3.Slerp(currentRotation, TargetRotation, snappiness*Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    public void RecoilFire()
    {
        if(isAiming) TargetRotation += new Vector3(aimrecoilX, Random.Range(-aimrecoilY,aimrecoilY),Random.Range(-aimrecoilZ,aimrecoilZ));
        else TargetRotation += new Vector3(recoilX, Random.Range(-recoilY,recoilY),Random.Range(-recoilZ,recoilZ));
    }
}
