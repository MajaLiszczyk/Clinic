import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AuthorizationService } from '../services/authorization.service';
import { ClinicService } from '../services/clinic.service';
import { Patient } from '../model/patient';

@Component({
  selector: 'app-patient-menu',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './patient-menu.component.html',
  styleUrl: './patient-menu.component.css'
})
export class PatientMenuComponent {
  patientId: number = 0;
  patient: Patient;

  constructor(private clinicService: ClinicService, private route: ActivatedRoute, public authorizationService: AuthorizationService,) {
    this.patient = { name: '', surname: '', id: 0, pesel: '', patientNumber: '', isAvailable: true };
  }
  ngOnInit() {
    this.route.params.subscribe(params => {
      this.patientId = +params['patientId'];
      console.log('Received patientId:', this.patientId);
      this.getPatientById(this.patientId);
    });
  }

  getPatientById(patientId: number) {
    this.clinicService.getPatientById(patientId).subscribe(data => {
      this.patient = data;
    })
  }

  logout() {
    this.authorizationService.logout();
  }
}
