import { Component } from '@angular/core';
import { AddDoctorComponent } from "../add-doctor/add-doctor.component";
import { GetDoctorsComponent } from "../get-doctors/get-doctors.component";
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-registrant-doctors',
  standalone: true,
  imports: [AddDoctorComponent, GetDoctorsComponent, CommonModule, RouterLink],
  templateUrl: './registrant-doctors.component.html',
  styleUrl: './registrant-doctors.component.css'
})
export class RegistrantDoctorsComponent {

  isAddNewDoctorVisible: boolean = false;

  addNewDoctor(){
    this.isAddNewDoctorVisible = true;
  }

}
