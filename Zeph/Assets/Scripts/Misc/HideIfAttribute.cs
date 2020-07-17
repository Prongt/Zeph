using System;
using UnityEngine;

//[AttributeUsage(AttributeTargets.Field |AttributeTargets.Property |
//                AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]

/// <summary>
/// Allows properties to be either enabled or disabled
/// </summary>
[AttributeUsage(AttributeTargets.Field |AttributeTargets.Property | 
                AttributeTargets.Class, Inherited = true)]
public class HideIfAttribute : PropertyAttribute
{
        //The name of the bool field that will be in control
        public string ConditionalSourceField = "";
        //TRUE = Hide in inspector / FALSE = Disable in inspector 
        public bool HideInInspector = false;
        public bool Reverse = false;

        /// <summary>
        /// Hide or disable property depending on specific bool (DOESNT SUPPORT FloatRefrence editor extensions)
        /// </summary>
        /// <param name="conditionalSourceField"> Bool name as string eg "boolName"</param>
        /// <param name="hideInInspector"> Hidden if true || Disabled if false</param>
        /// <param name="reverse"> Hidden if true || Disabled if false</param>
        public HideIfAttribute(string conditionalSourceField, bool hideInInspector, bool reverse = false)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
            this.Reverse = reverse;
        }
}
