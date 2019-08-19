using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookWave.Desktop.Util
{
    public interface XMLSaveObject
    {

        XElement ToXML();

        void FromXML(XElement xmlElement);

    }
}
