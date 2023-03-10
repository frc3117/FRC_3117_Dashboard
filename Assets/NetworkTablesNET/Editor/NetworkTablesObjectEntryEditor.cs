using UnityEditor;

namespace NetworkTablesNET.Editor
{
    [CustomEditor(typeof(NetworkTablesObjectEntry))]
    public class NetworkTablesObjectEntryEditor : UnityEditor.Editor
    {
        private NetworkTablesObjectEntry _entry;
        
        private void OnEnable()
        {
            _entry = target as NetworkTablesObjectEntry;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
