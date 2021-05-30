using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luffarschack_Forms_Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Extras
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        #endregion Extras

        // Funktionen som hanterar [Connect] knappen på Windows Forms objektet för menyn
        private void button1_Click(object sender, EventArgs e)
        {
            // Anslut till ett spel som en klient (isHost = false)
            // textBox1.Text är IP-Addressen av server-hosten
            Game newGame = new Game(false, textBox1.Text);

            // När spelet startar så stängs synligheten hos Windows Forms objektet för menyn av
            Visible = false;

            // Om det inte uppstod något fel i konstruktorn när spelet skapades
            if (!newGame.IsDisposed)
            {
                newGame.ShowDialog(); // Gör Windows Forms objektet för spelet synligt
            }

            // När Windows Forms objektet för spelet stängs ner, gör Windows Forms objektet för menyn synligt igen
            Visible = true;
        }

        // Funktionen som hanterar [Host Game] knappen på Windows Forms objektet för menyn
        private void button2_Click(object sender, EventArgs e)
        {
            // Skapa ett spel som server-host (isHost = true)
            Game newGame = new Game(true);

            // När spelet startar så stängs synligheten hos Windows Forms objektet för menyn av
            Visible = false;

            // Om det inte uppstod något fel i konstruktorn när spelet skapades
            if (!newGame.IsDisposed)
            {
                newGame.ShowDialog(); // Gör Windows Forms objektet för spelet synligt
            }

            // När Windows Forms objektet för spelet stängs ner, gör Windows Forms objektet för menyn synligt igen
            Visible = true;

        }
    }
}
