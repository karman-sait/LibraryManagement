using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.Maui.Controls;

namespace LibraryManagementSystem
{

    //main page of app to show everytinng
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Book> Books { get; } = new ObservableCollection<Book>();
        private BookDbService _bookService = new BookDbService();

        public ICommand EditBookCommand { get; }
        public ICommand DeleteBookCommand { get; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this; 

            LoadBooks();

            EditBookCommand = new Command<Book>(async (book) => await EditBook(book));
            DeleteBookCommand = new Command<Book>(async (book) => await DeleteBook(book));
        }

        private async void LoadBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        }

        private async void OnAddBookClicked(object sender, EventArgs e)
        {
            var newBook = new Book
            {
                Title = titleEntry.Text,
                Author = authorEntry.Text,
                ISBN = isbnEntry.Text,
                Available = availableSwitch.IsToggled
            };

            await _bookService.AddBookAsync(newBook);
            LoadBooks();
            ClearFormFields();
        }

        private void ClearFormFields()
        {
            titleEntry.Text = string.Empty;
            authorEntry.Text = string.Empty;
            isbnEntry.Text = string.Empty;
            availableSwitch.IsToggled = true;
        }

        private async Task EditBook(Book book)
        {
            await _bookService.UpdateBookAsync(book);
            LoadBooks();
        }

        private async Task DeleteBook(Book book)
        {
            bool answer = await DisplayAlert("Delete Book", "Are you sure you want to delete this book?", "Yes", "No");
            if (answer)
            {
                await _bookService.DeleteBookAsync(book.Id);
                LoadBooks();
            }
        }
    }
}
