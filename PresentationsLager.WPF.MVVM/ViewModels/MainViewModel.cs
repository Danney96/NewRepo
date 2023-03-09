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
                Status = $"Vald bok #{tillgängligaBöckerSelectedItem?.ISBN}: {tillgängligaBöckerSelectedItem?.Titel}";
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
                Status = $"Valda böcker #{valdaBöckerSelectedItem?.ISBN}: {valdaBöckerSelectedItem?.Titel}";
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

        private int lämnaBokningNr;
        public int LämnaBokningNr
        {
            get { return lämnaBokningNr; }
            set
            {
                lämnaBokningNr = value; OnPropertyChanged();
                TillbakaBokning = kontroller.HämtaBokning(lämnaBokningNr);
                
                Status = $"Visar bokningen med bokningsnummer: {TillbakaBokning.BokningId}";
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
                Status = $"Tillgängliga Böcker #{tillgängligaBöckerSelectedItem?.ISBN}: {tillgängligaBöckerSelectedItem?.Titel}";
                
                Status = $"Vald medlem: {medlemSelectedItem?.MedlemsId}: " +
                    $"{medlemSelectedItem?.Namn}";
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

                Status = $"Removed bok #{bok.Titel} ({bok.ISBN})";

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

                Status = $"Added bok #{bok.Titel} ({bok.ISBN})";

                IsNotModified = false;
            }
        }); //, () => TillgängligaBöcker.Count > 0

        private ICommand saveCommand = null!;
        public ICommand SaveCommand => saveCommand ??= saveCommand = new RelayCommand(() =>
        {
            if (valdaBöcker != null)
            {
                Expidit exp = kontroller.HämtaExpidit(1);
                UtBokning = kontroller.SkapaBokning(MedlemSelectedItem, exp, StartLån, ValdaBöcker);
                Status = $"Skapat bokning med följande bokningsId: {utBokning.BokningId}";
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
            if (bokningNr != null)
            {
                UtBokningensBöcker = new ObservableCollection<Bok>(kontroller.HämtaBokningensBöcker(bokningNr));
                IsNotModified = true;
            }
            
             
        });
        private ICommand hämtaUtCommand = null!;
        public ICommand HämtaUtCommand => hämtaUtCommand ??= hämtaUtCommand = new RelayCommand(() =>
        {
            UtBokning = kontroller.HämtaUtBokning(utBokning);
            Status = $"Bokningen med bokningsId {utBokning.BokningId}" +
            $"har lämnats ut {utBokning.FaktisktStartLån}" +
            $"och ska lämnas tillbakas senast {utBokning.ÅterTid}";
        });

        private ICommand sökTillbakaCommand = null!;
        public ICommand SökTillbakaCommand => sökTillbakaCommand ??= sökTillbakaCommand = new RelayCommand(() =>
        {
            TillbakaBokningensBöcker = new ObservableCollection<Bok>(kontroller.HämtaBokningensBöcker(lämnaBokningNr));
        });

        private ICommand lämnaTillbakaCommand = null!;
        public ICommand LämnaTillbakaCommand => lämnaTillbakaCommand ??= lämnaTillbakaCommand = new RelayCommand(() =>
        {
            Expidit exp = kontroller.HämtaExpidit(1);
            FakturaPotatis = kontroller.SkapaFaktura(exp, bokningNr);
        });

    }
}
