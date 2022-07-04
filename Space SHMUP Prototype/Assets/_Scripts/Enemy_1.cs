using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Set in Inspector")]
    public float waveFrequency = 2f;
    public float waveWidth = 4f;
    public float waveRotY = 45f;

    private float x0;
    private float bithTime;

    void Start()
    {
        x0 = pos.x;
        bithTime = Time.time;
    }
    public override void Move()
    {
        Vector3 tempPos = pos;
        float age = Time.time - bithTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);
        base.Move();
        //print (bndCheck)
    }


}
