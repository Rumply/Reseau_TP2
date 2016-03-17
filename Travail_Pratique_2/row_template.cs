using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Travail_Pratique_2
{
    class row_template
    {
        public TextBlock prefix { get; set; }

        public String message { get; set; }

        public row_template(String a_prefix,String a_message)
        {
            prefix = new TextBlock();
            prefix.Text = a_prefix;
            message = a_message;
        }
    }
}
