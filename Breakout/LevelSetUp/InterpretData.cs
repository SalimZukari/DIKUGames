using System;

namespace Breakout; 
public class InterpretData {
    /// <summary>
    /// Reads from an ASCII map and organizes the provided data
    /// </summary>
    private readonly string[] levelContents;
    readonly IDictionary<string, List<string>> organizedData;
    readonly IDictionary<char, string> legendOrganized;
    readonly IDictionary<string, List<(float, float)>> mapOrganized;
    readonly IDictionary<string, string> metaOrganized;        


    public InterpretData(string file) {
        try {
            levelContents = File.ReadAllLines(file);
        } catch {
            Console.WriteLine("Could not find file at location: {0}", file);
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

    /// <summary>
    /// Puts all data from the ASCII map and .txt file
    /// into an easy to navigate dictionary.
    /// </summary>
    public void OrganizingData() {
        try {
            if (levelContents != null) {
                List<string> mapData = new List<string>();
                for (int i = 1; i < GetMapIndex(); i++) {
                    mapData.Add(levelContents[i]);
                }
                organizedData.TryAdd("Map", mapData);

                List<string> metaData = new List<string>();
                for (int i = GetMetaStartIndex() + 1; i < GetMetaEndIndex(); i++) {
                    metaData.Add(levelContents[i]);
                }
                organizedData.TryAdd("Meta", metaData);

                List<string> legendData = new List<string>();
                for (int i = GetLegendStartIndex() + 1; i < GetLegendEndIndex(); i++) {
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

    private int GetMetaStartIndex() {
        return Array.IndexOf(levelContents, "Meta:");
    }

    private int GetMetaEndIndex() {
        return Array.IndexOf(levelContents, "Meta/");
    }

    private int GetLegendStartIndex() {
        return Array.IndexOf(levelContents, "Legend:");
    }

    private int GetLegendEndIndex() {
        return Array.IndexOf(levelContents, "Legend/");
    }

    /// <summary>
    /// Puts the legend data into it's own dictionary.
    /// </summary>
    public void ReadLegend() {
        try {
            List<string> legend = organizedData["Legend"];
            
            legend.ForEach(delegate(string data) {
                legendOrganized.TryAdd(data[0], data.Substring(3));
            });
        } catch {
            Console.WriteLine("Error Readng Legend");
        }
    }

    /// <summary>
    /// Puts the map data into it's own dictionary.
    /// </summary>
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

    /// <summary>
    /// Puts the meta data into it's own dictionary.
    /// </summary>
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

    public IDictionary<string, List<string>> GetOrganizedData() {
        return organizedData;
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

    public string[] GetLevelContents() {
        return levelContents;
    }
}