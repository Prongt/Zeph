using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Allows properties to be hidden or disabled via attribute in the inspector
/// </summary>
[CustomPropertyDrawer(typeof(HideIfAttribute))]
public class HideIfPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //get the attribute data
        HideIfAttribute hideIfAttribute = (HideIfAttribute)attribute;
        //check if the propery we want to draw should be enabled
        bool enabled = HideIfResult(hideIfAttribute, property);
        if (hideIfAttribute.Reverse)
        {
            enabled = !enabled;
        }
 
        //Enable/disable the property
        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
 
        //Check if we should draw the property
        if (!hideIfAttribute.HideInInspector || enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
 
        //Ensure that the next property that is being drawn uses the correct settings
        GUI.enabled = wasEnabled;
    }
    
    private bool HideIfResult(HideIfAttribute attribute, SerializedProperty property)
    {
        bool enabled = true;
        //Look for the sourcefield within the object that the property belongs to
        string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath = propertyPath.Replace(property.name, attribute.ConditionalSourceField); //changes the path to the conditionalsource property path
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);
 
        if (sourcePropertyValue != null)
        {
            enabled = sourcePropertyValue.boolValue;
        }
        else
        {
            Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + attribute.ConditionalSourceField);
        }
 
        return enabled;
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        HideIfAttribute condHAtt = (HideIfAttribute)attribute;
        bool enabled = HideIfResult(condHAtt, property);
 
        if (!condHAtt.HideInInspector || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            //The property is not being drawn
            //We want to undo the spacing added before and after the property
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
