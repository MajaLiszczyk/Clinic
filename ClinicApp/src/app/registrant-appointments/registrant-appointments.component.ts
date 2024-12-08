import { Component } from '@angular/core';
import { AddMedicalAppointmentComponent } from "../add-medical-appointment/add-medical-appointment.component";
import { GetMedicalAppointmentsComponent } from "../get-medical-appointments/get-medical-appointments.component";
import { RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-registrant-appointments',
  standalone: true,
  imports: [AddMedicalAppointmentComponent, GetMedicalAppointmentsComponent, RouterLink, CommonModule],
  templateUrl: './registrant-appointments.component.html',
  styleUrl: './registrant-appointments.component.css'
})
export class RegistrantAppointmentsComponent {
  isAddNewAppointmentVisible: boolean = false;
  isGetAllAppointmentsVisible: boolean = false;


  addNewAppointment(){
    this.isAddNewAppointmentVisible = true;
  }
  getAllAppointments(){
    this.isGetAllAppointmentsVisible = true;
  }

}
