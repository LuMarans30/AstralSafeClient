using System.Diagnostics.CodeAnalysis;

namespace astralSafeClientMaui;

public partial class MainPage : ContentPage
{
    private JsonRequest JsonRequestKeygen, JsonRequestLicense;

    private readonly CryptoExe Ce;

    [RequiresAssemblyFiles()]
    public MainPage()
	{
		InitializeComponent();

        Ce = new();
    }

    private void BtnKeygen_Clicked(object sender, EventArgs e)
    {
        if (!txtUID.Text.Equals(value: ""))
        {
            JsonRequest jsonRequest = new(UID: txtUID.Text);

            try
            {
                jsonRequest.SendRequest(option: "keygen");

            }
            catch (Exception ex)
            {
                DisplayAlert(title: "Errore", message: ex.Message, cancel: "OK");
            }

            JsonRequestKeygen = jsonRequest;

            txtLicense.Text = JsonRequestKeygen.License;

            string key = JsonRequestKeygen.Key;

            byte[] bytes = CryptoExe.StringToByteArray(key: key);

            Ce.Key = bytes;

            Ce.Crypt();

            btnValidateLicense.IsEnabled = true;
        }
        else
        {
            DisplayAlert(title: "Errore", message: "Inserire un UID", cancel: "OK");
        }
    }

    private void btnValidateLicense_Clicked(object sender, EventArgs e)
    {
        if (!txtLicense.Text.Equals(value: ""))
        {

            JsonRequest jsonRequest = new(UID: JsonRequestKeygen.UID, license: txtLicense.Text);

            try
            {
                jsonRequest.SendRequest(option: "validate-license");
            }
            catch (Exception ex)
            {
                DisplayAlert(title: "Errore", message: ex.Message, cancel: "OK");
            }

            JsonRequestLicense = jsonRequest;

            if(!string.IsNullOrEmpty(value: JsonRequestLicense.Key))
            {
                DisplayAlert(title: "Informazione", message: "La licenza è valida", cancel: "OK");

                Ce.Decrypt();

            }else if(!jsonRequest.Valid)
            {
                DisplayAlert(title: "Errore", message: "La licenza non è valida", cancel: "OK");
            }
        }
        else
        {
            DisplayAlert(title: "Errore", message: "Inserire un UID", cancel: "OK");
        }
    }
}

