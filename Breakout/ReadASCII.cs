using System.IO;
using System;

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
            for (int i = 2; i < GetMapIndex(); i++) {
                mapData.Add(levelContents[i]);
            }
            organizedData.Add("Map", mapData);

            List<string> metaData = new List<string>();
            for (int i = GetMapIndex() + 2; i < GetMetaIndex(); i++) {
                metaData.Add(levelContents[i]);
            }
            organizedData.Add("Meta", metaData);

            List<string> legendData = new List<string>();
            for (int i = GetMetaIndex() + 2; i < GetLegendIndex(); i++) {
                legendData.Add(levelContents[i]);
            }
            organizedData.Add("Legend", legendData);

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

        public IDictionary<string, (float, float)> ReadMap() {
            IDictionary<string, (float, float)> mapOrganized = 
                                            new Dictionary<string, (float, float)>();
            List<string> map = OrganizingData()["Map"];
            
            map.ForEach(delegate(string data) {
                foreach (char c in data) {
                    if (ReadLegend().ContainsKey(c)) {
                        mapOrganized.Add(ReadLegend()[c], (
                            (float)map.IndexOf(data) / 12.0f, (float)data.IndexOf(c) / 24.0f
                            ));
                    } else {
                        mapOrganized.Add("Empty", (
                            (float)map.IndexOf(data) / 12.0f, (float)data.IndexOf(c) / 24.0f
                        ));
                    }
                }
            });
            
            return mapOrganized;
        }
    }
}

// X in 12 segments
// Y in 24 segments