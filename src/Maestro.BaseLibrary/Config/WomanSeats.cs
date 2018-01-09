using System.Configuration;

namespace Maestro.BaseLibrary.Config
{
    public class WomanSeats : ConfigurationElementCollection
    {
        public WomanSeat this[int index]
        {
            get { return (WomanSeat)base.BaseGet(index); }
            set
            {
                if ((((base.BaseGet(index)) != null)))
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new WomanSeat();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WomanSeat)element).PartyCode;
        }

        public WomanSeat Add(string partyCode, string seatCount)
        {
            WomanSeat newWomanSeat = new WomanSeat();
            newWomanSeat.PartyCode = partyCode;
            newWomanSeat.SeatCount = seatCount;
            this.BaseAdd(newWomanSeat);
            return newWomanSeat;
        }
    }
}
