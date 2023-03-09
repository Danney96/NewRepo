using Business.Classes;
using BusinessLayer;
using PresentationsLager.WPF.MVVM.Commands;
using PresentationsLager.WPF.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PresentationsLager.WPF.MVVM.ViewModels
{
    public class MainViewModel : ObservableObject
    {

        /// <summary>
        /// Fixa så att böcker försvinner från datagrid när man klickar på knappen boka. 
        /// Fixa knappen "sök bokning" vid återlämning så man inte kan visa en bokning som inte hämtats ut. 
        /// Fixa Datum så vi inte visar timmar, minuter & sekunder. 
        /// </summary>
        private Kontroller kontroller;

        private ObservableCollection<Bok> valdaBöcker = null!;
        public ObservableCollection<Bok> ValdaBöcker { get => valdaBöcker; set { valdaBöcker = value; OnPropertyChanged(); } }
        private ObservableCollection<Bok> tillgängligaBöcker = null!;
        public ObservableCollection<Bok> TillgängligaBöcker { get => tillgängligaBöcker; set { tillgängligaBöcker = value; OnPropertyChanged(); } }
        private ObservableCollection<Bok> bokningensBöcker = null!;
        public ObservableCollection<Bok> BokningensBöcker { get => bokningensBöcker; set { bokningensBöcker = value; OnPropertyChanged(); } }
        private Bokning valdBokning;
        public Bokning ValdBokning { get => valdBokning; set { valdBokning = value; OnPropertyChanged(); } }
        private Bokning utBokning;
        public Bokning UtBokning { get => utBokning; set { utBokning = value; OnPropertyChanged(); } }

        private ObservableCollection<Bok> utBokningensBöcker = null!;
        public ObservableCollection<Bok> UtBokningensBöcker { get => utBokningensBöcker; set { utBokningensBöcker = value; OnPropertyChanged(); } }

        private ObservableCollection<Bok> tillbakaBokningensBöcker = null!;
        public ObservableCollection<Bok> TillbakaBokningensBöcker { get => tillbakaBokningensBöcker; set { tillbakaBokningensBöcker = value; OnPropertyChanged(); } }

        private Bokning tillbakaBokning;
        public Bokning TillbakaBokning { get => tillbakaBokning; set { tillbakaBokning = value; OnPropertyChanged(); } }

        private Faktura fakturaPotatis;
        public Faktura FakturaPotatis { get => fakturaPotatis; set { fakturaPotatis = value; OnPropertyChanged(); } }

        private Expidit inlogg;
        public Expidit Inlogg { get => inlogg; set { inlogg = value; OnPropertyChanged(); } }

        private int anställningsId;
        public int AnställningsId
        {
            get { return anställningsId; }
            set
            {
                anställningsId = value; OnPropertyChanged();
            }
        }
        private string lösenordInlogg;
        public string LösenordInlogg
        {
            get { return lösenordInlogg; }
            set
            {
                lösenordInlogg = value; OnPropertyChanged();
            }
        }


        private string status = "Ready";
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }

        private bool isNotModified = true;
        public bool IsNotModified
        {
            get { return isNotModified; }
            set { isNotModified = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Bok> boks = null!;
        public ObservableCollection<Bok> Boks
        {
            get => boks;
            set { boks = value; OnPropertyChanged(); }
        }

        private DateTime startLån;  //FIXA SENARE
        public DateTime StartLån
        {
            get
            {
                return startLån;
            }
            set
            {
                startLån = value;
                OnPropertyChanged();
                
            }
        }

        private ObservableCollection<Bokning> bokning = null!;
        public ObservableCollection<Bokning> Bokning
        {
            get => bokning;
            set { bokning = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Faktura> faktura = null!;
        public ObservableCollection<Faktura> Faktura
        {
            get => faktura;
            set { faktura = value; OnPropertyChanged(); }
        }
        private int tillgängligaBöckerSelectedIndex;
        public int TillgängligaBöckerSelectedIndex { get { return tillgängligaBöckerSelectedIndex; } set { tillgängligaBöckerSelectedIndex = value; OnPropertyChanged(); } }

        private Bok tillgängligaBöckerSelectedItem = null!;
        public Bok TillgängligaBöckerSelectedItem
        {
            get { return tillgängligaBöckerSelectedItem; }
            set
            {
                tillgängligaBöckerSelectedItem = value; OnPropertyChanged();              
                Status = $"Tryck på knappen 'Lägg till' för att lägga till boken: {TillgängligaBöckerSelectedItem?.Titel} med ISBN: " +
                    $"{TillgängligaBöckerSelectedItem?.ISBN} till eran bokning.";
            }           
        }

        private int valdaBöckerSelectedIndex;
        public int ValdaBöckerSelectedIndex { get { return valdaBöckerSelectedIndex; } set { valdaBöckerSelectedIndex = value; OnPropertyChanged(); } }

        private Bok valdaBöckerSelectedItem = null!;
        public Bok ValdaBöckerSelectedItem
        {
            get { return valdaBöckerSelectedItem; }
            set
            {
                valdaBöckerSelectedItem= value;
                OnPropertyChanged();
                Status = $"Valda böcker #{ValdaBöckerSelectedItem?.ISBN}: {ValdaBöckerSelectedItem?.Titel}";
            }
        }

        private int bokningNr;
        public int BokningNr
        {
            get { return bokningNr; }
            set 
            {               
                bokningNr = value; OnPropertyChanged();
                ValdBokning = kontroller.HämtaBokning(bokningNr);
                BokningensBöcker = new ObservableCollection<Bok>(kontroller.HämtaBokningensBöcker(bokningNr));
                Status = $"Visar bokningen med bokningsnummer: {valdBokning.BokningId}";
            }
        }

        private int tillbakaBokningNr;
        public int TillbakaBokningNr
        {
            get { return tillbakaBokningNr; }
            set
            {
                tillbakaBokningNr = value; OnPropertyChanged();
                TillbakaBokning = kontroller.HämtaBokning(tillbakaBokningNr);
                Status = $"Visar bokningen med bokningsnummer: {tillbakaBokning.BokningId}";
            }
        }


        private ObservableCollection<Medlem> medlem = null!;
        public ObservableCollection<Medlem> Medlem
        {
            get => medlem;
            set { medlem = value; OnPropertyChanged(); }
        }

        private int medlemSelectedIndex;
        public int MedlemSelectedIndex
        {
            get { return medlemSelectedIndex; }
            set 
            { 
                medlemSelectedIndex = value; OnPropertyChanged();              
            }
        }
        private Medlem medlemSelectedItem = null!;
        public Medlem MedlemSelectedItem
        {
            get { return medlemSelectedItem; }
            set
            {
                medlemSelectedItem = value; OnPropertyChanged();

                TillgängligaBöcker = new ObservableCollection<Bok>(kontroller.HämtaAllaBöcker());
                ValdaBöcker = new ObservableCollection<Bok>();
                Status = $"Tillgängliga Böcker #{TillgängligaBöckerSelectedItem?.ISBN}: {TillgängligaBöckerSelectedItem?.Titel}";
                
                Status = $"Vald medlem: {MedlemSelectedItem?.MedlemsId}: " +
                    $"{MedlemSelectedItem?.Namn}";
            }
        }
        


        public MainViewModel()
        {
            kontroller = new Kontroller();
            kontroller.LaddaData();

            Boks = new ObservableCollection<Bok>();
            Bokning = new ObservableCollection<Bokning>();
            Faktura = new ObservableCollection<Faktura>();
            TillgängligaBöcker = new ObservableCollection<Bok>();
            

            RefreshCommand.Execute(null);
        }

        private ICommand removeCommand = null!;
        public ICommand RemoveCommand => removeCommand ??= removeCommand = new RelayCommand(() =>
        {
            if (valdaBöckerSelectedItem != null)
            {
                Bok bok = valdaBöckerSelectedItem;
                //DishIngredient di = dishesSelectedItem.DishIngredients.FirstOrDefault(di => di.DishId == dishesSelectedItem.Id && di.IngredientId == ingredient.Id);
                //dishesSelectedItem.DishIngredients.Remove(di);
                TillgängligaBöcker.Add(bok);
                ValdaBöcker.Remove(bok);

                Status = $"Tagit bort boken '{bok.Titel}' med följande ISBN: {bok.ISBN}";

                IsNotModified = false;
            }
        }); //, () => ValdaBöcker.Count > 0

        private ICommand addCommand = null!;
        public ICommand AddCommand => addCommand ??= addCommand = new RelayCommand(() =>
        {
            if (tillgängligaBöckerSelectedItem != null)
            {
                Bok bok = tillgängligaBöckerSelectedItem;


                ValdaBöcker.Add(bok);
                TillgängligaBöcker.Remove(bok);

                Status = $"Lagt till boken '{bok.Titel}' med följande ISBN: {bok.ISBN}";

                IsNotModified = false;
            }
        }); //, () => TillgängligaBöcker.Count > 0

        private ICommand saveCommand = null!;
        public ICommand SaveCommand => saveCommand ??= saveCommand = new RelayCommand(() =>
        {
            if (valdaBöcker != null)
            {
                //Expidit exp = kontroller.HämtaExpidit(1);
                UtBokning = kontroller.SkapaBokning(MedlemSelectedItem, Inlogg, StartLån, ValdaBöcker);
                Status = $"Skapat bokning med följande bokningsId: {UtBokning.BokningId}";
                IsNotModified = true;
            }
        }, () => !IsNotModified);

        private ICommand exitCommand = null!;
        public ICommand ExitCommand =>
        exitCommand ??= exitCommand = new RelayCommand(() => App.Current.Shutdown());

        private ICommand refreshCommand = null!;
        public ICommand RefreshCommand => refreshCommand ??= refreshCommand = new RelayCommand(() =>
        {

            Medlem = new ObservableCollection<Medlem>(kontroller.Hittamedlem());
            MedlemSelectedItem = (Medlem.Count > 0) ? Medlem[0] : null!;
            IsNotModified = true;
        });

        private ICommand sökCommand = null!;
        public ICommand SökCommand => sökCommand ??= sökCommand = new RelayCommand(() =>
        {
            UtBokningensBöcker = new ObservableCollection<Bok>(kontroller.HämtaBokningensBöcker(bokningNr));
            TillbakaBokning = kontroller.HämtaBokning(bokningNr);
            IsNotModified = false;
        });
        private ICommand hämtaUtCommand = null!;
        public ICommand HämtaUtCommand => hämtaUtCommand ??= hämtaUtCommand = new RelayCommand(() =>
        {
            if (UtBokningensBöcker != null)
            {
                UtBokning = kontroller.HämtaUtBokning(UtBokning);
                Status = $"Bokningen med bokningsId {UtBokning.BokningId} " +
                $"har lämnats ut {UtBokning.FaktisktStartLån} " +
                $"och ska lämnas tillbakas senast {UtBokning.ÅterTid} ";
                IsNotModified = true;
            }
            
        }, () => !IsNotModified);

        private ICommand sökTillbakaCommand = null!;
        public ICommand SökTillbakaCommand => sökTillbakaCommand ??= sökTillbakaCommand = new RelayCommand(() =>
        {
            TillbakaBokningensBöcker = new ObservableCollection<Bok>(kontroller.HämtaBokningensBöcker(tillbakaBokningNr));
            Status = $"Visar nu böckerna ni önskar lämna tillbaka";
        });

        private ICommand lämnaTillbakaCommand = null!;
        public ICommand LämnaTillbakaCommand => lämnaTillbakaCommand ??= lämnaTillbakaCommand = new RelayCommand(() =>
        {
            //Expidit exp = kontroller.HämtaExpidit(1);
            FakturaPotatis = kontroller.SkapaFaktura(Inlogg, tillbakaBokningNr);
            Status = $"Bokningen har lämnats tillbaka och en Faktura med ID: {FakturaPotatis.FakturaId} har skapats.";
        });

        private ICommand inloggCommand = null!;
        public ICommand InloggCommand => inloggCommand ??= inloggCommand = new RelayCommand(() =>
        {
            Inlogg = kontroller.Inloggning(anställningsId, lösenordInlogg);
            if (Inlogg == null)
            {
                Status = $"Du har skrivit in fel användarnamn eller lösenord";
            }
            else
            {
                
            }
        });

    }
}
