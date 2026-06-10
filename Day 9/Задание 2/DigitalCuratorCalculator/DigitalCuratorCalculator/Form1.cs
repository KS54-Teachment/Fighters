using System;
using System.Windows.Forms;

namespace DigitalCuratorCalculator
{
    public partial class Form1 : Form
    {
        private double firstNumber = 0;
        private double secondNumber = 0;
        private string operation = "";
        private bool isNewEntry = true;

        public Form1()
        {
            InitializeComponent(); // Теперь будет найден
        }

        private void btn0_Click(object sender, EventArgs e) => AppendDigit("0");
        private void btn1_Click(object sender, EventArgs e) => AppendDigit("1");
        private void btn2_Click(object sender, EventArgs e) => AppendDigit("2");
        private void btn3_Click(object sender, EventArgs e) => AppendDigit("3");
        private void btn4_Click(object sender, EventArgs e) => AppendDigit("4");
        private void btn5_Click(object sender, EventArgs e) => AppendDigit("5");
        private void btn6_Click(object sender, EventArgs e) => AppendDigit("6");
        private void btn7_Click(object sender, EventArgs e) => AppendDigit("7");
        private void btn8_Click(object sender, EventArgs e) => AppendDigit("8");
        private void btn9_Click(object sender, EventArgs e) => AppendDigit("9");

        private void AppendDigit(string digit)
        {
            if (isNewEntry)
            {
                txtDisplay.Text = digit;
                isNewEntry = false;
            }
            else
            {
                txtDisplay.Text += digit;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) => SetOperation("+");
        private void btnSubtract_Click(object sender, EventArgs e) => SetOperation("-");
        private void btnMultiply_Click(object sender, EventArgs e) => SetOperation("*");
        private void btnDivide_Click(object sender, EventArgs e) => SetOperation("/");

        private void SetOperation(string op)
        {
            firstNumber = double.Parse(txtDisplay.Text);
            operation = op;
            isNewEntry = true;
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            secondNumber = double.Parse(txtDisplay.Text);

            double result = 0;
            bool hasError = false;

            switch (operation)
            {
                case "+": result = firstNumber + secondNumber; break;
                case "-": result = firstNumber - secondNumber; break;
                case "*": result = firstNumber * secondNumber; break;
                case "/":
                    if (secondNumber == 0)
                    {
                        MessageBox.Show("Ошибка: деление на ноль!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        hasError = true;
                    }
                    else
                    {
                        result = firstNumber / secondNumber;
                    }
                    break;
            }

            if (!hasError)
            {
                txtDisplay.Text = result.ToString();
                isNewEntry = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "0";
            firstNumber = 0;
            secondNumber = 0;
            operation = "";
            isNewEntry = true;
        }

        private void btnDecimal_Click(object sender, EventArgs e)
        {
            if (!txtDisplay.Text.Contains("."))
            {
                if (isNewEntry)
                {
                    txtDisplay.Text = "0.";
                    isNewEntry = false;
                }
                else
                {
                    txtDisplay.Text += ".";
                }
            }
        }
    }
}
