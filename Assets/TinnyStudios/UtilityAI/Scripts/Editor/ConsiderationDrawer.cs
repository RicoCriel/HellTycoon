using UnityEditor;
using UnityEngine;

namespace TinnyStudios.AIUtility.Editor
{
    /// <summary>
    /// A customer editor for considerations.
    /// A useful debugging section was added to see the output of the response curve by using the simulated value slider.
    /// </summary>
    [CustomEditor(typeof(Consideration), true)]
    public class ConsiderationDrawer : UnityEditor.Editor
    {
        private float _simulatedValue;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var consideration = (Consideration) target;
            //_agent = EditorGUILayout.ObjectField(_agent, typeof(Agent), true) as Agent;

            GUILayout.Space(10);
            _simulatedValue = EditorGUILayout.Slider("Simulated Value",_simulatedValue, 0, 1);

            GUILayout.Label($"Score Value: {consideration.GetSimulatedScore(_simulatedValue)}");
            GUILayout.Label($"Min Value: {consideration.GetSimulatedScore(0)}");
            GUILayout.Label($"Max Value: {consideration.GetSimulatedScore(1)}");
        }
    }
}