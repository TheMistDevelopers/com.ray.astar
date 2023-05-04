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
        public GridGraphServerData(GridGraph graph,int mapId) 
        {
            this.graph = graph;
            lenX = graph.unclampedSize.x;
            lenZ = graph.unclampedSize.y;
            gridWidth = graph.nodeSize;
            Name = graph.name;
            NodeDatas = new List<GridNodeServerData>();
            this.mapId = mapId;
            graph.GetNodes((node) =>
            {
                GridNodeServerData data = new GridNodeServerData();
                var gridNode = node as GridNode;
                data.Index = gridNode.NodeInGridIndex;
                data.x = gridNode.position.x;
                data.z = gridNode.position.z;
                data.Flags = gridNode.Flags;
                data.FlagParse();
                NodeDatas.Add(data);
                Debug.Log(data.ToString());

            });
        }

        public string Name;
        public int mapId;
        public float lenX;
        public float lenZ;
        public float gridWidth;
        public List<GridNodeServerData> NodeDatas;
        

    }
    [Serializable]
    public class GridNodeServerData
    {
        public int Index;
        public float x;
        public float z; 
        public uint Flags;
        public uint walkable;
        public uint HNodeIndex;
        public uint IsDirty;
        public uint Tag;
        public uint GraphIndex;

        public void FlagParse()
        {
            walkable = Flags & (1 << 0);
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
