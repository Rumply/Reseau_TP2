using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Net;


namespace Travail_Pratique_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool nouveau;
        private String username;
        private int name_minLenght;
        private int name_maxLenght;
        private IPAddress connect_ip;

        public MainWindow()
        {
            InitializeComponent();

            username = "";
            nouveau = true;
            name_minLenght = 3;
            name_maxLenght = 30;

            input_text.MaxLength = name_maxLenght;

            
            addHotKeys();
        }

        private void addHotKeys()
        {
            try
            {
                RoutedCommand commande1 = new RoutedCommand();
                commande1.InputGestures.Add(new KeyGesture(Key.Enter, ModifierKeys.Shift));
                CommandBindings.Add(new CommandBinding(commande1, newLine));

                RoutedCommand commande2 = new RoutedCommand();
                commande2.InputGestures.Add(new KeyGesture(Key.V, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(commande2, copyPaste));
            }
            catch (Exception)
            {
                
            }

        }

        private void copyPaste(object sender, RoutedEventArgs e)
        {
            if (sub_ip1.IsFocused && Clipboard.ContainsText())
            {
                string ip;
                ip = Clipboard.GetText();
                String[] ip_container = ip.Split('.');
                Console.WriteLine(sub_ip1);
                sub_ip1.Text = ip_container[0];
                sub_ip2.Text = ip_container[1];
                sub_ip3.Text = ip_container[2];
                sub_ip4.Text = ip_container[3];
                try
                {
                    port.Text = sub_ip4.Text.Split(':')[1];
                }
                catch (Exception)
                {

                }

            }
        }

        private void newLine(object sender, RoutedEventArgs e)
        {
            int position;

            position = 0;
            position = position + input_text.SelectionStart;

            if (input_text.IsFocused)
            {
                if (!nouveau)
                {
                    input_text.Text = input_text.Text.Insert(input_text.SelectionStart, Environment.NewLine);
                    input_text.SelectionStart = position + 1;
                }
            }
        }

        private void TextBox_TextInput(object sender, TextCompositionEventArgs e)
        {

        }


        private void Send_Click(object sender, RoutedEventArgs e)
        {
            String l_message;
            l_message = input_text.Text;
            if (nouveau)
            {
                if (l_message.Length >= name_minLenght)
                {
                    username = l_message;
                    l_message = l_message + " vient de se connecter!";
                    nouveau = false;
                    input_text.MaxLength = 300;
                    send_to(l_message);
                }
                else
                {
                    l_message = ("Longueur minimal de " + name_minLenght);
                }
                send_sysMessage(l_message);
            }
            else
            {
                send_message(l_message);
            }
            reset_input();
        }

        public void send_to(String a_message)
        {
            Socket socket;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(connect_ip, 1397);
            byte[] data = Encoding.UTF8.GetBytes("TBM");
            socket.Send(data);
            //socket.Send(64000,a_message.Length, SocketFlags.None, new IPEndPoint(connect_ip,1397));
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (new_connection())
            {
                send_sysMessage("Quel est votre username? ("
                                                    + name_minLenght +
                                              " à " + name_maxLenght +
                                              " caractères)");
            }
            
            
        }

        private void send_sysMessage(String a_message)
        {
            String name;
            name = "Système";
            this.list_message.Items.Add(new row_template(name, a_message));
        }

        private void send_message(String a_message)
        {
            TextBlock message = new TextBlock();
            
            a_message = (a_message + "\n");
            this.list_message.Items.Add(new row_template(username, a_message));
        }
        
        private Boolean new_connection()
        {
            Boolean connect;
            connect = false;
            nouveau = true;
            reset_input();

            String ip = (sub_ip1.Text + "."
                            + sub_ip2.Text + "."
                            + sub_ip3.Text + "."
                            + sub_ip4.Text);
            try
            {
                connect_ip = System.Net.IPAddress.Parse(ip);
                send_sysMessage("Connection vers l'addresse: " + ip);
                connect = true;
            }
            catch (FormatException)
            {
                send_sysMessage("Erreur, vérifier l'addresse IP.");
            }
            return connect;
        }

        

        public void reset_input()
        {
            input_text.Clear();
        }
    }

}
