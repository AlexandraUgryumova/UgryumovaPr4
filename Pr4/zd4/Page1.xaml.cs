using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace zd4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        
        private Entry loanAmountEntry;
        private Entry loanTermEntry;
        private Picker paymentTypePicker;
        private Slider interestRateSlider;
        private Label interestRateLabel;
        private Label monthlyPaymentLabel;
        private Label totalAmountLabel;
        private Label overpaymentLabel;

        public Page1()
        {

            InitializeComponent();

            loanAmountEntry = new Entry
            {
                Placeholder = "Сумма кредита"
            };

            loanTermEntry = new Entry
            {
                Placeholder = "Срок (месяцев)"
            };

            paymentTypePicker = new Picker
            {
                Title = "Вид платежа"
            };
            paymentTypePicker.Items.Add("Аннуитетный");
            paymentTypePicker.Items.Add("Дифференцированный");
            paymentTypePicker.Items.Add("Однократный");

            interestRateSlider = new Slider
            {
                Minimum = 0,
                Maximum = 100,
                Value = 5
            };

            interestRateLabel = new Label
            {
                Text = $"Процентная ставка: {interestRateSlider.Value}%"
            };

            monthlyPaymentLabel = new Label
            {
                Text = "Ежемесячный платеж: "
            };

            totalAmountLabel = new Label
            {
                Text = "Общая сумма: "
            };

            overpaymentLabel = new Label
            {
                Text = "Переплата: "
            };

            loanAmountEntry.TextChanged += UpdateCalculation;
            loanTermEntry.TextChanged += UpdateCalculation;
            paymentTypePicker.SelectedIndexChanged += UpdateCalculation;
            interestRateSlider.ValueChanged += UpdateCalculation;

            Content = new StackLayout
            {
                Children =
                {
                    loanAmountEntry,
                    loanTermEntry,
                    paymentTypePicker,
                    interestRateLabel,
                    interestRateSlider,
                    monthlyPaymentLabel,
                    totalAmountLabel,
                    overpaymentLabel
                }
            };
        }

        private void UpdateCalculation(object sender, EventArgs e)
        {
            try
            {
                double loanAmount;//сумма кре
                double.TryParse(loanAmountEntry.Text, out loanAmount);

                int loanTerm;//срок
                int.TryParse(loanTermEntry.Text, out loanTerm);

                if (loanTerm == 0)
                {
                    loanTermEntry.Text = "1";
                    DisplayAlert("Ошибка", "Срок месяцев не может быть 0", "OK");
                    return;
                }

                double interestRate = interestRateSlider.Value;
                interestRateLabel.Text = $"Процентная ставка: {interestRate}%";

                double monthlyPayment = 0;//ежемесячный платёж
                double totalAmount = 0;//общая сумма
                double overpayment = 0;//переплата

                if (paymentTypePicker.SelectedIndex == 0)
                {
                    monthlyPayment = loanAmount * (interestRate / 100 / 12) * (Math.Pow(1 + (interestRate / 100 / 12), loanTerm)) / ((interestRate / 100 / 12) * (Math.Pow(1 + (interestRate / 100 / 12), loanTerm)) - 1);
                    totalAmount = monthlyPayment * loanTerm;
                    overpayment = totalAmount - loanAmount;
                }
                else if (paymentTypePicker.SelectedIndex == 1)
                {
                    monthlyPayment = loanAmount / loanTerm;
                    double monthlyInterest = loanAmount * (interestRate / 100) / 12;
                    totalAmount = loanAmount + (monthlyInterest * loanTerm);
                    overpayment = totalAmount - loanAmount;
                }
                else if (paymentTypePicker.SelectedIndex == 2)
                {
                    monthlyPayment = loanAmount;
                    totalAmount = loanAmount;
                    double totalInterest = loanAmount * (interestRate / 100);
                    overpayment = totalInterest;
                }

                monthlyPaymentLabel.Text = $"Ежемесячный платеж: {monthlyPayment:C}";
                totalAmountLabel.Text = $"Общая сумма: {totalAmount:C}";
                overpaymentLabel.Text = $"Переплата: {overpayment:C}";
            }
            catch
            {
                DisplayAlert("Ошибка", "произошла ошибка вврода данных", "OK");
            }
        }
    }
}