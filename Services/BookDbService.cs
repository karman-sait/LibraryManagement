using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LibraryManagementSystem.Models;
using MySql.Data.MySqlClient;

namespace LibraryManagementSystem.Services
{

    //Service class for performing database operations related to "BOOKS"
    public class BookDbService
    {
        private readonly string _connectionString = "server=localhost;database=library_management;user=root;password=password;";

        //Initializes the books table in the library_management databse in case it doesn't exist
        public async Task InitializeDatabaseAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = @"CREATE TABLE IF NOT EXISTS Books (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Title VARCHAR(255) NOT NULL,
                        Author VARCHAR(255) NOT NULL,
                        ISBN VARCHAR(20) UNIQUE,
                        Available BOOLEAN DEFAULT TRUE
                    );";
            await connection.ExecuteAsync(sql);
        }

        //Adds a new book to the database
        public async Task AddBookAsync(Book book)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                var sql = "INSERT INTO Books (Title, Author, ISBN, Available) VALUES (@Title, @Author, @ISBN, @Available);";
                await connection.ExecuteAsync(sql, book);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error in AddBookAsync: {ex.Message}");
            }
        }

        //Updates an existing Book
        public async Task UpdateBookAsync(Book book)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                var sql = "UPDATE Books SET Title = @Title, Author = @Author, ISBN = @ISBN, Available = @Available WHERE Id = @Id;";
                await connection.ExecuteAsync(sql, book);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error in UpdateBookAsync: {ex.Message}");
            }
        }


        //Deletes a book
        public async Task DeleteBookAsync(int bookId)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                var sql = "DELETE FROM Books WHERE Id = @Id;";
                await connection.ExecuteAsync(sql, new { Id = bookId });
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error in DeleteBookAsync: {ex.Message}");
            }
        }

        //Retrieves a book (doesn't work yet)
        public async Task<List<Book>> GetAllBooksAsync()
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                var sql = "SELECT * FROM Books;";
                var result = await connection.QueryAsync<Book>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching books: {ex.Message}");
                return new List<Book>();
            }
        }
    }
}