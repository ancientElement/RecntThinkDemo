using System.Net.Http.Headers;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace A星寻路
{

    public class GridView : MonoBehaviour
    {
        public Text H;
        public Text G;
        public Text F;
        public Image Image;

        public void UpdateView(BaseNode node)
        {
            H.text = node.h.ToString();
            G.text = node.g.ToString();
            F.text = node.f.ToString();
            switch (node.NodeType)
            {
                case NodeType.Nomal:
                    Image.color = Color.white;
                    break;
                case NodeType.Barrier:
                    Image.color = Color.red;
                    break;
                case NodeType.Start:
                    Image.color = Color.blue;
                    break;
                case NodeType.Destination:
                    Image.color = Color.yellow;
                    break;
            }
        }

        public void UpdateView(Color color)
        {
            Image.color = color;
        }

        public void UpdateColor(BaseNode node)
        {
            H.text = "";
            G.text = "";
            F.text = "";
            switch (node.NodeType)
            {
                case NodeType.Nomal:
                    Image.color = Color.white;
                    break;
                case NodeType.Barrier:
                    Image.color = Color.red;
                    break;
                case NodeType.Start:
                    Image.color = Color.blue;
                    break;
                case NodeType.Destination:
                    Image.color = Color.yellow;
                    break;
            }
        }
    }
}