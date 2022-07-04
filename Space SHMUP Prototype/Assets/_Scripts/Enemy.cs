using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10f;
    public int score = 100;
    public float showDamageDuration = 0.1f;
    public float powerUpDropChance = 1f;

    [Header("Set Dynamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials;
    public bool showingDamage = false;
    public float damageDoneTime;
    public bool norifiedOfDestruction = false;

    protected BoundsCheck bndCheck;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }
    public Vector3 pos
    {
        get 
        {
            return (this.transform.position);
        }
        set 
        {
            this.transform.position = value;
        }
    }  
    void Update()
    {
        Move();
        if (showingDamage && Time.time > damageDoneTime) 
        {
            UnShowDamage();
        }
        if (bndCheck != null && bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Move() 
    {
        Vector3 temPos = pos;
        temPos.y -= speed * Time.deltaTime;
        pos = temPos;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                ShowDamage();
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {
                    if (!norifiedOfDestruction) 
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    norifiedOfDestruction = true;
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
        }
    }
    void ShowDamage() 
    {
        foreach (var item in materials)
        {
            item.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }
    void UnShowDamage() 
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
