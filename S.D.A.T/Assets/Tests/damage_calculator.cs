using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class damage_calculator
    {

        [Test]
        public void CalculateDamage_setsDamageToHalfWith50PercentMitigation()
        {
            int finalDamage = DamageCalculator.CalculateDamage(10, 0.5f);
            Assert.AreEqual(5, finalDamage);
        }
        
        [Test]
        public void CalculateDamage_setsDamageTo2With80PercentMitigation()
        {
            int finalDamage = DamageCalculator.CalculateDamage(10, .80f);
            Assert.AreEqual(2, finalDamage);
        }

    }
}
