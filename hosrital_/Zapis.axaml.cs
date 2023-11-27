using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using hosrital_.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySqlConnector;

namespace hosrital_;

public partial class Zapis : Window
{
    private List<Make_an_appointment> Make_an_appointment_ { get; set; }
    private MySqlConnectionStringBuilder _connectionSb;
    private List<Doctor> DoctorList { get; set; }
    private List<Patient_diseases> PatientDiseasesList { get; set; }
    private List<Information_about_diseases> InformationAboutDiseasesList { get; set; }
    private List<Patient_medical_card> PatientMedicalCardsList { get; set; }
    private Window window;
    public Zapis(Window win)
    {
        InitializeComponent();
        window = win;
        Make_an_appointment_ = new List<Make_an_appointment>();
        DoctorList = new List<Doctor>();
        PatientDiseasesList = new List<Patient_diseases>();
        InformationAboutDiseasesList = new List<Information_about_diseases>();
        PatientMedicalCardsList = new List<Patient_medical_card>();
        _connectionSb = new MySqlConnectionStringBuilder()
        {            
            Server = "localhost",
            Database = "hospital",            
            UserID = "root",
            Password = "3456"
        };
        UpdatePatientMedicalCardGrid();
        UpdateDoctorGrid();
    }
    void UpdatePatientMedicalCardGrid()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {                    
                command.CommandText = "SELECT * FROM Patient_medical_card";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PatientMedicalCardsList.Add(new Patient_medical_card
                        {
                            Id = reader.GetInt32("Id"),
                            LastName = reader.GetString("LastName"),
                            Name = reader.GetString("Name"),
                            FatherName = reader.GetString("FatherName"),
                            Age = reader.GetInt32("Age"),
                        });
                    }
                }
            }
            connection.Close();
        }
        PatientsComboBox.ItemsSource = PatientMedicalCardsList;
    }
    void UpdateDoctorGrid()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {                    
                command.CommandText = "SELECT * FROM Doctor";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorList.Add(new Doctor                    
                        {
                            Id = reader.GetInt32("Id"),
                            LastName_ = reader.GetString("LastName_"),
                            Name_ = reader.GetString("Name_"),
                            FatherName_ = reader.GetString("FatherName_"),
                            Room_number = reader.GetInt32("Room_number")
                        });
                    }
                }
            }
            connection.Close();
        }
        DoctorComboBox.ItemsSource = DoctorList;
    }
    
    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        DateTime selectedDate = Date.SelectedDate.Value.DateTime;
        
        using (var c = new MySqlConnection(_connectionSb.ConnectionString))
        {
            c.Open();
            using (var cmd = c.CreateCommand())
            {
                cmd.CommandText =
                    "INSERT INTO Make_an_appointment (Patient, Doctor, Date_and_time_of_reception)" +
                    "VALUES (@Patient, @Doctor, @Date_and_time_of_reception)";

                cmd.Parameters.AddWithValue("@Patient", (( PatientsComboBox.SelectedItem) as Patient_medical_card).Id);
                cmd.Parameters.AddWithValue("@Doctor", ((DoctorComboBox.SelectedItem) as Doctor).Id);
                cmd.Parameters.AddWithValue("@Date_and_time_of_reception", selectedDate); 


                var rowsCount = cmd.ExecuteNonQuery();
                if (rowsCount == 0) ;
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("Успех", "Удалось выполнить добавление!",
                        ButtonEnum.Ok);
                    box.ShowAsync();
                }
            }
            c.Close();
        }
        
        this.Close();
    }
    
}