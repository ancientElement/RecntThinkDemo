using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking.Types;

namespace A星寻路
{
    public enum NodeType
    {
        Nomal,
        Barrier,
        Start,
        Destination
    }

    public enum CaculateType
    {
        Manhattan,
        Euclid
    }

    [Serializable]
    public class BaseNode
    {
        public NodeType NodeType;

        public BaseNode preNode;

        public int x;
        public int y;

        public float g { get; private set; }
        public float h { get; private set; }
        public float f => h + g;

        public BaseNode(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Caculate(BaseNode startNode, BaseNode destinationNode, CaculateType caculateType = CaculateType.Manhattan)
        {
            switch (caculateType)
            {
                case CaculateType.Euclid:
                default:
                    if (preNode != null)
                    {
                        this.g = preNode.g + HeuristicEuclid(this, preNode);
                    }
                    else
                    {
                        this.g = HeuristicEuclid(this, startNode);
                    }
                    this.h = HeuristicEuclid(this, destinationNode);
                    break;
                case CaculateType.Manhattan:
                    if (preNode != null)
                    {
                        this.g = preNode.g + HeuristicManhattan(this, preNode);
                    }
                    else
                    {
                        this.g = HeuristicManhattan(this, startNode);
                    }
                    this.h = HeuristicManhattan(this, destinationNode);
                    break;
            }
        }

        public float HeuristicEuclid(BaseNode node1, BaseNode node2)
        {
            return Vector2.Distance(new Vector2(node1.x, node1.y), new Vector2(node2.x, node2.y));
        }

        public int HeuristicManhattan(BaseNode node1, BaseNode node2)
        {
            int dx = Mathf.Abs(node1.x - node2.x);
            int dy = Mathf.Abs(node1.y - node2.y);
            return dx + dy;
        }
    }
}