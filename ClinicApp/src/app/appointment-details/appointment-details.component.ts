import { Component } from '@angular/core';
import { MedicalAppointment } from '../model/medical-appointment';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { RouterLink, RouterOutlet } from '@angular/router';



@Component({
  selector: 'app-appointment-details',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, RouterLink],
  templateUrl: './appointment-details.component.html',
  styleUrl: './appointment-details.component.css'
})
export class AppointmentDetailsComponent {
  appointmentId: number = 0;
  medicalAppointment: MedicalAppointment = { id: 0, dateTime: '', patientId: 0, doctorId: 0, interview:'', diagnosis: '', diseaseUnit: 0 };
  readonly APIUrl="https://localhost:5001/api/MedicalAppointment";

  myForm: FormGroup;


  constructor(private http:HttpClient, private route: ActivatedRoute, private fb: FormBuilder){
    this.myForm = this.fb.group({
      interviewText: [''], // Domyślna wartość
      diagnosisText: [''],
    });
  }


  /*ngOnInit(){
    this.route.params.subscribe(params => {
      this.medicalAppointment = params['medicalAppointment']; // Przypisanie id z URL
      console.log('Received Med Ap', this.medicalAppointment);
    });
    this.getMedicalAppointmentsForDoctor(this.medicalAppointment.id)
  } */

  ngOnInit(){
    console.log('Received med ap Id :', this.appointmentId);

    this.route.params.subscribe(params => {
      this.appointmentId = +params['appointmentId']; // Przypisanie id z URL
      console.log('Received appointmentId:', this.appointmentId);
    });
    this.getMedicalAppointmentsDetails(this.appointmentId)

  } 

  get interview(): string {
    return this.myForm.get('interviewText')?.value;
  }

  get diagnosis(): string {
    return this.myForm.get('diagnosisText')?.value;
  }


  getMedicalAppointmentsDetails(appointmentId: number){
    this.http.get<MedicalAppointment>(this.APIUrl+"/Get/" + appointmentId).subscribe(data =>{
      this.medicalAppointment=data;
    })
  }

  saveAnAppointment(){
    this.medicalAppointment.diagnosis = this.diagnosis;
    this.medicalAppointment.interview = this.interview;
    this.http.put<MedicalAppointment>(this.APIUrl+"/update", this.medicalAppointment)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });

  }


  
  cancelAnAppointment(){

  }


}
