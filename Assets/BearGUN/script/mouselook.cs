using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouselook : MonoBehaviour
{
    public float mousesensitivity =40f;//滑鼠敏感度
    public Transform playerbody;//設定玩家身體
    float xRotation=0f;
    // Start is called before the first frame update
    void Start()
    {
     Cursor.lockState =CursorLockMode.Locked;//游標鎖定並隱藏   
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX =Input.GetAxis("Mouse X")*mousesensitivity*Time.deltaTime;//取得滑鼠x坐標
        float mouseY =Input.GetAxis("Mouse Y")*mousesensitivity*Time.deltaTime;//取得滑鼠y坐標
        playerbody.Rotate(Vector3.up*mouseX);//玩家身體左右旋轉

        xRotation -= mouseY;
        xRotation=Mathf.Clamp(xRotation,-90f,90f);//限制滑鼠上下移動不超過90度
        transform.localRotation= Quaternion.Euler(xRotation,0f,0f);//滑鼠取得y坐摽後繞著玩家身體旋轉
    }
}
