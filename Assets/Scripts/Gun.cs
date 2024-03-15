using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Physical Weapon Data")]
    public GameObject magazineObject;
    public GameObject bulletRaycastPoint;
    public GameObject cartridgeToEject;
    public GameObject cartridgeEjectPoint; // rotation represents ejection direction
    public GameObject muzzleFlash;

    [Header("Particle Data")]
    public ParticleSystem internalSmokeSpawnPoint;
    public ParticleSystem externalSmokeSpawnPoint;
    public ParticleSystem muzzleGasPoint;


    [Header("Sound Data")]
    public AudioClip fireSound;
    public AudioClip magazineInsertSound;

    [Header("Digital Weapon Data")]
    private int ammoRemainingInMagazine;
    public bool magazineInserted;
    public bool bulletInChamber;
    public float delayBetweenFiring;

    [Header("Damage Properties")]
    public float baseDamage;
    public bool exponentiateDamageFalloff;
    public float damageFalloffRate;
    [SerializeField]
    public SerializableDictionary<Material, float> wallBangableMaterials;
    
    private void Start()
    {
        ammoRemainingInMagazine = magazineObject.GetComponent<Magazine>().roundsRemaining;
    }
    public enum ActionType
    {
        SINGLE,
        DOUBLE,
        BOLT,
        PUMP,
        LEVER,
        BREAK
    }
    public enum FireType
    {
        BOLT,
        SEMI,
        AUTO
    }
    public ActionType actionType;
    public FireType fireType;
    public void Fire()
    {
        // spawn bullet raytrace
        // place decal
        // check for collisions
    }
    public void CycleRear()
    {
        // spawn smoke particles out of bullet well
        EjectCartridge();
    }
    internal void EjectCartridge()
    {
        // spawn spent cartridge
    }
}
