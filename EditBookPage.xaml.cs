using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{

    //page for editing the deatials in the book
    public partial class EditBookPage : ContentPage
    {
        private readonly BookDbService _bookService = new BookDbService();
        private Book _currentBook;

        public EditBookPage(Book book)
        {
            InitializeComponent();
            _currentBook = book;
            LoadBookDetails();
        }

        //loads the details into the form feilds from the current book object
        private void LoadBookDetails()
        {
            titleEntry.Text = _currentBook.Title;
            authorEntry.Text = _currentBook.Author;
            isbnEntry.Text = _currentBook.ISBN;
            availableSwitch.IsToggled = _currentBook.Available;
        }


        //saves changes made to the book in database 
        private async void OnSaveChangesClicked(object sender, EventArgs e)
        {
            _currentBook.Title = titleEntry.Text;
            _currentBook.Author = authorEntry.Text;
            _currentBook.ISBN = isbnEntry.Text;
            _currentBook.Available = availableSwitch.IsToggled;

            await _bookService.UpdateBookAsync(_currentBook);
            await Navigation.PopAsync(); 
        }

        //cancels editing nad returns to last page
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); 
        }
    }
}
