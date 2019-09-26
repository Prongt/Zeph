using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
// ReSharper disable All

namespace Tests
{
    public class TEST_damageCalculator
    {

        [Test]
        public void SetsDamageToHalfWith50PercentMitigation()
        {
            int finalDamage = TEST_DamageCalculator.CalculateDamage(10, 0.5f);
            Assert.AreEqual(5, finalDamage);
        }
        
        [Test]
        public void SetsDamageTo2With80PercentMitigation()
        {
            int finalDamage = TEST_DamageCalculator.CalculateDamage(10, .80f);
            Assert.AreEqual(2, finalDamage);
        }
        
        [Test]
        public void SpawnCubeAt10Y()
        {
            var point = new UnityEngine.Vector3(0, 10, 0);
           var t = TEST_DamageCalculator.InstantiateCubeAtPoint(point);
           
            Assert.AreEqual(point, t.transform.position);
        }
    }
}
