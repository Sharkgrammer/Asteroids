using UnityEngine;
using Transform = UnityEngine.Transform;

namespace Assets.Scripts.Weapons
{
    internal class Pew : Weapon
    { 
        public Pew()
        {
            this.setBullet("Pew");

            this.lifetime = 2;
            this.speed = 100;
            this.damage = 100;
        }

        public override void shoot(Transform transform)
        {
            GameObject bullet = GameObject.Instantiate(this.bullet, transform.position, transform.rotation, null);

            bullet.GetComponent<bulletHandler>().setBulletData(this);

            bullet.GetComponent<AudioSource>().Play();
        }
    }
}
