using Pathfinding;
using Pathfinding.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.IO;

namespace ExportServer
{
    [RequireComponent(typeof(AstarPath))]

    public class GridGraphExport : MonoBehaviour
    {
        [LabelText("Export Grid Graph")]
        AstarPath astarPath;
        List<GridGraphServerData> gridGraphServerDatas = new List<GridGraphServerData>();

        [Button("InitData")]
        private void InitData()
        {
            astarPath = gameObject.GetComponent<AstarPath>();
        }

        [Button("CreateDataCache")]
        [BoxGroup("Json")]
        private void CreateDataCache()
        {
            gridGraphServerDatas.Clear();
            foreach (var graph in astarPath.graphs)
            {
                if (graph is GridGraph)
                {
                    var gridGraph = (GridGraph)graph;
                    gridGraphServerDatas.Add(new GridGraphServerData(gridGraph,MapID));
                }
            }
        }
        [Title("MapID")]
        [BoxGroup("Json")]
        public int MapID;

        [Button("ExportJson")]
        [BoxGroup("Json")]
        private void ExportJson()
        {
            foreach (var data in gridGraphServerDatas)
            {
                string dataStr = JsonUtility.ToJson(data);
                string path = Application.dataPath + "/ExportJson/" + data.Name+".json";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                File.WriteAllText(path, dataStr);
            }
        }

    }
}


