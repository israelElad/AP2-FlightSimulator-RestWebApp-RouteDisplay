using System.Xml;

namespace Ex3.Models
{
    public class DisplayFlight
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
        public double Rudder { get; set; }
        public double Throttle { get; set; }
        public string EOF { get; set; }


        public void ToXml(XmlWriter writer)
        {

            writer.WriteStartElement("Display");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteElementString("Rudder", this.Rudder.ToString());
            writer.WriteElementString("Throttle", this.Throttle.ToString());
            writer.WriteElementString("EOF", this.EOF);
            writer.WriteEndElement();
        }
    }
}