namespace MauiAppProject;
using CommunityToolkit.Mvvm.ComponentModel;
public partial class AddUser : ContentPage
{
	private readonly UsersContext context;
    public AddUser()
	{
		InitializeComponent();
		this.context = new();
	}

    private void AddUserButton_Clicked(object sender, EventArgs e)
    {
		context.Users.Add(new User
		{
			Name = this.Name.Text,
			Surname = this.Surname.Text
		});
		context.SaveChanges();
		Navigation.PopAsync();
	}
}