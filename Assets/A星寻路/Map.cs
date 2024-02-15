using System;
using System.Collections.Generic;
using System.Linq;

namespace A星寻路
{
    [Serializable]
    public class Map
    {
        public List<BaseNode> Nodes;

        public int widthCount;
        public int heightCount;

        public Map(int x, int y)
        {
            this.widthCount = x;
            this.heightCount = y;
            Nodes = new List<BaseNode>();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Nodes.Add(new BaseNode(i, j));
                }
            }
        }

        public BaseNode GetNode(int x, int y)
        {
            int index = y + x * this.heightCount;
            return Nodes[index];
        }

        public List<BaseNode> Neigbors(BaseNode baseGrid,bool diagonal = true)
        {
            if (baseGrid == null)
            {
                return null;
            }
            List<BaseNode> nigbors = new List<BaseNode>();
            int up = baseGrid.y + 1;
            int down = baseGrid.y - 1;
            int left = baseGrid.x - 1;
            int right = baseGrid.x + 1;
            //左
            if (left >= 0)
            {
                BaseNode leftNode = GetNode(left, baseGrid.y);
                nigbors.Add(leftNode);
            }
            //左上
            if (left >= 0 && up < heightCount && diagonal)
            {
                BaseNode leftUpNode = GetNode(left, up);
                nigbors.Add(leftUpNode);
            }
            //左下
            if (left >= 0 && down >= 0 && diagonal)
            {
                BaseNode leftDownNode = GetNode(left, down);
                nigbors.Add(leftDownNode);
            }
            //上
            if (up < heightCount)
            {
                BaseNode upNode = GetNode(baseGrid.x, up);
                nigbors.Add(upNode);
            }
            //右上
            if (right < widthCount && up < heightCount && diagonal)
            {
                BaseNode rightUpNode = GetNode(right, up);
                nigbors.Add(rightUpNode);
            }
            //右
            if (right < widthCount)
            {
                BaseNode rightNode = GetNode(right, baseGrid.y);
                nigbors.Add(rightNode);
            }
            //右下
            if (right < widthCount && down >= 0 && diagonal)
            {
                BaseNode rightDownNode = GetNode(right, down);
                nigbors.Add(rightDownNode);
            }
            //下
            if (down >= 0)
            {
                BaseNode downNode = GetNode(baseGrid.x, down);
                nigbors.Add(downNode);
            }
            return nigbors;
        }

        public BaseNode SetStartNode(BaseNode baseGrid)
        {
            if (Nodes.Contains(baseGrid))
            {
                if (baseGrid.NodeType == NodeType.Destination ||
                     baseGrid.NodeType == NodeType.Start)
                {
                    return null;
                }
                BaseNode preNode = Nodes.Where(x => x.NodeType == NodeType.Start).FirstOrDefault();
                if (preNode != null)
                {
                    preNode.NodeType = NodeType.Nomal;
                    baseGrid.NodeType = NodeType.Start;
                    return preNode;
                }
                else
                {
                    baseGrid.NodeType = NodeType.Start;
                }
            }
            return null;
        }

        public BaseNode SetDestinationNode(BaseNode baseGrid)
        {
            if (Nodes.Contains(baseGrid))
            {
                if (baseGrid.NodeType == NodeType.Destination ||
                    baseGrid.NodeType == NodeType.Start)
                {
                    return null;
                }
                BaseNode preNode = Nodes.Where(x => x.NodeType == NodeType.Destination).FirstOrDefault();
                if (preNode != null)
                {
                    preNode.NodeType = NodeType.Nomal;
                    baseGrid.NodeType = NodeType.Destination;
                    return preNode;
                }
                else
                {
                    baseGrid.NodeType = NodeType.Destination;
                }
            }
            return null;
        }

        public BaseNode GetStart()
        {
            return Nodes.Where(x => x.NodeType == NodeType.Start).FirstOrDefault();
        }

        public BaseNode GetDestination()
        {
            return Nodes.Where(x => x.NodeType == NodeType.Destination).FirstOrDefault();
        }

        public List<BaseNode> FindPath(Action<BaseNode> callback,CaculateType caculateType = CaculateType.Manhattan, bool diagonal = true)
        {
            BaseNode startNode = GetStart();
            BaseNode destinationNode = GetDestination();

            //初始化open_set和close_set；
            List<BaseNode> openList = new List<BaseNode>() { startNode }; //将起点加入open_set中，并设置优先级为0（优先级最高）
            List<BaseNode> closeList = new List<BaseNode>();

            BaseNode current = null;
            BaseNodeFComparer baseNodeFComparer = new BaseNodeFComparer();
            BaseNodeHComparer baseNodeHComparer = new BaseNodeHComparer();

            if (startNode == null || destinationNode == null)
            {
                return null;
            }

            startNode.preNode = null;

            //如果open_set不为空
            while (openList.Any())
            {
                //则从open_set中选取优先级最高的节点n：
                if (openList.Count > 1)
                {
                    openList.Sort(baseNodeFComparer);
                    BaseNode nearLest = openList[0];
                    int count = 1;
                    while (nearLest.f == openList[count].f)
                    {
                        count++;
                        if (count >= openList.Count) break;
                    }
                    if (count > 1)
                    {
                        openList.Sort(0, count, baseNodeHComparer);
                    }
                }
                current = openList[0];

                //从终点开始逐步追踪parent节点，一直达到起点；
                //返回找到的结果路径，算法结束；
                if (current == destinationNode)
                {
                    List<BaseNode> path = new List<BaseNode>();
                    while (current.preNode != null)
                    {
                        path.Add(current.preNode);
                        current = current.preNode;
                    }
                    return path;
                }
                //如果节点n不是终点，则
                else
                {
                    //将节点n从open_set中删除，并加入close_set中；
                    openList.Remove(current);
                    closeList.Add(current);
                    //遍历节点n所有的邻近节点：
                    List<BaseNode> neigbors = Neigbors(current,diagonal);
                    foreach (BaseNode item in neigbors)
                    {
                        // 如果邻近节点m在close_set中，则：
                        if (closeList.Contains(item) ||
                            item.NodeType == NodeType.Barrier)
                        {
                            // 跳过，选取下一个邻近节点
                            continue;
                        }
                        // 如果邻近节点m也不在open_set中，则：
                        else if (!openList.Contains(item))
                        {
                            // 设置节点m的parent为节点n
                            item.preNode = current;
                            // 计算节点m的优先级
                            item.Caculate(startNode, destinationNode,caculateType);
                            // 将节点m加入open_set中
                            openList.Add(item);
                            callback.Invoke(item);
                        }
                    }
                }
            }
            return null;
        }

        public void SetBarrierNode(BaseNode baseGrid)
        {
            if (baseGrid.NodeType == NodeType.Destination ||
                baseGrid.NodeType == NodeType.Start)
            {
                return;
            }
            if (baseGrid.NodeType == NodeType.Barrier)
            {
                baseGrid.NodeType = NodeType.Nomal;
            }
            else
            {
                baseGrid.NodeType = NodeType.Barrier;
            }
        }
    }

    public class BaseNodeFComparer : IComparer<BaseNode>
    {
        public int Compare(BaseNode x, BaseNode y)
        {
            float res = x.f - y.f;
            if (res > 0)
            {
                return 1;
            }
            else if (res < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class BaseNodeHComparer : IComparer<BaseNode>
    {
        public int Compare(BaseNode x, BaseNode y)
        {
            float res = x.h - y.h;
            if (res > 0)
            {
                return 1;
            }
            else if (res < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}