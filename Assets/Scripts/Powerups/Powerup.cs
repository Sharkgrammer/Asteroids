using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Powerups
{
    internal class Powerup
    {
        protected float rofAdj = 0;
        protected float rorAdj = 0;
        protected float rosAdj = 0;

        public float getRateOfSpeedAdj()
        {
            return rosAdj;
        }
        public float getRateOfRotationAdj()
        {
            return rorAdj;
        }

        public float getRateOfFireAdj()
        {
            return rofAdj;
        }
    }
}
