using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ExportServer
{
    [Serializable]
    public class GridGraphServerData
    {
        [NonSerialized]
        private GridGraph graph;
        public GridGraphServerData(GridGraph graph) 
        {
            this.graph = graph;
            Rotation = new Vector2(graph.rotation.x, graph.rotation.z);
            Center = new Vector2(MathF.Round(graph.center.x), MathF.Round(graph.center.z));//四舍五入，规范设置都是整形，但是数据有时会.9999999所以这么处理
            UnclampedSize = graph.unclampedSize;
            NodeSize = graph.nodeSize;
            Name = graph.name;
            NodeDatas = new List<GridNodeServerData>();
            graph.GetNodes((node) =>
            {
                GridNodeServerData data = new GridNodeServerData();
                var gridNode = node as GridNode;
                data.Index = gridNode.NodeInGridIndex;
                data.Position = new Vector2(gridNode.position.x, gridNode.position.z);
                data.Flags = gridNode.Flags;
                data.FlagParse();
                NodeDatas.Add(data);
                Debug.Log(data.ToString());

            });
        }

        public string Name;
        public Vector2 Center;
        public Vector2 Rotation;
        public Vector2 UnclampedSize;
        public float NodeSize;
        public List<GridNodeServerData> NodeDatas;
        

    }
    [Serializable]
    public class GridNodeServerData
    {
        public int Index;
        public Vector2 Position;
        public uint Flags;
        public uint Walkable;
        public uint HNodeIndex;
        public uint IsDirty;
        public uint Tag;
        public uint GraphIndex;

        public void FlagParse()
        {
            Walkable = Flags & (1 << 0);
            HNodeIndex = 0;
            for (int i = 1; i <= 17; i++)
            {
                uint val = ((uint)1) << i;
                var data = Flags & (((uint)1) << i);
                if (data == 0)
                {
                    HNodeIndex = HNodeIndex & ~val;
                }
                else
                {
                    HNodeIndex = HNodeIndex | val;
                }
            }
            IsDirty = Flags & (1 << 18);
            Tag = 0;
            for (int i = 19; i <= 23; i++)
            {
                uint val = ((uint)1) << i;
                var data = Flags & (((uint)1) << i);
                if (data == 0)
                {
                    Tag = Tag & ~val;
                }
                else
                {
                    Tag = Tag | val;
                }
            }
            GraphIndex = 0;
            for (int i = 24; i <= 31; i++)
            {
                uint val = ((uint)1) << i;
                var data = Flags & (((uint)1) << i);
                if (data == 0)
                {
                    GraphIndex = GraphIndex & ~val;
                }
                else
                {
                    GraphIndex = GraphIndex | val;
                }
            }
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
