using net.shutosg.UnityEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace net.shutosg.UniEaseCopy
{
    public class UniEaseCopyWindow : EditorWindow
    {
        private Label logTypeLabel;
        private Label logText;
        private Label keyCountText;
        private ListView keyframeList;

        private Button pasteEaseButton;
        private Button pasteValueButton;

        [MenuItem("Window/Animation/UniEaseCopy")]
        public static void ShowWindow()
        {
            var openedWindows = (EditorWindow[])Resources.FindObjectsOfTypeAll(typeof(UniEaseCopyWindow));
            var window = openedWindows.Length == 0 ? CreateInstance<UniEaseCopyWindow>() : openedWindows[0];
            window.titleContent = new GUIContent("UniEaseCopy");
            window.ShowUtility();
        }

        private static T LoadAsset<T>(string assetName) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(DirectoryAnchor.Find("EditorResources") + assetName);
        }

        private static VisualTreeAsset LoadTreeAsset(string assetName) => LoadAsset<VisualTreeAsset>(assetName);

        private static StyleSheet LoadStyleAsset(string assetName) => LoadAsset<StyleSheet>(assetName);

        public void CreateGUI()
        {
#if UNITY_2020_1_OR_NEWER
            var index = LoadTreeAsset("Index.uxml").CloneTree();
#else
            var index = LoadTreeAsset("IndexWithoutStyle.uxml").CloneTree();
            index.styleSheets.Add(LoadStyleAsset("Index.uss"));
#endif
            rootVisualElement.Add(index);
            logText = index.Q<Label>("LogMessageText");
            logTypeLabel = index.Q<Label>("LogTypeText");
            keyCountText = index.Q<Label>("CopiedKeyframeLabel");
            keyframeList = index.Q<ListView>("CopiedKeyframeList");

            var copyButton = index.Q<Button>("CopyButton");
            copyButton.clicked += () => UniEaseCopy.CopyKeyframes(onCopied: UpdateCopiedKeyInfo, onLogged: OnCopyLogged);

            pasteEaseButton = index.Q<Button>("PasteEaseButton");
            pasteEaseButton.clicked += () => UniEaseCopy.PasteEase(UpdateLog);

            pasteValueButton = index.Q<Button>("PasteValueButton");
            pasteValueButton.clicked += () => UniEaseCopy.PasteValue(UpdateLog);

            var node = LoadTreeAsset("KeyframeNode.uxml");
#if UNITY_2020_1_OR_NEWER
            keyframeList.makeItem = () => node.CloneTree();
            keyframeList.bindItem = (element, i) =>
            {
                element.Q<Label>().text = $"[{i}] {((Keyframe)keyframeList.itemsSource[i]).Dump()}";
            };
#else
            keyframeList.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
#endif

            UpdateCopiedKeyInfo(null);
            UpdateLog(new UniEaseCopyLog() { Message = "initialized.", Type = LogType.Log });
        }

        private void OnCopyLogged(UniEaseCopyLog log)
        {
            UpdateLog(log);
            pasteEaseButton.SetEnabled(log.Type != LogType.Error);
            pasteValueButton.SetEnabled(log.Type != LogType.Error);
        }

        private void UpdateCopiedKeyInfo(Keyframe[] keyframes)
        {
            keyCountText.text = $"Copied Keyframes: {keyframes?.Length ?? 0}";
            keyframeList.itemsSource = keyframes;
#if UNITY_2021_2_OR_NEWER
            keyframeList.Rebuild();
#else
            keyframeList.Refresh();
#endif
        }


        private void UpdateLog(UniEaseCopyLog log)
        {
#if UNITY_2021_2_OR_NEWER
            logText.text = $"<b>[{log.Type}]</b> {log.Message}";
#else
            logText.text = $"[{log.Type}] {log.Message}";
#endif
            SetLogClass(logTypeLabel, log.Type);
        }

        private static void RemoveAllLogClass(VisualElement element)
        {
            element.RemoveFromClassList($"LogType{LogType.Log}");
            element.RemoveFromClassList($"LogType{LogType.Success}");
            element.RemoveFromClassList($"LogType{LogType.Warning}");
            element.RemoveFromClassList($"LogType{LogType.Error}");
        }

        private static void SetLogClass(VisualElement element, LogType logType)
        {
            RemoveAllLogClass(element);
            element.AddToClassList($"LogType{logType.ToString()}");
        }
    }
}
