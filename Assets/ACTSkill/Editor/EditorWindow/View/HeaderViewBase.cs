using UnityEditor;
using UnityEngine;

namespace ACTSkillEditor
{
    public abstract class HeaderViewBase : ViewBase
    {
        public abstract string Title { get; set; }

        public HeaderViewBase(ACTSkillEditorWindow owner) : base(owner) { }

        protected virtual void OnHeaderDraw(float headerHeight)
        {
        }
        
        public override void Draw(Rect rect)
        {
            float headerHeight = DrawHeader(rect);
            
            Rect contentRect = rect;
            contentRect.yMin += headerHeight;
            
            GUI.Box(contentRect, GUIContent.none, GUIStyleHelper.ViewBg);
            OnGUI(contentRect);
        }

        protected virtual float DrawHeader(Rect rect)
        {
            float headerHeight = EditorGUIUtility.singleLineHeight + 3;
            Rect headerRect = new Rect(rect.x, rect.y, rect.width, headerHeight);
            GUILayout.BeginArea(headerRect, EditorStyles.toolbar);
            GUILayout.BeginHorizontal();

            if (!string.IsNullOrEmpty(Title))
                GUILayout.Label(Title, GUIStyleHelper.TitleMiddleCenter, GUILayout.ExpandWidth(true));
            OnHeaderDraw(headerHeight);

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            return headerHeight;
        }
    }
}
