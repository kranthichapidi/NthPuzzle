using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NthPuzzle
{
    public partial class Form1 : Form
    {
        bool ISLOADGAME = false;
        int rows = 4, columns = 4;
        Random r;
        List<int> inputList = new List<int>();
        string Inputs;
        string compareString = "";
        string compareStringNew = "";
        string currInputs = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TXTRowsAndColumns.Text))
                rows = columns = Convert.ToInt32(TXTRowsAndColumns.Text);
            else
                MessageBox.Show("You havent entered any input. So, This game is going to start with 4 rows and 4 columns.");
            ClearLayout();
            FillInputs();
            CreatingControles();
            CreateInputs();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "c:\\";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                compareString = File.ReadAllText(ofd.FileName).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\n", string.Empty);
                string[] rowsColumns = compareString.Split(',');
                rows = columns = Convert.ToInt32(Math.Sqrt(rowsColumns.Count()));
                TXTRowsAndColumns.Text = rows.ToString();
                ClearLayout();
                FillInputs();
                CreatingControles(compareString);
                CreateInputs();
                //startGame(tagging);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog save = new SaveFileDialog();
            save.InitialDirectory = "c:\\";
            if (save.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = save.OpenFile()) != null)
                {
                    myStream.Close();
                    System.IO.File.WriteAllLines(save.FileName, new[] { compareString });
                }
            }
        }
        public void ClearLayout()
        {
            TLPGameLayout.Controls.Clear();
            TLPGameLayout.RowStyles.Clear();
            TLPGameLayout.ColumnStyles.Clear();
            TLPGameLayout.ColumnCount = columns;
            TLPGameLayout.RowCount = rows;
            for (int i = 0; i < rows; i++)
            {
                TLPGameLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                TLPGameLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            }
        }
        public void FillInputs()
        {
            int length = rows * columns;
            Inputs = "";
            for (int i = 0; i < length; i++)
            {
                inputList.Add(i);
                if (i == length - 1)
                    Inputs += i.ToString();
                else
                    Inputs += i.ToString() + ",";
            }
        }
        public void CreatingControles()
        {
            string[] inputs = compareString.Split(',');
            currInputs = "";
            int inputIndex = 0;
            int k = 0;
            r = new Random();
            for (int i = 0; i < TLPGameLayout.RowCount; i++)
            {
                for (int j = 0; j < TLPGameLayout.ColumnCount; j++)
                {
                    Button controleButton = new Button();
                    TLPGameLayout.Controls.Add(controleButton, i, j);
                    controleButton.Dock = DockStyle.Fill;
                    controleButton.Click += ControleButton_Click;
                    controleButton.Font = new Font("Times New Roman", 35);
                    if (ISLOADGAME)
                    {
                        currInputs = inputs[inputIndex];
                        inputIndex++;
                    }
                    else
                    {
                        k = r.Next(0, inputList.Count);
                        currInputs = inputList[k].ToString();
                    }
                    controleButton.Text = currInputs;
                    if (currInputs != "0")
                        controleButton.Visible = true;
                    else
                        controleButton.Visible = false;
                    if (!ISLOADGAME)
                        inputList.RemoveAt(k);
                }

            }
        }
        public void CreatingControles(string fromFile)
        {
            string[] inputs = fromFile.Split(',');
            int inputIndex = 0;
            for (int i = 0; i < TLPGameLayout.RowCount; i++)
            {
                for (int j = 0; j < TLPGameLayout.ColumnCount; j++)
                {
                    Button controleButton = new Button();
                    TLPGameLayout.Controls.Add(controleButton, j, i);
                    controleButton.Dock = DockStyle.Fill;
                    controleButton.Click += ControleButton_Click;
                    controleButton.Font = new Font("Times New Roman", 35);
                    currInputs = inputs[inputIndex];
                    inputIndex++;
                    controleButton.Text = currInputs;
                    if (currInputs != "0")
                        controleButton.Visible = true;
                    else
                        controleButton.Visible = false;
                }

            }
        }
        public void CreateInputs()
        {
            compareString = "";
            compareStringNew = 0.ToString();
            int count = 0;
            for (int i = 0; i < TLPGameLayout.RowCount; i++)
            {
                for (int j = 0; j < TLPGameLayout.ColumnCount; j++)
                {
                    Button button = (Button)TLPGameLayout.GetControlFromPosition(j, i);
                    compareString += button.Text;
                    if (count < rows * columns)
                    {
                        compareString += ",";
                        count++;
                    }
                }
            }
            compareStringNew = 0 + "," + compareString;
        }
        private void TXTRowsAndColumns_TextChanged(object sender, EventArgs e)
        {

        }
        public void Swap(Button currButton, Button nextButton)
        {
            string text = nextButton.Text.ToString();
            nextButton.Text = currButton.Text;
            nextButton.Text = currButton.Text;
            nextButton.Visible = true;
            currButton.Text = text;
            currButton.Visible = false;
        }
        public void winningDecition()
        {
            if ((String.Compare(this.Inputs, this.compareString) == 0) || (this.compareStringNew.Contains(this.Inputs)))
            {
                string message = "Game is over.\nTry again?";
                string caption = "The end";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Information);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    inputList = new List<int>();
                    compareString = "";
                    compareStringNew = "";
                    currInputs = "";
                    ClearLayout();
                    FillInputs();
                    CreatingControles();
                    CreateInputs();
                }
                else
                    this.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog save = new SaveFileDialog();
            save.InitialDirectory = "c:\\";
            if (save.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = save.OpenFile()) != null)
                {
                    myStream.Close();
                    System.IO.File.WriteAllLines(save.FileName, new[] { compareString });
                    this.Close();
                }
            }
        }

        private void ControleButton_Click(object sender, EventArgs e)
        {
            int column = TLPGameLayout.GetColumn(sender as Button);
            int row = TLPGameLayout.GetRow(sender as Button);
            Button currButton = (sender as Button);
            if (row != 0)
            {
                //Top
                Button b = new Button();
                b = (Button)TLPGameLayout.GetControlFromPosition(column, row - 1);
                if (b.Visible == false)
                {
                    Swap(currButton, b);
                    CreateInputs();
                    winningDecition();
                }
            }
            if (row != rows - 1)
            {
                //Bottom
                Button b = new Button();
                b = (Button)TLPGameLayout.GetControlFromPosition(column, row + 1);
                if (b.Visible == false)
                {
                    Swap(currButton, b);
                    CreateInputs();
                    winningDecition();
                }
            }
            if (column != 0)
            {
                //Left
                Button b = new Button();
                b = (Button)TLPGameLayout.GetControlFromPosition(column - 1, row);
                if (b.Visible == false)
                {
                    Swap(currButton, b);
                    CreateInputs();
                    winningDecition();
                }
            }
            if (column != column - 1)
            {
                //Right
                Button b = new Button();
                if (column == column - 1)
                    b = (Button)TLPGameLayout.GetControlFromPosition(column, row);
                else
                    b = (Button)TLPGameLayout.GetControlFromPosition(column + 1, row);
                if (b != null && b.Visible == false)
                {
                    Swap(currButton, b);
                    CreateInputs();
                    winningDecition();
                }
            }
        }
    }
}
