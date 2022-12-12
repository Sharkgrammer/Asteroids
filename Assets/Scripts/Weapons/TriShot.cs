using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;

namespace Assets.Scripts.Weapons
{
    internal class TriShot : Weapon
    {
        public int angle;

        public TriShot()
        {
            this.setBullet("SingleShot");

            this.lifetime = 2;
            this.speed = 70;
            this.damage = 20;

            this.angle = 30;
        }

        public override void shoot(Transform transform)
        {
            Vector3 p = transform.position;
            Quaternion q = transform.rotation;

            shootBullet(p, q * Quaternion.Euler(new Vector3(0, 0, angle * this.weaponModifer)));
            shootBullet(p, q, true);
            shootBullet(p, q * Quaternion.Euler(new Vector3(0, 0, -angle * this.weaponModifer)));
        }

        private void shootBullet(Vector3 p, Quaternion q, bool audio = false)
        {
            GameObject bullet = GameObject.Instantiate(this.bullet, p, q, null);
            bullet.GetComponent<bulletHandler>().setBulletData(this);

            if (audio)
            {
                bullet.GetComponent<AudioSource>().Play();
            }
        }
    }
}
