using System;
using System.Xml.Serialization;
using Common.Extensions;

namespace Common.Configuration.Menu
{
    [Serializable]
    [XmlRoot(ElementName = "MenuConfiguration")]
    public class MenuConfiguration
    {
        [XmlArray(ElementName = "menus", IsNullable = false)]
        [XmlArrayItem(ElementName = "menu", IsNullable = false)]
        public Menu[] Menus { get; set; }

        public static MenuConfiguration Load(string fileName)
        {
            return fileName.LoadFromXml<MenuConfiguration>();
        }
    }
}
