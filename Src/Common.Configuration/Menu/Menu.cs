using System;
using System.Xml.Serialization;

namespace Common.Configuration.Menu
{
    [Serializable]
    public class Menu
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlArray(ElementName = "items", IsNullable = false)]
        [XmlArrayItem(ElementName = "item", IsNullable = false)]
        public MenuItem[] Items { get; set; }
    }
}
