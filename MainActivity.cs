using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Xamarin.Essentials;
using System.Security.Cryptography;
using System.Text;

namespace Password1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText enterPassword;
        Button copyButton, generateButton;
        CheckBox lowCaseCheckBox, upCaseCheckBox, digitCheckBox, specSignCheckBox;
        TextView textView;
        readonly string bigLetter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        readonly string smallLetter = "abcdefghijklmnopqrstuvwxyz";
        readonly string digit = "1234567890";
        readonly string special = "!@#$%^&*()_+";
        string dict = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            enterPassword = FindViewById<EditText>(Resource.Id.EnterPasswordText);
            copyButton = FindViewById<Button>(Resource.Id.CopyButton);
            generateButton = FindViewById<Button>(Resource.Id.GenerateButton);
            lowCaseCheckBox = FindViewById<CheckBox>(Resource.Id.LowerCaseCheckbox);
            upCaseCheckBox = FindViewById<CheckBox>(Resource.Id.UpperCaseCheckbox);
            digitCheckBox = FindViewById<CheckBox>(Resource.Id.DigitsCheckbox);
            specSignCheckBox = FindViewById<CheckBox>(Resource.Id.SpecialSignsCheckbox);
            textView = FindViewById<TextView>(Resource.Id.GeneratedPasswordTextview);

            generateButton.Click += GenerateButtonClick;
            copyButton.Click += CopyButtonClick;
        }

        // On click CopyButton event
        private void CopyButtonClick(object sender, EventArgs e)
        {
            Clipboard.SetTextAsync(textView.Text);

            var toast = Toast.MakeText(Application.Context, "Copied!", ToastLength.Short);
            toast.Show();
            return;
        }

        // On click generateButton event
        private void GenerateButtonClick(object sender, EventArgs e)
        {
            dict = "";
            if (lowCaseCheckBox.Checked) dict += smallLetter;
            if (upCaseCheckBox.Checked) dict += bigLetter;
            if (digitCheckBox.Checked) dict += digit;
            if (specSignCheckBox.Checked) dict += special;
            if (dict.Length < 1)
            {
                var toast = Toast.MakeText(Application.Context, "Check at least one option!", ToastLength.Short);
                toast.Show();
                return;
            }
            if (enterPassword.Text == "")
            {
                var toast = Toast.MakeText(Application.Context, "Please type something!", ToastLength.Short);
                toast.Show();
                return;
            }

            string str = enterPassword.Text;
            SHA256 sha256Hash = SHA256.Create();
            string hash = GetHash(sha256Hash, str);
            textView.Text = hash;


        }

        // Generate hash
        private string GetHash(SHA256 hashAlgorithm, string str)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                int ind = data[i] % dict.Length;
                sBuilder.Append(dict[ind]);
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        }

    }
}