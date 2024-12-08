import { Component } from '@angular/core';
import { AddDoctorComponent } from "../add-doctor/add-doctor.component";
import { GetDoctorsComponent } from "../get-doctors/get-doctors.component";

@Component({
  selector: 'app-registrant-doctors',
  standalone: true,
  imports: [AddDoctorComponent, GetDoctorsComponent],
  templateUrl: './registrant-doctors.component.html',
  styleUrl: './registrant-doctors.component.css'
})
export class RegistrantDoctorsComponent {

  isAddNewDoctorVisible: boolean = false;

  addNewDoctor(){
    this.isAddNewDoctorVisible = true;
  }

}
