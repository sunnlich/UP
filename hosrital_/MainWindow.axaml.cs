using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using hosrital_.Models;
using MySqlConnector;
using hosrital_.Models;

namespace hosrital_;

public partial class MainWindow : Window
{
    private List<Make_an_appointment> Make_an_appointment_ { get; set; }
    private MySqlConnectionStringBuilder _connectionSb;
    private List<Doctor> DoctorList { get; set; }
    private List<Patient_diseases> PatientDiseasesList { get; set; }
    private List<Information_about_diseases> InformationAboutDiseasesList { get; set; }
    private List<Patient_medical_card> PatientMedicalCardsList { get; set; }
    public MainWindow()
    {
        InitializeComponent();
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
        UpdateDataGrid();
        UpdateDoctorGrid();
        UpdatePatientDiseasesGrid();
        UpdateInformationAboutDiseasesGrid();
        UpdatePatientMedicalCardGrid();
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
        GroupsUpdatePatientMedicalCardGrid.ItemsSource = PatientMedicalCardsList;
    }
   public void UpdateDataGrid()
    {
        Make_an_appointment_.Clear();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {                    
                command.CommandText = "SELECT * FROM Make_an_appointment JOIN Patient_medical_card ON Make_an_appointment.Patient = Patient_medical_card.id" +
                                      " JOIN Doctor ON Make_an_appointment.Doctor = Doctor.id ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Make_an_appointment_.Add(new Make_an_appointment                    
                        {
                            Id = reader.GetInt32("Id"),
                            Patient = reader.GetString("LastName"),
                            Doctor = reader.GetString("LastName_"),
                            Date_and_time_of_reception = reader.GetDateTime("Date_and_time_of_reception")
                        });
                    }
                }
            }
            connection.Close();
        }
        GroupsDataGrid.ItemsSource = Make_an_appointment_;
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
        GroupsDoctorGrid.ItemsSource = DoctorList;
    }
    void UpdatePatientDiseasesGrid()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {                    
                command.CommandText = "SELECT * FROM Patient_diseases JOIN Patient_medical_card ON Patient_diseases.PatientId = Patient_medical_card.id " +
                                      "JOIN Information_about_diseases ON Patient_diseases.DiseasesId = Information_about_diseases.id";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PatientDiseasesList.Add(new Patient_diseases                 
                        {
                            Id = reader.GetInt32("Id"),
                            DiseasesId = reader.GetString("Diagnosis"),
                            PatientId = reader.GetString("LastName")
                        });
                    }
                }
            }
            connection.Close();
        }
        GroupsPatientDiseasesGrid.ItemsSource = PatientDiseasesList;
    }
    
    void UpdateInformationAboutDiseasesGrid()
        {
            using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {                    
                    command.CommandText = "SELECT * FROM Information_about_diseases";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InformationAboutDiseasesList.Add(new Information_about_diseases                   
                            {
                                Id = reader.GetInt32("Id"),
                                Symptoms = reader.GetString("Symptoms"),
                                Diagnosis = reader.GetString("Diagnosis"),
                            });
                        }
                    }
                }
                connection.Close();
            }
            GroupsInformationAboutDiseasesGrid.ItemsSource = InformationAboutDiseasesList;
        }

    private void Zapis_OnClick(object? sender, RoutedEventArgs e)
    {
        Zapis zapis = new Zapis(this);
        zapis.Show();
    }

    private void Obnovlenie_OnClick(object? sender, RoutedEventArgs e)
    {
        
        UpdateDataGrid();
    }
    
    private void Bt_chet_OnClick(object? sender, RoutedEventArgs e)
    {
        DateTime startDate = StartDate.SelectedDate.Value.DateTime;
        DateTime endDate = EndDate.SelectedDate.Value.DateTime;
        UpdateDataGrid();
        int patientCount = Make_an_appointment_.Count(a =>
            a.Date_and_time_of_reception >= startDate && a.Date_and_time_of_reception <= endDate);
        chiclo_patientov_box.Text = "";
        chiclo_patientov_box.SelectedText = patientCount.ToString();
    }
}