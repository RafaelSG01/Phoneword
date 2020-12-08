using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace App3
{
    class MainPage : ContentPage
    {
        Entry phoneNumberText;
        Button translateButton;
        Button callButton;
        string translatedNumber;
        public MainPage()
        {
            this.Padding = new Thickness(20, 20, 20, 20);
            StackLayout panel = new StackLayout
            {
                Spacing = 15
            };

            panel.Children.Add(new Label
            {
                Text = "Entre com a senha:",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });
            panel.Children.Add(phoneNumberText = new Entry
            {
                Text = "(85)5XAMARIN",
            });
            panel.Children.Add(translateButton = new Button
            {
                Text = "Converter"
            });
            panel.Children.Add(callButton = new Button
            {
                Text = "Call",
                IsEnabled = false,
            });

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;

            this.Content = panel;
        }
        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = phoneNumberText.Text;
            translatedNumber = App3.PhonewordTranslator.ToNumber(enteredNumber);

            if (!string.IsNullOrEmpty(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = "Ligar " + translatedNumber;
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Ligar";
            }
        }
        async void OnCall(object sender, System.EventArgs e)
        {
            if (await this.DisplayAlert(
            "Discar um Numero",
            "Você gostaria de discar o número " + translatedNumber + "?",
            "Sim",
            "Não"))
            {
                try
                {
                    PhoneDialer.Open(translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Não habilitado para discar", "Número de telefone inválido.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Não habilitado para discar", "Discagem não suportada.", "OK");
                }
                catch (Exception)
                {
                    // Other error has occurred.
                    await DisplayAlert("Não habilitado para discar", "Discagem falhou.", "OK");
                }
            }
        }
    }
}
