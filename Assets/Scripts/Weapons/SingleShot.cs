using UnityEngine;
using Transform = UnityEngine.Transform;

namespace Assets.Scripts.Weapons
{
    internal class SingleShot : Weapon
    { 
        public SingleShot()
        {
            this.setBullet("SingleShot");

            this.lifetime = 2;
            this.speed = 100;
            this.damage = 50;
        }

        public override void shoot(Transform transform)
        {
            GameObject bullet = GameObject.Instantiate(this.bullet, transform.position, transform.rotation, null);

            //float s = this.speed + (this.speed - (this.speed * this.weaponModifer));

            bullet.GetComponent<bulletHandler>().setBulletData(this);

            bullet.GetComponent<AudioSource>().Play();
        }
    }
}
