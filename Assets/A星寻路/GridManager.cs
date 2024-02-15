using System.Collections.Generic;
using UnityEngine;

namespace A星寻路
{
    public class GridManager : MonoBehaviour
    {
        private Map map;
        public Transform father;
        public Vector2Int Rect;
        public Vector2Int Grids;
        private List<GridView> gridViewList;
        public CaculateType caculateType;
        public bool diagonal;

        private void Start()
        {
            map = new Map(Grids.x, Grids.y);
            gridViewList = new List<GridView>();

            GameObject gridPrefab = Resources.Load<GameObject>("Grid");
            Vector2 offset = new Vector2(Rect.x * 0.5f, Rect.y * 0.5f);

            foreach (BaseNode item in map.Nodes)
            {
                GameObject gridGO = GameObject.Instantiate(gridPrefab);
                gridGO.transform.SetParent(father, false);
                Vector3 pos = new Vector3(item.x * Rect.x + offset.x, item.y * Rect.y + offset.y);
                gridGO.transform.SetPositionAndRotation(pos, Quaternion.identity);
                gridGO.GetComponent<GridContorller>().Init(item, this);
                gridViewList.Add(gridGO.GetComponent<GridView>());
            }
        }
        #region Test
        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    List<BaseNode> nodes = map.Neigbors(map.GetDestination());
            //    if (nodes == null)
            //    {
            //        return;
            //    }
            //    for (int i = 0; i < nodes.Count; i++)
            //    {
            //        SetBarrierNode(nodes[i]);
            //    }
            //}
            if ((Input.GetKeyDown(KeyCode.W)))
            {
                for (int i = 0; i < 50; i++)
                {
                    int random = Random.Range(0, map.Nodes.Count);
                    if (map.Nodes[random].NodeType == NodeType.Barrier)
                    {
                        return;
                    }
                    SetBarrierNode(map.Nodes[random]);
                }
            }

            if ((Input.GetKeyDown(KeyCode.E)))
            {
                for (int i = 0; i < map.Nodes.Count; i++)
                {
                    BaseNode item = map.Nodes[i];
                    if (item.NodeType == NodeType.Destination ||
                        item.NodeType == NodeType.Start
                       )
                    {
                        continue;
                    }
                    gridViewList[i].UpdateColor(item);
                }

                List<BaseNode> list = map.FindPath((item) =>
                {
                    if (item.NodeType == NodeType.Destination ||
                        item.NodeType == NodeType.Start
                       )
                    {
                        return;
                    }
                    int index = GetNode(item);
                    gridViewList[index].UpdateView(item);
                    gridViewList[index].UpdateView(Color.cyan);
                },caculateType,diagonal);

                if(list == null)
                {
                    return;
                }

                for (int i = 0; i < list.Count; i++)
                {
                    BaseNode item = list[i];
                    if (item == map.GetStart())
                    {
                        continue;
                    }
                    int index = GetNode(item);
                    gridViewList[index].UpdateView(item);
                    gridViewList[index].UpdateView(Color.black);
                }
            }
        }
        #endregion
        public int GetNode(BaseNode baseNode)
        {
            int index = baseNode.y + baseNode.x * map.heightCount;
            return index;
        }

        internal void SetStartNode(BaseNode baseNode)
        {
            BaseNode preNode = map.SetStartNode(baseNode);
            if (preNode != null)
            {
                int index1 = GetNode(preNode);
                gridViewList[index1].UpdateColor(preNode);
            }
            int index2 = GetNode(baseNode);
            gridViewList[index2].UpdateColor(baseNode);
        }

        internal void SetDestinationNode(BaseNode baseNode)
        {
            BaseNode preNode = map.SetDestinationNode(baseNode);
            if (preNode != null)
            {
                int index1 = GetNode(preNode);
                gridViewList[index1].UpdateColor(preNode);
            }
            int index2 = GetNode(baseNode);
            gridViewList[index2].UpdateColor(baseNode);
        }

        internal void SetBarrierNode(BaseNode baseNode)
        {
            map.SetBarrierNode(baseNode);
            gridViewList[GetNode(baseNode)].UpdateColor(baseNode);
        }
    }
}
