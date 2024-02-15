using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace A星寻路
{
    public class GridContorller : MonoBehaviour
    {
        [SerializeField] private GridView m_gridView;
        private BaseNode m_baseNode;
        private GridManager m_manager;

        public void Init(BaseNode baseNode, GridManager manager)
        {
            m_baseNode = baseNode;
            m_manager = manager;
        }

        public void OnMouseDown(BaseEventData baseEventData)
        {
            PointerEventData pointerEventData = (PointerEventData)baseEventData;
            if (pointerEventData != null)
            {
                switch (pointerEventData.button)
                {
                    case PointerEventData.InputButton.Left:
                        m_manager.SetStartNode(m_baseNode);
                        break;
                    case PointerEventData.InputButton.Right:
                        m_manager.SetDestinationNode(m_baseNode);
                        break;
                    case PointerEventData.InputButton.Middle:
                        m_manager.SetBarrierNode(m_baseNode);
                        break;
                }
            }
        }
    }
}