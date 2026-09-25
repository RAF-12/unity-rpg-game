using System;
using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    [SerializeField]
    public GameObject ObjectFollow;
    [SerializeField]
    private Vector3 targetPosicion;
    [SerializeField]
    private float camaraSpeed = 4.01f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = new Vector3(ObjectFollow.transform.position.x, ObjectFollow.transform.position.y, this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        targetPosicion = new Vector3(ObjectFollow.transform.position.x, ObjectFollow.transform.position.y, this.transform.position.z);
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosicion, camaraSpeed * Time.deltaTime);
    }
}
