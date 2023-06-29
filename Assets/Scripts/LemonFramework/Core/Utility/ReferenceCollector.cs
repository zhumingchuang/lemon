using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LemonFramework
{
    [Serializable]
    public class ReferenceCollectorData
    {
        public string key;
        public UnityEngine.Object gameObject;
    }

    public class ReferenceCollectorDataComparer : IComparer<ReferenceCollectorData>
    {
        public int Compare (ReferenceCollectorData x, ReferenceCollectorData y)
        {
            return string.Compare (x.key, y.key, StringComparison.Ordinal);
        }
    }

    public class ReferenceCollector : MonoBehaviour, ISerializationCallbackReceiver
    {
        public List<ReferenceCollectorData> data = new List<ReferenceCollectorData> ();
        private readonly Dictionary<string, UnityEngine.Object> dict = new Dictionary<string, UnityEngine.Object> ();


#if UNITY_EDITOR
        public void Add (string key, UnityEngine.Object obj)
        {
            SerializedObject serializedObject = new SerializedObject (this);
            SerializedProperty dataProperty = serializedObject.FindProperty ("data");
            int i;
            for (i = 0; i < data.Count; i++)
            {
                if (data[i].key == key)
                {
                    break;
                }
            }
            if (i != data.Count)
            {
                SerializedProperty element = dataProperty.GetArrayElementAtIndex (i);
                element.FindPropertyRelative ("gameObject").objectReferenceValue = obj;
            }
            else
            {
                dataProperty.InsertArrayElementAtIndex (i);
                SerializedProperty element = dataProperty.GetArrayElementAtIndex (i);
                element.FindPropertyRelative ("key").stringValue = key;
                element.FindPropertyRelative ("gameObject").objectReferenceValue = obj;
            }
            EditorUtility.SetDirty (this);
            serializedObject.ApplyModifiedProperties ();
            serializedObject.UpdateIfRequiredOrScript ();
        }

        public void Remove (string key)
        {
            SerializedObject serializedObject = new SerializedObject (this);
            SerializedProperty dataProperty = serializedObject.FindProperty ("data");
            int i;
            for (i = 0; i < data.Count; i++)
            {
                if (data[i].key == key)
                {
                    break;
                }
            }
            if (i != data.Count)
            {
                dataProperty.DeleteArrayElementAtIndex (i);
            }
            EditorUtility.SetDirty (this);
            serializedObject.ApplyModifiedProperties ();
            serializedObject.UpdateIfRequiredOrScript ();
        }

        public void Clear ()
        {
            SerializedObject serializedObject = new SerializedObject (this);
            var dataProperty = serializedObject.FindProperty ("data");
            dataProperty.ClearArray ();
            EditorUtility.SetDirty (this);
            serializedObject.ApplyModifiedProperties ();
            serializedObject.UpdateIfRequiredOrScript ();
        }

        public void Sort ()
        {
            SerializedObject serializedObject = new SerializedObject (this);
            data.Sort (new ReferenceCollectorDataComparer ());
            EditorUtility.SetDirty (this);
            serializedObject.ApplyModifiedProperties ();
            serializedObject.UpdateIfRequiredOrScript ();
        }
#endif

        public T Get<T> (string key) where T : class
        {
            UnityEngine.Object dictGo;
            if (!dict.TryGetValue (key, out dictGo))
            {
                return null;
            }
            return dictGo as T;
        }

        public UnityEngine.Object GetObject (string key)
        {
            UnityEngine.Object dictGo;
            if (!dict.TryGetValue (key, out dictGo))
            {
                return null;
            }
            return dictGo;
        }

        public void OnBeforeSerialize ()
        {

        }

        public void OnAfterDeserialize ()
        {
            dict.Clear ();
            foreach (ReferenceCollectorData referenceCollectorData in data)
            {
                if (!dict.ContainsKey (referenceCollectorData.key))
                {
                    dict.Add (referenceCollectorData.key, referenceCollectorData.gameObject);
                }
            }
        }
    }
}
