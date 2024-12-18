import { Component } from '@angular/core';
import { AddDoctorComponent } from "../add-doctor/add-doctor.component";
import { GetDoctorsComponent } from "../get-doctors/get-doctors.component";
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-registrant-doctors-old',
  standalone: true,
  imports: [AddDoctorComponent, GetDoctorsComponent, CommonModule, RouterLink],
  templateUrl: './registrant-doctors-old.component.html',
  styleUrl: './registrant-doctors-old.component.css'
})
export class RegistrantDoctorsOldComponent {

  isAddNewDoctorVisible: boolean = false;

  addNewDoctor(){
    this.isAddNewDoctorVisible = true;
  }

}
