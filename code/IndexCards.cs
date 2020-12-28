using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace spr_index_cards
{
    class IndexCard
    {
        public uint number;
        public string front, back;
    }

    class Lektion
    {
        public uint number;
        public List<IndexCard> cards;

        public Lektion ()
        {
            cards = new List<IndexCard>();
        }

        public static List<Lektion> LoadLektionenFromXML()
        {
            List<Lektion> result = new List<Lektion>();

            XElement heisig = XElement.Load(@"test.xml");
            List<XElement> lektionen = heisig.Elements("lektion").ToList();
            foreach(XElement l in lektionen)
            {
                Lektion lek = new Lektion();
                lek.number = uint.Parse(l.Attribute("number").Value);
                List<XElement> karten = l.Elements("card").ToList();
                foreach(XElement k in karten)
                {
                    IndexCard icard = new IndexCard();
                    icard.number = uint.Parse(k.Attribute("number").Value);
                    icard.front = k.Attribute("front").Value;
                    icard.back = k.Attribute("back").Value;
                    lek.cards.Add(icard);
                }
                result.Add(lek);
            }

            return result;
        }
    }
}
