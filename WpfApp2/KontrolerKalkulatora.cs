using System;
using System.ComponentModel;

namespace WpfApp2
{
    internal class KontrolerKalkulatora : INotifyPropertyChanged
    {
        private double? lewyArgument = null;
        private double? prawyArgument = null;
        private string? buforDziałania = null;
        private string wynik = "0";
        private bool flagaDziałania = false;

        public string Wynik
        {
            get => wynik;
            set
            {
                wynik = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wynik"));
            }
        }

        public string Bufory
        {
            get
            {
                if (lewyArgument == null)
                    return "";
                else if (buforDziałania == null)
                    return $"{lewyArgument}";
                else if (prawyArgument == null)
                    return $"{lewyArgument} {buforDziałania}";
                else
                    return $"{lewyArgument} {buforDziałania} {prawyArgument} =";
            }
        }

        internal void WprowadźCyfrę(string cyfra)
        {
            if (flagaDziałania)
            {
                Wynik = "0";
                flagaDziałania = false;
            }

            if (Wynik == "0")
                Wynik = cyfra;
            else
                Wynik += cyfra;
        }


        internal void ZmieńZnak()
        {
            if (flagaDziałania)
                Wynik = "0";
            if (Wynik == "0")
                return;
            else if (Wynik[0] == '-')
                Wynik = Wynik.Substring(1);
            else
                Wynik = "-" + Wynik;
        }

        internal void WprowadźPrzecinek()
        {
            if (flagaDziałania)
                Wynik = "0";
            if (Wynik.Contains(','))
                return;
            else
                Wynik += ",";
        }

        internal void SkasujZnak()
        {
            if (flagaDziałania)
                Wynik = "0";
            if (Wynik == "0")
                return;
            else if (Wynik == "-0," || Wynik.Length == 1 || (Wynik.Length == 2 && Wynik[0] == '-'))
                Wynik = "0";
            else
                Wynik = Wynik.Substring(0, Wynik.Length - 1);
        }

        internal void WyczyśćWynik()
        {
            Wynik = "0";
        }

        internal void WyczyśćWszystko()
        {
            WyczyśćWynik();
            lewyArgument = prawyArgument = null;
            buforDziałania = null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bufory"));
        }

        internal void WprowadźDziałanieDwuargumentowe(string działanie)
        {
            if (lewyArgument == null)
            {
                lewyArgument = Convert.ToDouble(Wynik);
                buforDziałania = działanie;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bufory"));
                wynik = "0";
            }
            else if (buforDziałania == null)
            {
                buforDziałania = działanie;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bufory"));
                wynik = "0";
            }
            else
            {
                prawyArgument = Convert.ToDouble(Wynik);
                WykonajDziałanie();
                buforDziałania = działanie;
                lewyArgument = Convert.ToDouble(Wynik);
                prawyArgument = null;
                wynik = "0";
                flagaDziałania = true;
            }
        }

        internal void WykonajDziałanie()
        {
            if (prawyArgument == null)
                prawyArgument = Convert.ToDouble(Wynik);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bufory"));

            switch (buforDziałania)
            {
                case "+":
                    Wynik = $"{lewyArgument + prawyArgument}";
                    break;
                case "-":
                    Wynik = $"{lewyArgument - prawyArgument}";
                    break;
                case "×":
                    Wynik = $"{lewyArgument * prawyArgument}";
                    break;
                case "÷":
                    Wynik = $"{lewyArgument / prawyArgument}";
                    break;
                case "xʸ":
                    Wynik = $"{Math.Pow((double)lewyArgument, prawyArgument.Value)}";
                    break;
                case "%":
                    Wynik = $"{(prawyArgument / 100)*lewyArgument}";
                    break;
                case "mod":
                    Wynik = $"{lewyArgument % prawyArgument}";
                    break;
            }
        }

        internal void WykonajDziałanieJednoargumentowe(string działanie)
        {
            if (lewyArgument == null)
                lewyArgument = Convert.ToDouble(Wynik);

            switch (działanie)
            {
                case "√":
                    lewyArgument = Math.Sqrt(lewyArgument.Value);
                    break;
                case "1/x":
                    lewyArgument = 1 / lewyArgument;
                    break;
                case "x!":
                    lewyArgument = ObliczSilnię(Convert.ToInt32(lewyArgument));
                    break;
                case "log":
                    lewyArgument = Math.Log10(lewyArgument.Value);
                    break;
                case "ln":
                    lewyArgument = Math.Log(lewyArgument.Value);
                    break;
                case "floor":
                    lewyArgument = Math.Floor(lewyArgument.Value);
                    break;
                case "ceil":
                    lewyArgument = Math.Ceiling(lewyArgument.Value);
                    break;
            }

            Wynik = $"{lewyArgument}";
            flagaDziałania = true;
            buforDziałania = null;
            prawyArgument = null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bufory"));
        }

        private double ObliczSilnię(int n)
        {
            if (n == 0)
                return 1;
            else
                return n * ObliczSilnię(n - 1);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

