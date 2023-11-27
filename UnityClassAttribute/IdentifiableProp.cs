using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityClassAttribute
{   
    [DisallowMultipleComponent]
    [AddComponentMenu("My Menu/ Identifiable Prop")]
    [Component("Identifiable Prop", "This component will be used in labels to identify the prop")]
    public class IdentifiableProp : MonoBehaviour {
        [header("Interactions")]

        [SerializeField]
        [Tooltip("Wheter or not this prop can be moved by the player")]
        
        [Header("Shown Details")]

        [SerializeField]
        [Tooltip("Name of the prop as displayed to the player")]
        string m_name = "prop";

        [SerializeField]
        [Tooltip("Description of the prop as displayed to the pplayer")]
        [Multiline]
        string m_description = "Prop Description";

    }
    
}