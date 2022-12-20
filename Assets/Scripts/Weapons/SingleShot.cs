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
            this.speed = 120;
            this.damage = 50;

            this.rofAdj = 0.2f;
            this.scaleAdj = 3.0f;
        }

        public override void shoot(Transform transform)
        {
            GameObject bullet = GameObject.Instantiate(this.bullet, transform.position, transform.rotation, null);

            bullet.GetComponent<bulletHandler>().setBulletData(this);

            bullet.GetComponent<AudioSource>().Play();
        }
    }
}
