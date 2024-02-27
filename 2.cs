using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json;

namespace DailyPlanner
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Note> notes;
        private DateTime selectedDate;
        private Note selectedNote;

        public ObservableCollection<Note> Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                OnPropertyChanged();
            }
        }

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                LoadNotes();
                OnPropertyChanged();
            }
        }

        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SelectedDate = DateTime.Today;
        }

        private void LoadNotes()
        {
            string filePath = $"{SelectedDate.ToString("yyyyMMdd")}.json";

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Notes = JsonConvert.DeserializeObject<ObservableCollection<Note>>(json);
            }
            else
            {
                Notes = new ObservableCollection<Note>();
            }
        }

        private void SaveNotes()
        {
            string filePath = $"{SelectedDate.ToString("yyyyMMdd")}.json";
            string json = JsonConvert.SerializeObject(Notes);
            File.WriteAllText(filePath, json);
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            Note newNote = new Note
            {
                Title = titleTextBox.Text,
                Description = descriptionTextBox.Text
            };

            Notes.Add(newNote);
            SaveNotes();
        }

        private void UpdateNote_Click(object sender, RoutedEventArgs e)
        {
            SaveNotes();
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedNote != null)
            {
                Notes.Remove(SelectedNote);
                SaveNotes();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Note
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
