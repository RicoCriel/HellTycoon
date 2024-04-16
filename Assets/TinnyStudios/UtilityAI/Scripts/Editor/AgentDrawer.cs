using UnityEditor;
using UnityEngine;

namespace TinnyStudios.AIUtility.Editor
{
    /// <summary>
    /// The custom editor for <see cref="Agent"/>
    /// This is mostly use for debugging purposes. Exposes a few buttons to change the Agent's current plan such as aborting.
    /// It also shows a list of actions and the scores.
    /// </summary>
    [CustomEditor(typeof(Agent), true)]
    public class AgentDrawer : UnityEditor.Editor
    {
        public const float SmallLabelSize = 100;
        public bool ShowInfo;

        public GUIStyle myFoldoutStyle => new GUIStyle(EditorStyles.foldout)
        {
            richText = true
        };

        public GUIStyle GeneralStyle => new GUIStyle(EditorStyles.label)
        {
            richText = true,
            wordWrap = true
        };

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var agent = (Agent)target;


            if (!Application.isPlaying)
                agent.SearchForActionsInParent();

            var actions = agent.Actions;

            GUI.skin.label.richText = true;

            GUILayout.Box("Agent Debugger");

            if (actions.Count > 0)
            {
                GUILayout.Space(10);

                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Find Plan"))
                        agent.FindPlan();

                    if (GUILayout.Button("Execute Plan"))
                        agent.ExecutePlan();

                    if (GUILayout.Button("Abort Plan"))
                        agent.AbortPlan();
                }

                GUILayout.EndHorizontal();

                ShowInfo = EditorGUILayout.Foldout(ShowInfo, "More Info", true);

                if (ShowInfo)
                {
                    EditorGUILayout.LabelField("<b>Find plan</b> will search for best current action", GeneralStyle);
                    EditorGUILayout.LabelField("<b>Execute Plan</b> will step through 1 frame of the plan execution.", GeneralStyle);
                    EditorGUILayout.LabelField("<b>Abort Plan</b> will change the state to abort. If you don't have update, you'll need to press Execute Plan to continue.", GeneralStyle);
                }

                GUILayout.EndVertical();

                GUILayout.Space(10);

                GUILayout.Label("Actions");

                GUILayout.BeginVertical(EditorStyles.helpBox);

                foreach (var utilityAction in actions)
                {
                    GUILayout.BeginHorizontal();

                    var displayName = utilityAction == agent.CurrentAction
                        ? utilityAction.Name.ToColor(Color.green)
                        : utilityAction.Name.ToBold();

                    utilityAction.FoldOutEnabled = EditorGUILayout.Foldout(utilityAction.FoldOutEnabled, $"{displayName}:", true, myFoldoutStyle);
                    EditorGUILayout.LabelField($"Score: {utilityAction.Score:F2}", GUILayout.Width(SmallLabelSize));

                    GUILayout.EndHorizontal();

                    if (utilityAction.FoldOutEnabled)
                    {
                        FormatLabelHorizontal("Available", $"{utilityAction.IsAvailable()}");
                        FormatLabelHorizontal("Elasped", $"{utilityAction.TimeWatch.GetTotalSeconds():F2}");
                        FormatLabelHorizontal("State", $"{utilityAction.State}");

                        FormatLabelHorizontal("Considerations", "");
                        EditorGUI.indentLevel++;
                        foreach (var consideration in utilityAction.Considerations)
                        {
                            EditorGUILayout.LabelField($"{consideration.name}: Score: {consideration.GetScore(agent, utilityAction)}");
                        }
                        EditorGUI.indentLevel--;
                    }
                }

                GUILayout.EndVertical();

                Repaint();

            }
            else
            {
                GUILayout.Box("Require actions to show info.");
            }
        }

        public void FormatLabelHorizontal(params string[] labels)
        {
            GUILayout.BeginHorizontal();

            for (var i = 0; i < labels.Length; i++)
            {
                var label = labels[i];
                if (i == 0)
                    label = label.ToBold();

                EditorGUILayout.LabelField(label, GeneralStyle, GUILayout.Width(SmallLabelSize));
            }

            GUILayout.EndHorizontal();
        }
    }
}