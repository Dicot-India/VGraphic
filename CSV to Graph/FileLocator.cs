using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSV_Graph
{
    internal class FileLocator
    {
        public static string mainLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sansel_Database");
    }

    [Serializable]
    public class FileDataList
    {
        public int fileNumber { get; set; } = 0;
        public List<fileInfo> fileList { get; set; } = new List<fileInfo>();

        public FileDataList(int deviceID)
        {
            string folder = Path.Combine(FileLocator.mainLocation, deviceID.ToString());
            if (!Directory.Exists(Path.Combine(FileLocator.mainLocation, deviceID.ToString())))
            {
                Directory.CreateDirectory(folder);
            }

            string fileCollectionInfo = Path.Combine(folder, "Collection.json");


            if (!File.Exists(fileCollectionInfo))
            {
                using (File.Create(fileCollectionInfo)) { };
                this.fileNumber = 0;
                this.fileList = new List<fileInfo>();
            }
            else
            {
                FileDataLoad(fileCollectionInfo);
            }
        }

        public FileDataList() { }

        public void FileDataLoad(string Location)
        {
            using (StreamReader sr =  new StreamReader(Location))
            {
                string line = sr.ReadLine();
                if (line != null)
                {
                    var information = JsonConvert.DeserializeObject<FileDataList>(line);
                    this.fileList = information.fileList;
                    this.fileNumber = information.fileNumber;
                }
            }
        }

        public void SaveFileInfo(int deviceID)
        {
            string folder = Path.Combine(FileLocator.mainLocation, deviceID.ToString());
            string fileCollectionInfo = Path.Combine(folder, "Collection.json");
            string Data = JsonConvert.SerializeObject(this);
            using (StreamWriter sw = new StreamWriter(fileCollectionInfo, false))
            {
                sw.WriteLine(Data);
            }
        }

        public void addInfo(DateTime start, DateTime end, List<string> Channels, string FileName)
        {
            if (!fileList.Any(fileinfo => fileinfo.fileName == FileName))
            {
                fileInfo info = new fileInfo
                {
                    startTimeStamp = start.Ticks,
                    endTimeStamp = end.Ticks,
                    channelList = Channels,
                    channels = Channels.Count,
                    fileName = FileName
                };

                this.fileList.Add(info);
                fileNumber++;
            }
        }   

    }

    [Serializable]
    public struct fileInfo {
        public long startTimeStamp;
        public long endTimeStamp;
        public int channels;
        public List<string> channelList;
        public string fileName;
    };
}
