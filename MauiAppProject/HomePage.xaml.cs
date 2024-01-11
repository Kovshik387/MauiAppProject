using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.System;

namespace MauiAppProject;

public partial class HomePage : ContentPage
{
    private UsersContext usersContext;
    public ObservableCollection<User> Users { get; set; }
    public ICommand EditCommand { get; set; }
    public ICommand SaveCommand { get; set; }
    public ICommand NewCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand UndoCommand { get; set; }
    public HomePage()
	{
		InitializeComponent();

        usersContext = new UsersContext();
        Users = new ObservableCollection<User>();

        EditCommand = new Command(EditHandler);
        SaveCommand = new Command(SaveHandler);
        NewCommand = new Command(NewHandler);
        DeleteCommand = new Command(DeleteHandler);
        UndoCommand = new Command(UndoHandler);

        this.BindingContext = this;
	}

    private void DeleteHandler(object sender)
    {
        if (this.collectionView.SelectedItem is User user)
        {
            this.usersContext.Users.Remove(user);
        }
    }

    private void UndoHandler(object sender)
    {
        var users = usersContext.Users.Include(x => x.CompanyNavigation).ToList();
        this.collectionView.ItemsSource =
            new ObservableCollection<User>(users);
    }

    private void EditHandler(object sender)
    {
        if (this.collectionView.SelectedItem is User user)
        {
            this.usersContext.Users.Update(user);
        }
    }

    private void SaveHandler(object sender) =>
        this.usersContext.SaveChanges();

    protected async void NewHandler(object sender)
    {
        await Navigation.PushAsync(new AddUser());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var users = usersContext.Users.Include(x => x.CompanyNavigation).ToList();
        this.collectionView.ItemsSource = new ObservableCollection<User>(users);
    }

    private async void SearchButton_Clicked(object sender, EventArgs e)
    {
        if (Surname.Text is null && Company.Text is null)
        {
            var users = usersContext.Users.Include(x => x.CompanyNavigation).ToList();
            this.collectionView.ItemsSource = new ObservableCollection<User>(users);
            await DisplayAlert("Ошибка", "Данные не были введены", "Ок");
        }
        else if (Surname is not null)
        {
            var search_user = usersContext.Users.Include(x => x.CompanyNavigation).
                Where(x => EF.Functions.Like(x.Surname, $"{this.Surname}")).
                ToList();
            this.collectionView.ItemsSource = new ObservableCollection<User>(search_user);
        }
        else if (Company is not null)
        {
            var search_user = usersContext.Users.Include(x => x.CompanyNavigation).
                Where(x => EF.Functions.Like(x.CompanyNavigation.Name, $"{this.Company}")).
                ToList();
            this.collectionView.ItemsSource = new ObservableCollection<User>(search_user);
        }
        else
        {
            var search_user = usersContext.Users.Include(x => x.CompanyNavigation).
               Where(x => EF.Functions.Like(x.CompanyNavigation.Name, $"{this.Company}") &&
               EF.Functions.Like(x.Surname, $"{this.Surname}")
               ).
               ToList();
            this.collectionView.ItemsSource = new ObservableCollection<User>(search_user);
        }
    }
}