using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Runtime.CompilerServices;
using ShootingGame;

using System.Text.Json;

namespace ShootingGame
{
    public class Save
    {
        public static string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string gameName = "MyPlatDemo";
        public static string baseFolder = localAppDataPath + "\\"+gameName+"";
        public static string MapbaseFolder = localAppDataPath + "\\" + gameName + "\\Json\\SavedGames";

        public Game1 game;

        public Save(Game1 game)
        { 
            this.game = game;
            CreateBaseFolders();
        }
        //public string backupFolder, backupPath;


        public void CreateBaseFolders()
        {
            CreateFolder(localAppDataPath + "\\" + gameName + "");
            CreateFolder(localAppDataPath + "\\" + gameName + "\\Json");
            CreateFolder(MapbaseFolder);
        }

        public void CreateFolder(string s)
        {
            DirectoryInfo CreateSiteDirectory = new DirectoryInfo(s);
            if (!CreateSiteDirectory.Exists)
            {
                CreateSiteDirectory.Create();
            }
        }

        public bool CheckIfFileExists(string PATH)
        {
            bool fileExists = File.Exists(localAppDataPath + "\\" + gameName + "\\" + PATH);
            return fileExists;
            //return true;
        }

        public void DeleteFile(string PATH)
        {
  
            File.Delete(PATH);
        }
        public void DeleteMapFile(string PATH)
        {
            PATH = MapbaseFolder + PATH;

            File.Delete(PATH);
        }


        public void SaveMapData(Map map,string filename)
        {
            filename = MapbaseFolder + filename;

            string json = JsonSerializer.Serialize(map, new JsonSerializerOptions()
            {
                IncludeFields = true
            });


            File.WriteAllText(filename, json);
        }
        public Map LoadMapData(string filepath)
        {
            filepath = MapbaseFolder + filepath;


            if (!File.Exists(filepath))
            {
                return null;
            }
            var jsonOptions = new JsonSerializerOptions()
            {
                IncludeFields = true
            };



            string json = File.ReadAllText(filepath);
            Map map = JsonSerializer.Deserialize<Map>(json,jsonOptions);

            return map;
        }




     





    }
}





//#region Converting to Binary and back

//public static string StringToBinary(string data)
//{
//    StringBuilder sb = new StringBuilder();

//    foreach (char c in data.ToCharArray())
//    {
//        sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
//    }
//    return sb.ToString();
//}

//public static string BinaryToString(string data)
//{
//    List<Byte> byteList = new List<Byte>();

//    for (int i = 0; i < data.Length; i += 8)
//    {
//        byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
//    }

//    return Encoding.ASCII.GetString(byteList.ToArray());
//}
//#endregion

//public virtual void HandleSaveFormates(XDocument xml)
//{

//    byte[] compress = Encoding.ASCII.GetBytes(StringToBinary(xml.ToString()));
//    File.WriteAllBytes(Globals.appDataFilePath + "\\" + gameName + "\\XML\\SavedGames\\" + Convert.ToString(gameId, Globals.culture), compress);



//}
