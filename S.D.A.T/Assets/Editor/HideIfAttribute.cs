﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class HideIfAttribute : PropertyAttribute
{
    
    //The name of the bool field that will be in control
        public string ConditionalSourceField = "";
        //TRUE = Hide in inspector / FALSE = Disable in inspector 
        public bool HideInInspector = false;
 
        public HideIfAttribute(string conditionalSourceField)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = false;
        }
 
        public HideIfAttribute(string conditionalSourceField, bool hideInInspector)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
        }
}
