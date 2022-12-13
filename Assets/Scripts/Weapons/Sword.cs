using System.Collections.Generic;
using UnityEngine;
using Transform = UnityEngine.Transform;

namespace Assets.Scripts.Weapons
{
    internal class Sword : Weapon
    {

        List<GameObject> swordPieces;

        public Sword()
        {
            this.setBullet("Sword");

            this.lifetime = 2;
            this.speed = 16;
            this.damage = 25;

            swordPieces = new List<GameObject>();
        }

        public override void shoot(Transform transform)
        {
            GameObject bullet = GameObject.Instantiate(this.bullet, transform.position, transform.rotation, null);

            bullet.GetComponent<bulletHandler>().setBulletData(this);
            
            bullet.transform.SetParent(transform);
            bullet.transform.localPosition = new Vector3(0, 0, 0);

            bullet.GetComponent<AudioSource>().Play();

            swordPieces.Add(bullet);
            swordPieces.RemoveAll(b => b == null);

            
        }

    }
}
