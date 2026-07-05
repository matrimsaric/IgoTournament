using CommonModule.Enums;
using JosekiDomain.Model;
using StoneLedger.Services.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace StoneLedger.ViewModels.JosekiStudy
{
    public class JosekiListViewModel : BaseViewModel
    {
        private readonly JosekiEntryService _josekiEntryService;

        public ObservableCollection<JosekiEntry> JosekiEntries { get; } = new();
        public ObservableCollection<VariationType> Categories { get; } =
    new(Enum.GetValues(typeof(VariationType))
        .Cast<VariationType>()
        .ToList());


        private VariationType _selectedCategory = VariationType.None;
        public VariationType SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    LoadEntriesCommand.Execute(null);
                    OnPropertyChanged(nameof(CanAdd));
                }
            }
        }


        public bool CanAdd => SelectedCategory != VariationType.None;


        private JosekiEntry? _selectedEntry;
        public JosekiEntry? SelectedEntry
        {
            get => _selectedEntry;
            set
            {
                if (SetProperty(ref _selectedEntry, value) && value != null)
                {
                    OnSelectEntry(value);
                }
            }
        }

        private async void OnSelectEntry(JosekiEntry entry)
        {
            await Shell.Current.GoToAsync($"joseki?id={entry.Id}");
        }

        public ICommand AddEntryCommand => new Command(async () =>
        {
            // Navigate to the JosekiStudyPage with no ID → new entry mode
            await Shell.Current.GoToAsync("joseki");
        });





        public ICommand LoadEntriesCommand { get; }

        public JosekiListViewModel(JosekiEntryService service)
        {
            _josekiEntryService = service;

            LoadEntriesCommand = new Command(async () => await LoadEntriesAsync());


            LoadEntriesCommand.Execute(null);
        }

        private async Task LoadEntriesAsync()
        {
            JosekiEntries.Clear();

            List<JosekiEntry> items =
    SelectedCategory == VariationType.None
        ? (await _josekiEntryService.GetAllJosekiAsync()).ToList()
        : (await _josekiEntryService.GetByCategoryAsync((int)SelectedCategory)).ToList();


            foreach (var item in items)
                JosekiEntries.Add(item);
        }

        private int CategoryToInt(string category) =>
            category switch
            {
                "Joseki" => 0,
                "Fuseki" => 1,
                "Tesuji" => 2,
                "Yose" => 3,
                _ => -1
            };


    }

}
