using System.IO;
using System;
using System.Linq;

namespace Breakout {
    public class InterpretData {
        private readonly string[] levelContents;
        readonly IDictionary<string, List<string>> organizedData;
        readonly IDictionary<char, string> legendOrganized;
        readonly IDictionary<string, List<(float, float)>> mapOrganized;
        readonly IDictionary<string, string> metaOrganized;


        public InterpretData(string file) {
            try {
                levelContents = File.ReadAllLines(file);
            } catch {
                Console.WriteLine("Could not find file");
                levelContents = Array.Empty<string>();
            }
            organizedData = new Dictionary<string, List<string>>();
            legendOrganized = new Dictionary<char, string>();
            mapOrganized = new Dictionary<string, List<(float, float)>>();
            metaOrganized = new Dictionary<string, string>();
            OrganizingData();
            ReadLegend();
            ReadMap();
            ReadMeta();
        }

        public void OrganizingData() {
            try {
                if (levelContents != null) {
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
                } else {
                    Console.WriteLine("levelContents is null");
                }
            } catch {
                Console.WriteLine("Error Organizing Data");
            }
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

        public void ReadLegend() {
            try {
                List<string> legend = organizedData["Legend"];
                
                legend.ForEach(delegate(string data) {
                    legendOrganized.TryAdd(data[0], data.Substring(3));
                });
            } catch {
                Console.WriteLine("Error Reading Legend");
            }
        }

        public void ReadMap() {
            try {
                List<string> map = organizedData["Map"];
                List<(float, float)> emptyPositions = new List<(float, float)>();
                for (int j = 0; j < map.Count; j++) {
                    for (int i = 0; i < map[i].Length; i++) {
                        if (legendOrganized.ContainsKey(map[j][i])) {
                            string legendKey = legendOrganized[map[j][i]];
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
            } catch {
                Console.WriteLine("Error Reading Map");
            }
        }

        public void ReadMeta() {
            try {
                List<string> meta = organizedData["Meta"];
                foreach (string data in meta) {
                    int whereToSplit = data.IndexOf(":");
                    metaOrganized.Add(data.Substring(0, whereToSplit),
                        data.Substring(whereToSplit + 1));
                }
            } catch {
                Console.WriteLine("Error Reading Meta");
            }
        }

        public IDictionary<char, string> GetLegendOrganized() {
            return legendOrganized;
        }

        public IDictionary<string, List<(float, float)>> GetMapOrganized() {
            return mapOrganized;
        }

        public IDictionary<string, string> GetMetaOrganized() {
            return metaOrganized;
        }
    }
}