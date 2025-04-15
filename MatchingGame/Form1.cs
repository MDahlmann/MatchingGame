using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        //Stores the first label (square) in a set of two,
        //that a player clicks. Null if the player hasn't
        //clicked anything yet.
        Label firstClicked = null;
        //Same as above, but with second label clicked in a set.
        Label secondClicked = null;

        public Form1()
        {
            InitializeComponent();

            AssignIconsToSquares();
        }

        //'Random' object used for getting random icons for the squares
        Random rnd = new Random();

        //List of 16 strings, that when "converted" to Webdings,
        //will represent icons for the squares. Each string
        //has two appearances, so they can be matched.
        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };

        //Goes through each square (control) and assigning it
        //an index from the icons list at random. Then sets the color
        //of the icon to the background color to hide it. Then removes
        //the icon (index) from the icons list.
        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = rnd.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        //Every label's (square) Click event is handled by this.
        private void label1_Click(object sender, EventArgs e)
        {
            //Ignore clicks, when timer is running, because
            //that means two non-matching icons are clicked.
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                //If the label is black, the label
                //has already been clicked and
                //method ends.
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                //If firstClicked is null, this is the first
                //label (square) to be pressed. Assign it to
                //firstClicked and change the label to black.
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;

                    return;
                }

                //If the method reaches this, a firstClick has
                //already been made, and this label is assigned
                //to secondClicked and set to black.
                if (secondClicked == null)
                {
                    secondClicked = clickedLabel;
                    secondClicked.ForeColor = Color.Black;
                }

                //Check to see if the player won.
                CheckForWinner();

                //If two matching icons are clicked, the clicked
                //variables are reset, and the method is returned.
                //Keeping the icons visible.
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                //If method reaches this, two non-matching icons
                //were clicked, and the timer starts, counts to
                // 3/4 of a second and hides the icons.
                timer1.Start();
            }
        }

        //Timer is triggered, when two icons are clicked, that
        //don't match. After 3/4 second, it hides both icons.
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            //Hides both icons.
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            //Resets the variables that track clicked icons.
            firstClicked = null;
            secondClicked = null;
        }

        //Method check every labels foreground color to back-
        //ground color. If there are no labels with same fore-
        //and background color, all 16 icons were matched.
        //The player won.
        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            //If the method/loop didn't return, all icons are
            //matched, and the player is shown a message and form
            //is closed.
            MessageBox.Show("You've matched all the icons", "Good job and congratulations");
            Close();
        }
    }
}