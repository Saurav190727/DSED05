using System;
using System.Drawing;
using System.Windows.Forms;

namespace DSED05.Business
{
    public class Punter
    {
        public string name { get; set; }
        public int racer { get; set; }
        public Single cash { get; set; }
        public Single bet { get; set; }
        public Label labelWinner { get; set; }
        public Color myColor { get; set; }
    }
}
