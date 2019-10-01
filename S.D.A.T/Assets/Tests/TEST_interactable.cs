using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TEST_interactable
    {
        // A Test behaves as an ordinary method
        [Test]
        public void GrowableComponentAddedToInteractableObject()
        {
            var obj = new GameObject();
            obj.AddComponent<Interactable>();
            obj.GetComponent<Interactable>().AddAspect(AspectType.Growable);

           bool hasGrowableComponent = false;
           
           if (obj.GetComponent<Growable>())
           {
               hasGrowableComponent = true;
           }

           Assert.AreEqual(true, hasGrowableComponent);
        }

//        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
//        // `yield return null;` to skip a frame.
//        [UnityTest]
//        public IEnumerator TEST_interactableWithEnumeratorPasses()
//        {
//            // Use the Assert class to test conditions.
//            // Use yield to skip a frame.
//            yield return null;
//        }
    }
}
