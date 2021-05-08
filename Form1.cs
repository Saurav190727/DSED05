using DSED05.Business;
using DSED05.Properties;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DSED05
{
    public partial class Form1 : Form
    {
        #region Global Veriables
        //Create my racers
        Racer[] racers = new Racer[4];
        //Create my Punters
        Punter[] myPunters = new Punter[3];
        //Create my Punters Balance Labels
        Label[] puntersBalanceLabels;

        RadioButton[] punterRadioButtons;

        //Which Racer Wins
        private int RacerWinner;

        private bool betPlaced = false;
        #endregion

        #region Form Initiate
        public Form1()
        {
            InitializeComponent();
            LoadRacers();
            LoadPunters();
            updatePuntersBalanceLabels();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            punterRadioButtons = new RadioButton[] { radJack, radVaughn, radJeremy  };
        }
        #endregion

        #region Load Punters and Racers
        private void LoadPunters()
        {
            for (int i = 0; i < 3; i++)
            {
                myPunters[i] = Factory.GetAPunter(i);
                //myPunter[0] which is howard is a New Howard
                myPunters[i].labelWinner = lblWinner;
            }
            puntersBalanceLabels = new Label[] { lblJack, lblVaughn, lblJeremy };
        }

        private void LoadRacers()
        {
            racers[0] = new Racer { length = 0, myPB = pbRacer1, name = "CSharp" };
            racers[0].myPB.BackgroundImage = Resources.csharpLogoResize;
            racers[1] = new Racer { length = 0, myPB = pbRacer2, name = "Java" };
            racers[1].myPB.BackgroundImage = Resources.javaLogoResize;
            racers[2] = new Racer { length = 0, myPB = pbRacer3, name = "Php" };
            racers[2].myPB.BackgroundImage = Resources.phpLogoResize;
            racers[3] = new Racer { length = 0, myPB = pbRacer4, name = "Python" };
            racers[3].myPB.BackgroundImage = Resources.pythonLogoResize;
            for (int i = 0; i < racers.Length; i++)
            {
                cbxRacers.Items.Add(racers[i].name);
            }
        }
        #endregion

        #region Resetting the Race
        private void ResetRace()
        {
            // Putting all of the racers back the the beginning and resetting all of their bets
            int start = pbRaceTrack.Left;
            lbxEvents.Items.Clear();
            for (int i = 0; i < racers.Length; i++)
            {
                racers[i].myPB.Left = start;
            }
            int count = 0;
            for (int i = 0; i < myPunters.Length; i++)
            {
                myPunters[i].bet = 0;
                // Disabling the Punters from playing when they have run out of cash

                if (myPunters[i].cash == 0)
                {
                    count = count + 1;
                    foreach (RadioButton radioButton in punterRadioButtons)
                    {
                        if (radioButton.Text == myPunters[i].name)
                        {
                            radioButton.Enabled = false;
                            lbxEvents.Items.Add($"{myPunters[i].name} is Busted");
                        }
                    }
                }
                // If all of the punters have no cash, it will restart the game and set all of the radiobuttons back to enabled
                if (count == myPunters.Length)
                {
                    MessageBox.Show("Game Over, All punters have no more cash. Restarting Game");
                    LoadPunters();
                    updatePuntersBalanceLabels();
                    foreach (RadioButton radioButton in punterRadioButtons)
                    {
                        radioButton.Enabled = true;

                    }
                }
            }
        }
        #endregion

        #region Running the Race
        private void RunRace()
        {
            //Setting all Radio Buttons to false
            foreach (RadioButton radioButton in punterRadioButtons)
            {
                radioButton.Checked = false;
            }


            bool end = false;
            //Running the rade until end has been set to True
            while (!end)
            {
                //Calculating the distance of the track
                int distance = pbRaceTrack.Width - (pbRacer1.Width);
                // Initializing Random
                var myrand = new Random();
                // Looping over each racer and setting a random number for them to move forward
                for (int i = 0; i < racers.Length; i++)
                {
                    // Moving their picture boxes that set amount forward
                    racers[i].myPB.Left += myrand.Next(1, 5);

                    // Checking if a racer has reached the end
                    if (racers[i].myPB.Left > distance)
                    {
                        // Setting the index of the winner
                        RacerWinner = i;
                        // setting the end to true
                        end = true;
                        betPlaced = false;
                        // Logging the winning racer
                        lbxEvents.Items.Add($"{racers[i].name} Wins!");

                    }
                }
            }
            // Finding all of the winning and loosing punters
            FindWinner();
        }

        #region Race Finished
        private void FindWinner()
        {
            // creating an arary with the index of the punters that have won and lost
            int[] winners = new int[0];
            int[] loosers = new int[0];
            // Looping over the punters
            for (int i = 0; i < myPunters.Length; i++)
            {
                // Checking to see if the punter has won, else they will be added to the loosers array
                if (myPunters[i].racer == RacerWinner)
                {
                    myPunters[i].cash += myPunters[i].bet;
                    Array.Resize(ref winners, winners.Length + 1);
                    winners[winners.Length - 1] = i;
                }
                else
                {
                    myPunters[i].cash -= myPunters[i].bet;
                    Array.Resize(ref loosers, loosers.Length + 1);
                    loosers[loosers.Length - 1] = i;
                }
            }

            // Creating the winners and loosers text to be output into the Events Box
            // Label Winner
            string[] winnersTextArr = WinnersText(winners, loosers);
            foreach (string text in winnersTextArr)
            {
                lbxEvents.Items.Add(text);
            }
            // Updating the punters Balance labels
            updatePuntersBalanceLabels();
        }

        private string[] WinnersText(int[] winners, int[] loosers)
        {
            // Looping over the winners and loosers text and creating an array with the text to be put into the events box
            string[] winnersText = new string[3];
            for (int i = 0; i < winners.Length; i++)
            {
                winnersText[i] = $"{myPunters[winners[i]].name} Won {myPunters[winners[i]].bet}";
            }
            for (int i = 0; i < loosers.Length; i++)
            {
                winnersText[i + winners.Length] = $"{myPunters[loosers[i]].name} Lost {myPunters[loosers[i]].bet}";
            }
            return winnersText;
        }
        #endregion
        #endregion

        #region Bettings

        private void updatePuntersBalanceLabels()
        {
            for (int i = 0; i < myPunters.Length; i++)
            {
                puntersBalanceLabels[i].Text = $"${myPunters[i].cash}";
            }
        }

        private void placeBet()
        {
            var currentPunter = puntersRADBox.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);

            if (currentPunter != null)
            {
                if (betAmount.Value.ToString() != "0")
                {
                    if (cbxRacers.SelectedItem != null)
                    {
                        for (int i = 0; i < myPunters.Length; i++)
                        {
                            if (myPunters[i].name == currentPunter.Text)
                            {
                                if (myPunters[i].cash >= float.Parse(betAmount.Value.ToString()))
                                {
                                    if (myPunters[i].bet == 0)
                                    {
                                        myPunters[i].bet = float.Parse(betAmount.Value.ToString());
                                        for (int x = 0; x < racers.Length; x++)
                                        {
                                            if (racers[x].name == cbxRacers.SelectedItem.ToString())
                                            {
                                                myPunters[i].racer = x;
                                            }
                                        }
                                        lbxEvents.Items.Add($"{myPunters[i].name} has placed a bet of {myPunters[i].bet} on {racers[myPunters[i].racer].name}");
                                    }
                                    else
                                    {
                                        MessageBox.Show("This punter has already placed their bet");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Sorry the bet is to high");
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose a Racer");

                    }
                }
                else
                {
                    MessageBox.Show("Please choose a bet Amount");

                }

            }
            else
            {
                MessageBox.Show("Please select a Punter before placing your bet!");
            }
        }

        private bool checkBetsPlaced()
        {
            for (int i = 0; i < myPunters.Length; i++)
            {
                if (myPunters[i].bet == 0 && myPunters[i].cash != 0)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Form Controls
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pbRacer1_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (checkBetsPlaced())
            {
                RunRace();
            }
            else
            {
                MessageBox.Show("Please make sure all bets are placed");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetRace();
        }
        private void btnPlaceBet_Click(object sender, EventArgs e)
        {
            placeBet();
        }

        private void punterRad_Changed(object sender, EventArgs e)
        {
            RadioButton rad = (RadioButton)sender;
            if (rad.Checked == true)
            {
                foreach (var punter in myPunters)
                {
                    if (punter.name == rad.Text)
                    {
                        betAmount.Maximum = (decimal)punter.cash;
                    }
                }
            }
        }
        #endregion


    }
}
