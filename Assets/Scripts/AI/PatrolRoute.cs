using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(PatrolRoute))]
    public class ChildCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Display the script name at the top
            EditorGUILayout.Space();
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((PatrolRoute)target), typeof(PatrolRoute), false);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Update the serialized object to reflect any changes made in the Inspector.
            serializedObject.Update();

            // Get a reference to the target object of the Inspector, which is an instance of the 
            PatrolRoute patrolRoute = (PatrolRoute)target;


            // Display the property field for the "m_PatrolPointNames" serialized field
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PatrolPointNames"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PatrolPointLingerTime"));

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Patrol Point", GUILayout.MinHeight(30), GUILayout.ExpandWidth(true), GUILayout.MinWidth(0)))
            {
                patrolRoute.AddPatolPoint();
            }

            if (GUILayout.Button("Delete Patrol Point", GUILayout.MinHeight(30), GUILayout.ExpandWidth(true), GUILayout.MinWidth(0)))
            {
                patrolRoute.DeletePatrolPoint();
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Reset Patrol Point Transforms", GUILayout.MinHeight(30), GUILayout.ExpandWidth(true), GUILayout.MinWidth(0)))
            {
                patrolRoute.ResetPatrolPointTransforms();
            }

            if (GUILayout.Button("Delete All Patrol Points", GUILayout.MinHeight(30), GUILayout.ExpandWidth(true), GUILayout.MinWidth(0)))
            {
                patrolRoute.DeleteAllPatrolPoints();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Display the property fields for the debug settings
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PatrolPointIndicatorSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_EnableIndicators"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_IndicatorColor"));

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    public class PatrolRoute : MonoBehaviour
    {
        [Header("Patrol Route Settings")]
        [SerializeField] private string m_PatrolPointNames = "Patrol Point";
        [SerializeField] private float m_PatrolPointLingerTime = 15f;

        [Header("Debug Settings")]
        [SerializeField][Range(0.1f, 1f)] private float m_PatrolPointIndicatorSize = 0.25f;
        [SerializeField] private bool m_EnableIndicators = true;
        [SerializeField] private Color m_IndicatorColor = Color.cyan;

        // Private vars
        private int m_PatrolPoints = 0;

        private void OnValidate()
        {
            // Ensure that the number of children is always non-negative
            m_PatrolPoints = Mathf.Max(m_PatrolPoints, 0);
        }

        /// <summary>
        /// Gets the time to linger at east patrol point.
        /// </summary>
        /// <returns>The time to linger at each patrol point</returns>
        public float GetLingerTime()
        {
            return m_PatrolPointLingerTime;
        }

        /// <summary>
        /// Adds a new patrol point.
        /// </summary>
        public void AddPatolPoint()
        {
            m_PatrolPoints++;

            if (m_PatrolPoints == 1)
            {
                CreatePatrolPoint(m_PatrolPoints - 1);
            }
            else
            {
                GameObject patrolPoint = CreatePatrolPoint(m_PatrolPoints - 1);
                Transform previousPatrolPoint = transform.GetChild(m_PatrolPoints - 2);
                patrolPoint.transform.position = previousPatrolPoint.position;
            }
        }

        /// <summary>
        /// Removes the most recently added patrol point.
        /// </summary>
        public void DeletePatrolPoint()
        {
            if (m_PatrolPoints > 0)
            {
                m_PatrolPoints--;
                DeleteChild(m_PatrolPoints);
            }
        }

        /// <summary>
        /// Creates a new patrol point GameObject with a specific index.
        /// </summary>
        /// <param name="index">The index of the patrol point.</param>
        private GameObject CreatePatrolPoint(int index)
        {
            GameObject patrolPoint = new GameObject(m_PatrolPointNames + " " + (index + 1));
            patrolPoint.transform.parent = transform;

            // Check if the index is out of bounds
            if (index >= transform.childCount)
            {
                // Adjust the index to the maximum valid index
                index = transform.childCount - 1;
            }

            // Get the position of the previous patrol point
            Vector3 previousPosition = Vector3.zero;
            if (index > 0)
            {
                Transform previousPatrolPoint = transform.GetChild(index - 1);
                previousPosition = previousPatrolPoint.position;
            }

            // Set the position of the new patrol point
            patrolPoint.transform.position = previousPosition;

            return patrolPoint;
        }

        /// <summary>
        /// Deletes the patrol point GameObject at a specific index.
        /// </summary>
        /// <param name="index">The index of the patrol point to delete.</param>
        private void DeleteChild(int index)
        {
            if (transform.childCount > index)
            {
                Transform child = transform.GetChild(index);
                DestroyImmediate(child.gameObject);
            }
        }

        /// <summary>
        /// Resets the local transforms (position, rotation, scale) of all patrol points to their original values.
        /// </summary>
        public void ResetPatrolPointTransforms()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localPosition = Vector3.zero;
                transform.GetChild(i).localRotation = Quaternion.identity;
                transform.GetChild(i).localScale = Vector3.one;
            }
        }

        /// <summary>
        /// Deletes all patrol points from the parent.
        /// </summary>
        public void DeleteAllPatrolPoints()
        {
            while (transform.childCount > 0)
            {
                Transform child = transform.GetChild(0);
                DestroyImmediate(child.gameObject);
            }

            m_PatrolPoints = 0;
        }

        private void OnDrawGizmos()
        {
            if (!m_EnableIndicators) return;

            Gizmos.color = m_IndicatorColor;

            // Draw spheres for each patrol point and lines connecting them
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetPatrolPoint(i), m_PatrolPointIndicatorSize);
                Gizmos.DrawLine(GetPatrolPoint(i), GetPatrolPoint(j));
            }
        }

        /// <summary>
        /// Gets the index of the next patrol point / wraps around to the first point.
        /// </summary>
        /// <param name="i">The current patrol point index.</param>
        /// <returns>The index of the next patrol point.</returns>
        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }

            return i + 1;
        }

        /// <summary>
        /// Gets the position of a patrol point.
        /// </summary>
        /// <param name="i">The index of the patrol point.</param>
        /// <returns>The position of the patrol point.</returns>
        public Vector3 GetPatrolPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
