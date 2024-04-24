using System.IO;
using System;
using System.Linq;

namespace Breakout {
    public class InterpretData {
        string[] levelContents;
        IDictionary<string, List<string>> organizedData;

        public InterpretData(string file) {
            levelContents = File.ReadAllLines(file);
            organizedData = new Dictionary<string, List<string>>();
        }

        public IDictionary<string, List<string>> OrganizingData() {
            List<string> mapData = new List<string>();
            for (int i = 1; i < GetMapIndex(); i++) {
                mapData.Add(levelContents[i]);
            }
            organizedData.TryAdd("Map", mapData);

            List<string> metaData = new List<string>();
            for (int i = GetMapIndex() + 2; i < GetMetaIndex(); i++) {
                metaData.Add(levelContents[i]);
            }
            organizedData.TryAdd("Meta", metaData);

            List<string> legendData = new List<string>();
            for (int i = GetMetaIndex() + 2; i < GetLegendIndex(); i++) {
                legendData.Add(levelContents[i]);
            }
            organizedData.TryAdd("Legend", legendData);

            return organizedData;
        }

        private int GetMapIndex() {
            return Array.IndexOf(levelContents, "Map/");
        }

        private int GetMetaIndex() {
            return Array.IndexOf(levelContents, "Meta/");
        }

        private int GetLegendIndex() {
            return Array.IndexOf(levelContents, "Legend/");
        }

        public IDictionary<char, string> ReadLegend() {
            IDictionary<char, string> legendOrganized = new Dictionary<char, string>();
            List<string> legend = OrganizingData()["Legend"];
            
            legend.ForEach(delegate(string data) {
                legendOrganized.Add(data[0], data.Substring(3));
            });
            
            return legendOrganized;
        }

        public IDictionary<string, List<(float, float)>> ReadMap() {
            IDictionary<string, List<(float, float)>> mapOrganized = 
                                            new Dictionary<string, List<(float, float)>>();
            List<string> map = OrganizingData()["Map"];
            List<(float, float)> emptyPositions = new List<(float, float)>();
            for (int j = 0; j < map.Count; j++) {
                for (int i = 0; i < map[i].Length; i++) {
                    // Console.WriteLine(map[j]);
                    if (ReadLegend().ContainsKey(map[j][i])) {
                        string legendKey = ReadLegend()[map[j][i]];
                        if (!mapOrganized.ContainsKey(legendKey)) {
                            mapOrganized[legendKey] = new List<(float, float)>();
                        } 

                        mapOrganized[legendKey].Add(((float)i / 12.0f, 0.95f - ((float)j / 24.0f)));
                    } else {
                        emptyPositions.Add(((float)i / 12.0f, 0.95f - ((float)j / 24.0f)));
                    }
                }
            }

            mapOrganized["Empty"] = emptyPositions;
            return mapOrganized;
        }
    }
}

// X in 12 segments
// Y in 24 segments