using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public abstract class Weapon
    {
        protected GameObject bullet;
        protected AudioSource audio;
        protected float speed = 100.0f;
        protected float damage = 50.0f;
        protected int lifetime = 2;
        protected string name;

        public float weaponModifer = 1;

        public void setBullet(string name)
        {
            this.name = name;
            this.bullet = (GameObject) Resources.Load("Bullets/" + name);
        }

        public float getDamage()
        {
            return damage;
        }

        public float getSpeed()
        {
            return speed;
        }

        public int getLifetime()
        {
            return lifetime;
        }

        public string getName()
        {
            return name;
        }

        public abstract void shoot(Transform transform);

    }

}
