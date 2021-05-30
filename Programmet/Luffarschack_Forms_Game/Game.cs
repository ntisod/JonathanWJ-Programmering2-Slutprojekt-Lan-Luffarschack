using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Luffarschack_Forms_Game
{
    public partial class Game : Form
    {
        public Game(bool isHost, string ip = null)
        {
            InitializeComponent();
            MessageReceiver.DoWork += MessageReceiver_DoWork;
            CheckForIllegalCrossThreadCalls = false; // Används för att lättare ansluta enheter när man debuggar programmet

            // Om användaren har valt att vara matchens server-host
            if (isHost)
            {
                playerChar = 'X';
                opponentChar = 'O';
                // Starta servern och lyssna efter klienter som försöker att ansluta till matchen med porten (5732)
                server = new TcpListener(System.Net.IPAddress.Any, 5732);
                server.Start();
                sock = server.AcceptSocket();

                // Lägger till " - Host" till titeln "Luffarschack Game" så att det är lättate att se vilken enhet som är server-host och vilken enhet som är klienten om man spelar på samma enhet
                //ActiveForm.Text += " - Host";
            }
            else
            {
                playerChar = 'O';
                opponentChar = 'X';
                // try-catch block som ser till så att problem som kan uppstå i anslutningen inte slutar med att klienten eller servern kraschar
                try
                {
                    client = new TcpClient(ip, 5732);
                    sock = client.Client; // Deklarera sock's värde till att vara den nya clientens Socket
                    MessageReceiver.RunWorkerAsync();

                    // Lägger till " - Client" till titeln "Luffarschack Game" så att det är lättate att se vilken enhet som är server-host och vilken enhet som är klienten om man spelar på samma enhet
                    //ActiveForm.Text += " - Client";
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message); // Visa felmedelandet om ett fel uppstår
                    Close();  // Stäng ner Windows Forms objektet
                }
            }
        }

        // Funktionen som hanterar alla medelanden som skickas mellan servern och klienten
        // Den tar alltså emot informationen om vilka drag som spelarna gör under matchen
        private void MessageReceiver_DoWork(object sender, DoWorkEventArgs e)
        {
            if (CheckState()) 
            {
                return;
            }
            
            FreezeBoard();
            label1.Text = "Opponents Turn";
            ReceiveMove();

            label1.Text = "Your Turn";
            if (!CheckState())
            {
                UnfreezeBoard();
            }
        }

        // Vairablerna som används i programmet
        private char playerChar;
        private char opponentChar;
        private Socket sock;
        private BackgroundWorker MessageReceiver = new BackgroundWorker();
        private TcpListener server = null;
        private TcpClient client;

        // Kollar på spelbrädet (knapparna) för att se om någon av spelarna har vunnit eller om spelet är oavgjort
        private bool CheckState()
        {
            #region Horizontals
            // Top Row
            if (button1.Text == button2.Text && button2.Text == button3.Text && button3.Text != "")
            {
                if (button1.Text[0] == playerChar)
                {
                    label1.Text = "You Won!";
                }
                else
                {
                    label1.Text = "You Lost";
                }
                return true;
            }

            // Mid Row
            else if (button4.Text == button5.Text && button5.Text == button6.Text && button6.Text != "")
            {
                if (button4.Text[0] == playerChar)
                {
                    label1.Text = "You Won!";
                }
                else
                {
                    label1.Text = "You Lost";
                }
                return true;
            }

            // Bot Row
            else if (button7.Text == button8.Text && button8.Text == button9.Text && button9.Text != "")
            {
                if (button7.Text[0] == playerChar)
                {
                    label1.Text = "You Won!";
                }
                else
                {
                    label1.Text = "You Lost";
                }
                return true;
            }
            #endregion Horizontals

            #region Verticals
            // Left Column
            else if (button1.Text == button4.Text && button4.Text == button7.Text && button7.Text != "")
            {
                if (button1.Text[0] == playerChar)
                {
                    label1.Text = "You Won!";
                }
                else
                {
                    label1.Text = "You Lost";
                }
                return true;
            }
            
            // Mid Column
            else if (button2.Text == button5.Text && button5.Text == button8.Text && button8.Text != "")
            {
                if (button2.Text[0] == playerChar)
                {
                    label1.Text = "You Won!";
                }
                else
                {
                    label1.Text = "You Lost";
                }
                return true;
            }

            // Right Column
            else if (button3.Text == button6.Text && button6.Text == button9.Text && button9.Text != "")
            {
                if (button3.Text[0] == playerChar)
                {
                    label1.Text = "You Won!";
                }
                else
                {
                    label1.Text = "You Lost";
                }
                return true;
            }
            #endregion Verticals

            #region Diagonals
            // TopLeft to BottomRight
            else if (button1.Text == button5.Text && button5.Text == button9.Text && button9.Text != "")
            {
                if (button1.Text[0] == playerChar)
                {
                    label1.Text = "You Won!";
                }
                else
                {
                    label1.Text = "You Lost";
                }
                return true;
            }
            
            // TopRight to BottomLeft
            else if (button3.Text == button5.Text && button5.Text == button7.Text && button7.Text != "")
            {
                if (button3.Text[0] == playerChar)
                {
                    label1.Text = "You Won!";
                    MessageBox.Show("You Won!");
                }
                else
                {
                    label1.Text = "You Lost";
                    MessageBox.Show("You Lost");
                }
                return true;
            }
            #endregion Diagonals

            
            // Draw
            else if (button1.Text != "" && button2.Text != "" && button3.Text != "" && button4.Text != "" && button5.Text != "" && button6.Text != "" && button7.Text != "" && button8.Text != "" && button9.Text != "")
            {
                label1.Text = "Game ends in a Draw";
                return true;
            }

            return false;
        }

        // Avaktiverar (fryser) alla knapparna på spelbrädet när det är den andra spelarens tur
        private void FreezeBoard()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
        }

        // Aktiverar (avfryser) alla knapparna på spelbrädet som ska aktiveras
        // Det vill säga alla knapparna som inte har en spelpjäs på sig
        private void UnfreezeBoard()
        {
            if (button1.Text == "") { button1.Enabled = true; }
            if (button2.Text == "") { button2.Enabled = true; }
            if (button3.Text == "") { button3.Enabled = true; }
            if (button4.Text == "") { button4.Enabled = true; }
            if (button5.Text == "") { button5.Enabled = true; }
            if (button6.Text == "") { button6.Enabled = true; }
            if (button7.Text == "") { button7.Enabled = true; }
            if (button8.Text == "") { button8.Enabled = true; }
            if (button9.Text == "") { button9.Enabled = true; }
        }

        // Tar emot drag som den andra spelaren gjort
        private void ReceiveMove()
        {
            byte[] buffer = new byte[1];
            sock.Receive(buffer); // Vänta på att den andra spelaren har gjort sitt drag och att det draget tas emot här

            if (buffer[0] == 1) { button1.Text = opponentChar.ToString(); }
            if (buffer[0] == 2) { button2.Text = opponentChar.ToString(); }
            if (buffer[0] == 3) { button3.Text = opponentChar.ToString(); }
            if (buffer[0] == 4) { button4.Text = opponentChar.ToString(); }
            if (buffer[0] == 5) { button5.Text = opponentChar.ToString(); }
            if (buffer[0] == 6) { button6.Text = opponentChar.ToString(); }
            if (buffer[0] == 7) { button7.Text = opponentChar.ToString(); }
            if (buffer[0] == 8) { button8.Text = opponentChar.ToString(); }
            if (buffer[0] == 9) { button9.Text = opponentChar.ToString(); }
        }

        #region Labels
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        #endregion Labels

        // Alla funktionerna som hanterar eventen för knapparna på spelbrådet
        #region Buttons
        private void button1_Click(object sender, EventArgs e)
        {
            byte[] num = { 1 };
            sock.Send(num);
            button1.Text = playerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] num = { 2 };
            sock.Send(num);
            button2.Text = playerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] num = { 3 };
            sock.Send(num);
            button3.Text = playerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] num = { 4 };
            sock.Send(num);
            button4.Text = playerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            byte[] num = { 5 };
            sock.Send(num);
            button5.Text = playerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            byte[] num = { 6 };
            sock.Send(num);
            button6.Text = playerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            byte[] num = { 7 };
            sock.Send(num);
            button7.Text = playerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            byte[] num = { 8 };
            sock.Send(num);
            button8.Text = playerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            byte[] num = { 9 };
            sock.Send(num);
            button9.Text = playerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        #endregion Buttons

        // Funktionen som ser till att spelets resurser släpps på rätt sätt när Windows Forms objekten stängs ner
        // Alltså förhindrar funktionen att möjliga memory leaks kan inträffa
        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageReceiver.WorkerSupportsCancellation = true;
            MessageReceiver.CancelAsync();
            if (server != null)
            {
                server.Stop();
            }
        }
    }
}
