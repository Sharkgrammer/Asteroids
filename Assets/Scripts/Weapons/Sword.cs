using UnityEngine;
using Transform = UnityEngine.Transform;

namespace Assets.Scripts.Weapons
{
    internal class Sword : Weapon
    {

        public Sword()
        {
            this.setBullet("Sword");

            this.lifetime = 2;
            this.speed = 15;
            this.damage = 12;

            this.rorAdj = 0.25f;
            this.rofAdj = 0.7f;
        }

        public override void shoot(Transform transform)
        {
            GameObject bullet = GameObject.Instantiate(this.bullet, transform.position, transform.rotation, null);

            bullet.GetComponent<bulletHandler>().setBulletData(this);
            
            bullet.transform.SetParent(transform);
            bullet.transform.localPosition = new Vector3(0, 0, 0);

            bullet.GetComponent<AudioSource>().Play();
        }

    }
}
