using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ElectronicLib
{
    public class DesignManager
    {
        private static List<IWindowBase> styles;
        public static void AddStyle(IWindowBase style)
        {
            if (styles == null)
                styles = new List<IWindowBase>();
            styles.Add(style);
        }

        public static void RefreshStyle(DesignManager design)
        {
            if (styles == null)
                return;

            foreach (var item in styles)
            {
                item.SetStyle(design);
            }
        }

        public static DesignManager current { get; set; }
        Dictionary<string, BitmapSource> dictionary;
        Dictionary<BitmapSource, ImageBrush> bitmapCache;
        Dictionary<string, BitmapImage> bitmapStream;

        public string Name { get; set; }
        public string Author { get; set; }
        public Color borderColor { get; set; }

        public DesignManager(string designFile)
        {
            current = this;
            dictionary = new Dictionary<string, BitmapSource>();
            bitmapCache = new Dictionary<BitmapSource, ImageBrush>();
            LoadXmlData(designFile);
        }

        private void LoadXmlData(string designFile)
        {

            BitmapImage getBitmap(string absolutePath)
            {
                if (bitmapStream == null)
                    bitmapStream = new Dictionary<string, BitmapImage>();


                //string p = Path.GetTempFileName();

                //File.WriteAllBytes(p, (f.stream as MemoryStream).ToArray());

                BitmapImage img = null;
                absolutePath = absolutePath.ToLower();
                if (!bitmapStream.ContainsKey(absolutePath))
                {
                    string streamPath = DataBase.DataBaseDirFullPath + "\\" + absolutePath;
                    var f = DataBase.GetFile(streamPath);

                    img = new BitmapImage();

                    img.BeginInit();
                    img.StreamSource = f.stream;
                    img.EndInit();

                    bitmapStream.Add(absolutePath, img);
                }
                else
                {
                    img = bitmapStream[absolutePath];
                }
                return img; /*new BitmapImage(new Uri("file://" + p));*/
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(designFile);

            XmlElement root = doc["design"];

            XmlElement source = root["source"];
            string path = Path.GetDirectoryName(designFile);
            BitmapImage map = getBitmap(source.GetAttribute("src"));
            map.CacheOption = BitmapCacheOption.OnLoad;
            XmlElement info = root["info"];
            this.Name = info.GetAttribute("name");
            this.Author = info.GetAttribute("author");

            XmlElement rects = root["rects"];
            for (int i = 0; i < rects.ChildNodes.Count; i++)
            {
                XmlNode node = rects.ChildNodes[i];

                if (node is XmlComment)
                    continue;

                XmlElement r_data = (XmlElement)node;
                string item_id = r_data.Name;

                Int32Rect item_rect = Int32Rect.Empty;

                BitmapImage dest = map;
                bool mapLoaded = false;
                if (r_data.HasAttribute("src"))
                {
                    dest = getBitmap(r_data.GetAttribute("src"));
                    mapLoaded = true;
                }

                if (r_data.HasAttribute("rect"))
                {
                    string value = r_data.GetAttribute("rect").Trim();
                    string[] vls = value.Split(',');
                    //Преобразование
                    item_rect = new Int32Rect(
                        int.Parse(vls[0]),
                        int.Parse(vls[1]),
                        int.Parse(vls[2]),
                        int.Parse(vls[3]));
                }
                else
                {
                    item_rect = new Int32Rect(0, 0, dest.PixelWidth, dest.PixelHeight);
                }


                CroppedBitmap cropped = new CroppedBitmap(dest, item_rect);
                BitmapSource bmp = cropped;

                dictionary.Add(item_id, bmp);

                if (mapLoaded)
                {
                    //dest.Relo
                }
            }

            XmlElement form = root["form"];
            string[] f_rgb = form.GetAttribute("borderColorRGB").Trim().Split(',');
            this.borderColor = Color.FromArgb(255, byte.Parse(f_rgb[0]), byte.Parse(f_rgb[1]), byte.Parse(f_rgb[2]));

            //  map.Dispose();
        }

        public bool HasId(string id)
        {
            return dictionary.ContainsKey(id);
        }

        public ImageBrush GetImageBrushFromId(string id)
        {
            BitmapSource src = GetBitmapSourceFromId(id);
            if (src == null)
                return null;

            ImageBrush _brsh;
            if (bitmapCache.TryGetValue(src, out _brsh))
            {
                return _brsh;
            }

            bitmapCache.Add(src, _brsh = new ImageBrush(src));

            return (ImageBrush)_brsh;

        }

        public BitmapSource GetBitmapSourceFromId(string id)
        {
            if (!HasId(id))
                return null;
            return dictionary[id];
        }
    }
}
