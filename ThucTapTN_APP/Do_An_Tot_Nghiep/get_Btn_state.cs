using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using BunifuColorTransition;
using Bunifu.Framework.Lib;
namespace Do_An_Tot_Nghiep
{
    class get_Btn_state
    {
        public void render_state(Label lab, string stt)
        {
            lab.Invoke((MethodInvoker)(() => lab.Text = stt));

        }


      
    }
}
