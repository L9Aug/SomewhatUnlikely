using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    // the muzzle of the gun
    public Transform Muzzle;
    // the number of bullets the gun can store.
    public int MagazineSize;
    // the time it takes this gun to reload.
    public float ReloadTime;
    // the number of bullets this gun can fire per second.
    public float RateofFire;
    // the damage of each bullet if it hits a target.
    public float Damage;
    // the range of the gun.
    public float Range = 1000f;

    bool Reloading = false;
    float reloadTimer = 0;

    bool OnCooldown = false;
    float cooldownTimer = 0;

    int Magazine;

    public delegate Vector3 TargetFunc();

	// Use this for initialization
	void Start () {
        Magazine = MagazineSize;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Fires this gun if the gun is able to fire.
    /// </summary>
    /// <param name="Target">A Fucntion that returns the target you would like to shoot at (This way it only calcs the target if the gun will fire).</param>
    /// <param name="TargetLayer">The layer of the target you wish to hit.</param>
    /// <param name="Varience">The amount that the gun can miss by (in unity units).</param>
    /// <param name="DebugDraw">Should the bullet path be drawn using debug draws?</param>
    /// <param name="InGameDraw">Should the bullet path be drawn using in game visulisation?</param>
    /// <returns>Returns True if the gun fired successfully.</returns>
    public virtual bool Fire(TargetFunc Target, LayerMask TargetLayer, float Varience = 0, bool DebugDraw = false, bool InGameDraw = false)
    {
        if (!Reloading)
        {
            if (!OnCooldown)
            {
                if (Magazine > 0)
                {
                    //can fire
                    RaycastHit hit = new RaycastHit();
                    Vector3 bulletEndDestination = Target() + (Varience * Random.insideUnitSphere);
                    hit.point = bulletEndDestination;
                    Ray BulletPath = new Ray(Muzzle.position, (bulletEndDestination - Muzzle.position).normalized);
                    if (Physics.Raycast(BulletPath, out hit, Range, TargetLayer))
                    {
                        hit.collider.SendMessage("Hit", Damage, SendMessageOptions.DontRequireReceiver);
                    }
                    if (DebugDraw) Debug.DrawLine(BulletPath.origin, hit.point, Color.green, 1f);
                    //if (DebugDraw) Debug.DrawRay(BulletPath.origin, BulletPath.direction * Range, Color.green, 1f);
                    if (InGameDraw) DrawBulletPathInGame(BulletPath, 1000f); // comming soon if wanted
                    FiredGun();
                    return true;
                }
                else
                {
                    Reload();
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Fires this gun if the gun is able to fire.
    /// </summary>
    /// <param name="Target">The location you want to shoot at.</param>
    /// <param name="TargetLayer">The layer of the target you wish to hit.</param>
    /// <param name="Varience">The amount that the gun can miss by (in unity units).</param>
    /// <param name="DebugDraw">Should the bullet path be drawn using debug draws?</param>
    /// <param name="InGameDraw">Should the bullet path be drawn using in game visulisation?</param>
    /// <returns>Returns True if the gun fired successfully.</returns>
    public virtual bool Fire(Vector3 Target, LayerMask TargetLayer, float Varience = 0, bool DebugDraw = false, bool InGameDraw = false)
    {
        if (!Reloading)
        {
            if (!OnCooldown)
            {
                if (Magazine > 0)
                {
                    //can fire
                    RaycastHit hit = new RaycastHit();
                    Vector3 bulletEndDestination = Target + (Varience * Random.insideUnitSphere);
                    hit.point = bulletEndDestination;
                    Ray BulletPath = new Ray(Muzzle.position, (bulletEndDestination - Muzzle.position).normalized);
                    if (Physics.Raycast(BulletPath, out hit, Range, TargetLayer))
                    {
                        hit.collider.SendMessage("Hit", Damage, SendMessageOptions.DontRequireReceiver);
                    }
                    if (DebugDraw) Debug.DrawLine(BulletPath.origin, hit.point, Color.green, 1f);
                    //if (DebugDraw) Debug.DrawRay(BulletPath.origin, BulletPath.direction * Range, Color.green, 1f);
                    if (InGameDraw) DrawBulletPathInGame(BulletPath, 1000f); // comming soon if wanted
                    FiredGun();
                    return true;
                }
                else
                {
                    Reload();
                }
            }
        }
        return false;
    }

    void FiredGun()
    {
        OnCooldown = true;
        --Magazine;
        StartCoroutine(CooldownTick());
    }

    /// <summary>
    /// Comming soon if people want it to.
    /// </summary>
    /// <param name="bulletPath"></param>
    /// <param name="BulletLength"></param>
    void DrawBulletPathInGame(Ray bulletPath, float BulletLength)
    {

    }

    public virtual void Reload()
    {
        if (!Reloading)
        {
            Reloading = true;
            StartCoroutine(ReloadTick());
        }
    }

    /// <summary>
    /// CooldownTick happens at the end of each frame that the gun is cooling down after fireing.
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownTick()
    {
        while (OnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if(cooldownTimer >= (1f / RateofFire))
            {
                OnCooldown = false;
                cooldownTimer = 0;
            }
            yield return null;
        }
    }

    /// <summary>
    /// ReloadTick happens at the end of each frame whilst reloading.
    /// </summary>
    /// <returns></returns>
    IEnumerator ReloadTick()
    {
        while (Reloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= ReloadTime)
            {
                Reloading = false;
                reloadTimer = 0;
                Magazine = MagazineSize;
            }
            yield return null;
        }
    }
}
